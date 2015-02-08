using System.Globalization;
using DataType;
using Utility;

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

        public double OpenPrice { get;  set; }

        public double HighPrice { get; set; }

        public double LowPrice { get;  set; }

        public double ClosePrice { get;  set; }

        public double SettlePrice { get;  set; }

        public int Volume { get;  set; }

        public int Position { get;  set; }

        public ContractTransactionInfo() { }
        public ContractTransactionInfo(DateTime date, string exchange, string commodity, string contract, double open, 
                double high, double low, double close, double settle, int volume, int position)
        {
            TransactionDate = date;
            Commodity = null == commodity ? "" : commodity.ToLower(new CultureInfo("en-US"));
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

            string dateString1 = TransactionDate.ToString(GlobalDefinition.DateFormat, GlobalDefinition.FormatProvider);
            string dateString2 = another.TransactionDate.ToString(GlobalDefinition.DateFormat, GlobalDefinition.FormatProvider);
            return dateString1.Equals(dateString2) && Commodity.Equals(another.Commodity) && 
                   DoubleUtility.Equals(OpenPrice, another.OpenPrice) && DoubleUtility.Equals(HighPrice, another.HighPrice) && 
                   DoubleUtility.Equals(LowPrice, another.LowPrice) && DoubleUtility.Equals(ClosePrice, another.ClosePrice) && 
                   DoubleUtility.Equals(SettlePrice, another.SettlePrice) && Volume==another.Volume && Position==another.Position;
        }
    }
}
