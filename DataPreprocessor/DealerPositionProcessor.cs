using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;

namespace DataPreprocessor
{
    enum PositionType
    {
        Buy,
        Sell
    }
    class DealerPositionProcessor :FutureDataPreprocessor
    {
        private int GetAggregatedPosition(DealerPositionInfo info, int sum, PositionType type)
        {
            if (null == info)
            {
                return 0;
            }

            string positionString = null;
            switch (type)
            {
                case PositionType.Buy:
                    positionString = info.BuyDealers;
                    break;
                case PositionType.Sell:
                    positionString = info.SellDealers;
                    break;
            }

            if (string.IsNullOrWhiteSpace(positionString))
            {
                return 0;
            }

            var dealers = positionString.Split(new char[] {';'});
            int length = dealers.Length < sum ? dealers.Length : sum;

            int result = 0;
            for (int i = 0; i < length; i++)
            {
                var pair = dealers[i].Split(new char[] {'='}, StringSplitOptions.RemoveEmptyEntries);
                if (pair.Length < 2)
                {
                    continue;
                }
                result = result + Convert.ToInt32(pair[1]);
            }
            return result;
        }
        private DealerPositionFeature GenerateFeatures(ContractTransactionInfo info, IEnumerable<DealerPositionInfo> contextData)
        {
            if (0 == info.Volume || 0 == info.Position || info.OpenPrice <= 0 || info.ClosePrice <= 0 || info.SettlePrice <= 0)
            {
                return null;
            }

            var positionContext = contextData.Where(d => d.Commodity.Equals(info.Commodity) && d.Month.Equals(info.Contract)).OrderBy(d => d.TransactionDate).ToArray();
            int index = 0;
            int length = positionContext.Length;
            for (; index < length; index++)
            {
                if (positionContext[index].TransactionDate.Equals(info.TransactionDate))
                {
                    break;
                }
            }

            if (index >= length)
            {
                return null;
            }

            DealerPositionFeature feature = new DealerPositionFeature();

            feature.Id = BuildId(info.TransactionDate, info.Commodity);
            feature.Volume = info.Volume;

            double buyPosition20 = GetAggregatedPosition(positionContext[index], 20, PositionType.Buy);
            double sellPosition20 = GetAggregatedPosition(positionContext[index], 20, PositionType.Sell);
            double buyPosition5 = GetAggregatedPosition(positionContext[index], 5, PositionType.Buy);
            double sellPosition5 = GetAggregatedPosition(positionContext[index], 5, PositionType.Sell);
            if (index > 10)
            {
                var historicalData = positionContext[index - 10];
                feature.TopBuy20Delta10 = CalculateRatio(GetAggregatedPosition(historicalData, 20, PositionType.Buy), buyPosition20);
                feature.TopSell20Delta10 = CalculateRatio(GetAggregatedPosition(historicalData, 20, PositionType.Sell), sellPosition20); 
                feature.TopBuy5Delta10 = CalculateRatio(GetAggregatedPosition(historicalData, 5, PositionType.Buy), buyPosition5); 
                feature.TopSell5Delta10 = CalculateRatio(GetAggregatedPosition(historicalData, 5, PositionType.Sell), sellPosition5); 
            }

            if (index > 3)
            {
                var historicalData = positionContext[index - 3];
                feature.TopBuy20Delta3 = CalculateRatio(GetAggregatedPosition(historicalData, 20, PositionType.Buy), buyPosition20);
                feature.TopSell20Delta3 = CalculateRatio(GetAggregatedPosition(historicalData, 20, PositionType.Sell), sellPosition20);
                feature.TopBuy5Delta3 = CalculateRatio(GetAggregatedPosition(historicalData, 5, PositionType.Buy), buyPosition5); 
                feature.TopSell5Delta3 = CalculateRatio(GetAggregatedPosition(historicalData, 5, PositionType.Sell), sellPosition5);

            }

            if (index > 0)
            {
                var historicalData = positionContext[index - 1];
                feature.TopBuy20Delta1 = CalculateRatio(GetAggregatedPosition(historicalData, 20, PositionType.Buy), buyPosition20);
                feature.TopSell20Delta1 = CalculateRatio(GetAggregatedPosition(historicalData, 20, PositionType.Sell), sellPosition20);
                feature.TopBuy5Delta1 = CalculateRatio(GetAggregatedPosition(historicalData, 5, PositionType.Buy), buyPosition5);
                feature.TopSell5Delta1 = CalculateRatio(GetAggregatedPosition(historicalData, 5, PositionType.Sell), sellPosition5);
            }

            return feature;
        }
        public void ProcessData(DateTime start, DateTime end)
        {
            var transactionData = GetTopTransactionInfo(start, end);

            int count = 0;


            if (transactionData.Any())
            {
                var positionData = GetDealerPositionInfo(start.AddDays(BACK_CONTEXT_DAYS), end);

                foreach (var topContract in transactionData)
                {
                    var row = GenerateFeatures(topContract, positionData);

                    if (null != row)
                    {
                        DataStore.DealerPositionFeatures.AddOrUpdate(row);
                        System.Console.WriteLine(row.Id + " built....");
                        ++count;
                    }

                    if (count >= 100)
                    {
                        DataStore.SaveChanges();
                        count = 0;
                    }
                }
                DataStore.SaveChanges();
            }
        }
    }
}
