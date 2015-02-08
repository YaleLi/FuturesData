using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;

namespace FuturesDataTest
{
    enum InformationType
    {
        VolumeInfo,
        BuyInfo,
        SellInfo
    }
    class SingleDealerPosition
    {
        public DateTime TransactionDate { get; set; }

        public string Commodity { get; set; }

        public string Month { get; set; }

        public InformationType DealerInformationType { get; set; }

        public int Rank { get; set; }

        public string DealerName { get; set; }
        public int Amount { get; set; }

        public SingleDealerPosition(DateTime transactionDate, string commodity, string month, InformationType type,
            int rank, string dealer, int amount)
        {
            TransactionDate = transactionDate;
            Commodity = commodity.ToLower();
            Month = month;
            DealerInformationType = type;
            Rank = rank;
            DealerName = dealer;
            Amount = amount;
        }

        public bool ExistIn(DealerPositionInfo another)
        {
            if (null == another)
            {
                return false;
            }

            string date1 = TransactionDate.ToString(GlobalDefinition.DateFormat, GlobalDefinition.FormatProvider);
            string date2 = another.TransactionDate.ToString(GlobalDefinition.DateFormat, GlobalDefinition.FormatProvider);
            bool result = date1.Equals(date2) && Commodity.Equals(another.Commodity) && Month.Equals(another.Month);

            string dealerList = "";
            if (result)
            {
                switch (DealerInformationType)
                {
                    case InformationType.VolumeInfo:
                        dealerList = another.VolumeDealers;
                        break;
                    case InformationType.BuyInfo:
                        dealerList = another.BuyDealers;
                        break;
                    case InformationType.SellInfo:
                        dealerList = another.SellDealers;
                        break;
                }
                string dealer;
                int amount;
                ExtractDealer(Rank, dealerList, out dealer, out amount);

                result = DealerName.Equals(dealer) && Amount == amount;
            }

            return result;
        }

        private void ExtractDealer(int rank, string dealerList, out string dealer, out int amount)
        {
            dealer = "";
            amount = -1;
            if (string.IsNullOrEmpty(dealerList) || rank<1)
            {
                return;
           }

            var pairs = dealerList.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            if (null == pairs || pairs.Length < rank)
            {
                return;
            }
            var pair = pairs[rank - 1].Split(new char[] {'='}, StringSplitOptions.RemoveEmptyEntries);
            if (null != pair || pair.Length == 2)
            {
                dealer = pair[0];
                amount = Int32.Parse(pair[1], NumberStyles.Any);
            }

        }
    }
}
