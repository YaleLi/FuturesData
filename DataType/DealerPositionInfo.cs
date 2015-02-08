using System.Globalization;

namespace DataType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DealerPositionInfo")]
    public partial class DealerPositionInfo
    {
        public string Id { get;  set; }

        public DateTime TransactionDate { get;  set; }

        public string Commodity { get;  set; }

        public string Month { get;  set; }

        public string VolumeDealers { get;  set; }

        public string BuyDealers { get;  set; }

        public string SellDealers { get;  set; }

        public DealerPositionInfo() { }
        public DealerPositionInfo(DateTime transactionDate, string commodity, string month, string volumeDealers,
            string buyDealers, string sellDealers)
        {
            TransactionDate = transactionDate;
            Commodity = null == commodity ? "" : commodity.ToLower(new CultureInfo("en-US"));
            Month = month;
            VolumeDealers = volumeDealers;
            BuyDealers = buyDealers;
            SellDealers = sellDealers;

            Id = Commodity + Month + "_" + TransactionDate.ToString(GlobalDefinition.DateFormat, GlobalDefinition.FormatProvider);
        }

        public bool Equals(DealerPositionInfo another)
        {
            if (null == another)
            {
                return false;
            }

            if (this == another)
            {
                return true;
            }
            string tmpDate1 = TransactionDate.ToString(GlobalDefinition.DateFormat, GlobalDefinition.FormatProvider);
            string tmpDate2 = another.TransactionDate.ToString(GlobalDefinition.DateFormat, GlobalDefinition.FormatProvider);

            return tmpDate1.Equals(tmpDate2) && Commodity.Equals(another.Commodity) && Month.Equals(another.Month) &&
                   VolumeDealers.Equals(another.VolumeDealers) && BuyDealers.Equals(another.BuyDealers) &&
                   SellDealers.Equals(another.SellDealers);
        }
    }
}
