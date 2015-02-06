using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCrawler
{
    public class DceDailyTransactionCrawler :DceDataCrawler
    {
        protected override string BuildUrl(DateTime date)
        {
            string url = ConfigurationManager.AppSettings["Dce.Transaction.Url"];

            return url.Replace("[DATE]", date.ToString("yyyyMMdd"));
        }
    }
}
