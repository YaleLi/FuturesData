using DataType;

namespace DataType
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

        public ContractTransactionInfo(DateTime date, string exchange, string commodity, string contract, double open, 
                double high, double low, double close, double settle, int volume, int position)
        {
            TransactionDate = date;
            Commodity = commodity;
            Exchange = exchange;
            Contract = contract;
            OpenPrice = open;
            HighPrice = high;
            LowPrice = low;
            ClosePrice = close;
            SettlePrice = settle;
            Volume = volume;
            Position = position;

            ID = Exchange + "_" + Commodity + Contract + "_" + TransactionDate.ToString(GlobalDefinition.DateFormat, GlobalDefinition.FormatProvider);
        }

        public bool Equals(ContractTransactionInfo another)
        {
            if (null == another)
            {
                return false;
            }

            if (this == another)
            {
                return true;
            }
            return TransactionDate.Equals(another.TransactionDate) && Commodity.Equals(another.Commodity) && 
                   OpenPrice.Equals(another.OpenPrice) && HighPrice.Equals(another.HighPrice) && 
                   LowPrice.Equals(another.LowPrice) && ClosePrice.Equals(another.ClosePrice) && SettlePrice.Equals(another.SettlePrice) &&
                   Volume==another.Volume && Position==another.Volume;
        }
    }
}
