using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;

namespace DataPreprocessor
{
    public sealed class ContractTransactionProcessor
    {
        private FuturesDataStore dataStore = null;


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
            var endDate = end.Date.AddDays(1);
            var originalData = dataStore.ContractTransactionInfoes.Where(d => d.TransactionDate>= startDate && d.TransactionDate < endDate);

            if (null != originalData)
            {
                
            }
        }
   }
}
