using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesDataCrawler
{
    public class DceDailyTransactionCrawler :DceDataCrawler
    {
        protected override Uri BuildUrl(DateTime transactionDate)
        {
            string url = ConfigurationManager.AppSettings["Dce.Transaction.Url"];

            return new Uri(url.Replace("[DATE]", transactionDate.ToString(DateFormat, DateFormatterProvider)));
        }
    }
}
