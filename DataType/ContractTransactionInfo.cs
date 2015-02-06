namespace ModelFirstTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContractTransactionInfo")]
    public partial class ContractTransactionInfo
    {
        public string ID { get; set; }

        public DateTime TransactionDate { get; set; }

        public string Commodity { get; set; }

        public string Exchange { get; set; }

        public string Contract { get; set; }

        public double OpenPrice { get; set; }

        public double HighPrice { get; set; }

        public double LowPrice { get; set; }

        public double ClosePrice { get; set; }

        public double SettlePrice { get; set; }

        public int Volume { get; set; }

        public int Position { get; set; }
    }
}
