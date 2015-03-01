using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Migrations;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;

namespace DataPreprocessor
{
    public sealed class ContractTransactionProcessor
    {
        private FuturesDataStore dataStore = null;

        private ContractTransactionFeature GenerateFeatures(ContractTransactionInfo info, IEnumerable<ContractTransactionInfo> contextData)
        {
            if (0 == info.Volume || 0 == info.Position || info.OpenPrice <= 0 || info.ClosePrice <= 0 || info.SettlePrice <= 0)
            {
                return null;
            }
            var contractContext = contextData.Where(d => d.Commodity.Equals(info.Commodity) && d.Contract.Equals(info.Contract)).OrderBy(d=>d.TransactionDate).ToArray();
            int index = 0;
            int length = contractContext.Length;
            for (; index < length; index++)
            {
                if (contractContext[index].TransactionDate.Equals(info.TransactionDate))
                {
                    break;
                }
            }

            if (index >= length)
            {
                throw new ObjectNotFoundException(info.ID + " not found in contextual data array.");
            }

            var featureRow = new ContractTransactionFeature();
            featureRow.Id = "_N" + info.TransactionDate.ToString(GlobalDefinition.DateFormat, GlobalDefinition.FormatProvider) + "_" + info.Commodity;
            featureRow.Volume = info.Volume;
            var closePrice = info.ClosePrice + 0.01;
            if (index + 10 < length)
            {
                featureRow.ReturnRate10 = 100.0 * (contractContext[index + 10].ClosePrice - closePrice)/closePrice;
            }

            if (index + 5 < length)
            {
                featureRow.ReturnRate5 = 100.0 * (contractContext[index + 5].ClosePrice - closePrice) / closePrice;
            }

            if (index + 1 < length)
            {
                featureRow.ReturnRate1 = 100.0 * (contractContext[index + 1].ClosePrice - closePrice) / closePrice;
            }

            featureRow.DeltaHighOpen = 100.0 * (info.HighPrice - info.OpenPrice)/info.OpenPrice;
            featureRow.DeltaLowOpen = 100.0 * (info.LowPrice - info.OpenPrice)/info.OpenPrice;
            featureRow.DeltaHighClose = 100.0 * (info.HighPrice - info.ClosePrice) / info.HighPrice;
            featureRow.DeltaLowClose = 100.0 * (info.LowPrice - info.ClosePrice) / info.LowPrice;

            var volume = info.Volume;
            var position = info.Position;
            IEnumerable<ContractTransactionInfo> historyData;
            if (index > 20)
            {
                historyData = contractContext.Skip(index - 20).Take(20);
                var averageVolume = historyData.Average(d => d.Volume) + 1;
                var averagePosition = historyData.Average(d => d.Position) + 1;
                var averageClosePrice = historyData.Average(d => d.ClosePrice) + 0.01;
                var averageSettlePrice = historyData.Average(d => d.SettlePrice) + 0.01;

                featureRow.DeltaVolume20 = 100.0 * (volume - averageVolume)/averageVolume;
                featureRow.DeltaPosition20 = 100.0 * (position - averagePosition)/averagePosition;
                featureRow.DeltaClosePrice20 = 100.0 * (info.ClosePrice - averageClosePrice)/averageClosePrice;
                featureRow.DeltaSettlePrice20 = 100.0 * (info.SettlePrice - averageSettlePrice)/averageSettlePrice;
                featureRow.DeltaCloseSettlePrice20 = 100.0 * (info.ClosePrice - averageSettlePrice)/averageSettlePrice;
            }
            if (index > 10)
            {
                historyData = contractContext.Skip(index - 10).Take(10);
                var averageVolume = historyData.Average(d => d.Volume) + 1;
                var averagePosition = historyData.Average(d => d.Position) + 1;
                var averageClosePrice = historyData.Average(d => d.ClosePrice) + 0.01;
                var averageSettlePrice = historyData.Average(d => d.SettlePrice) + 0.01;

                featureRow.DeltaVolume10 = 100.0 * (volume - averageVolume) / averageVolume;
                featureRow.DeltaPosition10 = 100.0 * (position - averagePosition) / averagePosition;
                featureRow.DeltaClosePrice10 = 100.0 * (info.ClosePrice - averageClosePrice) / averageClosePrice;
                featureRow.DeltaSettlePrice10 = 100.0 * (info.SettlePrice - averageSettlePrice) / averageSettlePrice;
                featureRow.DeltaCloseSettlePrice10 = 100.0 * (info.ClosePrice - averageSettlePrice) / averageSettlePrice;
            }
            if (index > 5)
            {
                historyData = contractContext.Skip(index - 5).Take(5);
                var averageVolume = historyData.Average(d => d.Volume) + 1;
                var averagePosition = historyData.Average(d => d.Position) + 1;
                var averageClosePrice = historyData.Average(d => d.ClosePrice) + 0.01;
                var averageSettlePrice = historyData.Average(d => d.SettlePrice) + 0.01;

                featureRow.DeltaVolume5 = 100.0 * (volume - averageVolume) / averageVolume;
                featureRow.DeltaPosition5 = 100.0 * (position - averagePosition) / averagePosition;
                featureRow.DeltaClosePrice5 = 100.0 * (info.ClosePrice - averageClosePrice) / averageClosePrice;
                featureRow.DeltaSettlePrice5 = 100.0 * (info.SettlePrice - averageSettlePrice) / averageSettlePrice;
                featureRow.DeltaCloseSettlePrice5 = 100.0 * (info.ClosePrice - averageSettlePrice) / averageSettlePrice;
            }
            if (index > 0)
            {
                var lastInfo = contractContext[index - 1];
                featureRow.DeltaVolume = 100.0 * ((double) info.Volume - lastInfo.Volume)/(lastInfo.Volume+1);
                featureRow.DeltaPosition = 100.0 * ((double)info.Position - lastInfo.Position) / (lastInfo.Position+1);
                featureRow.DeltaClosePrice = 100.0 * (info.ClosePrice - lastInfo.ClosePrice)/(lastInfo.ClosePrice + 0.01);
                featureRow.DeltaSettlePrice = 100.0 * (info.SettlePrice - lastInfo.SettlePrice)/(lastInfo.SettlePrice + 0.01);

            }

            System.Console.WriteLine(featureRow.Id + " built successfully....");
            return featureRow;
        }

