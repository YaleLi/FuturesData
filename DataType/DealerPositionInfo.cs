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
        public string Id { get; set; }

        public DateTime TransactionDate { get; set; }

        public string Commodity { get; set; }

        public string Month { get; set; }

        public string VolumeDealers { get; set; }

        public string BuyDealers { get; set; }

        public string SellDealers { get; set; }
    }
}
