using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCrawler
{
    public abstract class CzceDataCrawler :DataCrawler
    {
        public CzceDataCrawler()
        {
            ContentEncoding = Encoding.GetEncoding("GB2312");
        }

        protected virtual string BuildUrl(string origConfig, DateTime date)
        {
            var url = origConfig.Replace("[DATE]", date.ToString("yyyyMMdd"));
            url = url.Replace("[YEAR]", date.ToString("yyyy"));

            return url;
        }
    }
}