        public void ProcessData(DateTime start, DateTime end)
        {
            if (end < start)
            {
                return;
            }

            if (null == dataStore)
            {
                dataStore = new FuturesDataStore(ConfigurationManager.ConnectionStrings["CloudDBConnect"].ConnectionString);
            }

            var startDate = start.Date;
            var endDate = end.Date;
            var originalData = dataStore.ContractTransactionInfoes.Where(d => d.TransactionDate>= startDate && d.TransactionDate <= endDate).ToArray();

            if (originalData.Any())
            {
                startDate = startDate.AddDays(-60);
                endDate = endDate.AddDays(30);

                int counter = 0;
                var featureRows = new List<ContractTransactionFeature>();
                var contextData = dataStore.ContractTransactionInfoes.Where(d => d.TransactionDate >= startDate && d.TransactionDate < endDate).ToList();
                foreach (var dailyContracts in originalData.GroupBy(d => d.TransactionDate))
                {
                    foreach (var dailyCommodities in dailyContracts.GroupBy(d => d.Commodity))
                    {
                        var topContract = dailyCommodities.OrderByDescending(d => d.Volume).First();
                        var row = GenerateFeatures(topContract, contextData);
                        if (null != row)
                        {
                            featureRows.Add(row);
                        }

                        if (featureRows.Count >= 100)
                        {
                            dataStore.ContractTransactionFeatures.AddRange(featureRows);
                            //dataStore.ContractTransactionFeatures.AddOrUpdate(featureRows.First());
                            dataStore.SaveChanges();
                            featureRows.Clear();
                        }
                    }
                }

                if (featureRows.Any())
                {
                    dataStore.ContractTransactionFeatures.AddRange(featureRows);
                    dataStore.SaveChanges();
                    featureRows.Clear();
                }
                
            }
        }
   }
}
