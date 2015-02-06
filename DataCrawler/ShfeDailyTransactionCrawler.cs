using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCrawler
{
    public class ShfeDailyTransactionCrawler : ShfeDataCrawler
    {
        protected override string BuildUrl(DateTime date)
        {
            string url = ConfigurationManager.AppSettings["Shfe.Transaction.Url"];

            return url.Replace("[DATE]", date.ToString("yyyyMMdd"));
        }
    }
}
