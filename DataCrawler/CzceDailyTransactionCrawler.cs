using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesDataCrawler
{
    public class CzceDailyTransactionCrawler : CzceDataCrawler
    {
        protected override Uri BuildUrl(DateTime transactionDate)
        {
            string url = ConfigurationManager.AppSettings["Czce.Transaction.Url"];

            return BuildUrl(url, transactionDate);
        }
    }
}
