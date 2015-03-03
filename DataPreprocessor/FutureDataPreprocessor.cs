using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;

namespace DataPreprocessor
{
    public class FutureDataPreprocessor
    {
        protected FuturesDataStore DataStore { get; private set;}
        protected static int BACK_CONTEXT_DAYS = -60;
        protected static int FORWARD_CONTEXT_DAYS = 30;

        private bool CheckPreCondition(DateTime start, DateTime end)
        {
            if (end < start)
            {
                return false;
            }

            if (null == DataStore)
            {
                DataStore = new FuturesDataStore(ConfigurationManager.ConnectionStrings["CloudDBConnect"].ConnectionString);
            }

            return true;
        }
        protected ContractTransactionInfo[] GetTransactionInfo(DateTime start, DateTime end)
        {
            if (!CheckPreCondition(start, end))
            {
                return new ContractTransactionInfo[0];
            }

            return DataStore.ContractTransactionInfoes.Where(d => d.TransactionDate >= start && d.TransactionDate <= end).ToArray();
        }

        protected ContractTransactionInfo[] GetTopTransactionInfo(DateTime start, DateTime end)
        {
            var originalData = GetTransactionInfo(start, end);

            var result = new List<ContractTransactionInfo>();
            foreach (var dailyContracts in originalData.GroupBy(d => d.TransactionDate))
            {
                foreach (var dailyCommodities in dailyContracts.GroupBy(d => d.Commodity))
                {
                    result.Add(dailyCommodities.OrderByDescending(d => d.Volume).First());
                }
            }

            return result.ToArray();
        }


        protected DealerPositionInfo[] GetDealerPositionInfo(DateTime start, DateTime end)
        {
            if (!CheckPreCondition(start, end))
            {
                return new DealerPositionInfo[0];
            }
            return DataStore.DealerPositionInfoes.Where(d => d.TransactionDate >= start && d.TransactionDate <= end).ToArray();
        }

        protected string BuildId(DateTime date, string commodity)
        {
            return "_N" + date.ToString(GlobalDefinition.DateFormat, GlobalDefinition.FormatProvider) + "_" + commodity;
        }

        protected double CalculateRatio(double baseNumber, double newNumber)
        {
            return (baseNumber - newNumber)/(baseNumber + 0.001);
        }
    }
}
