using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace FuturesDataCrawler
{
    public abstract class CzceDataCrawler :DataCrawler
    {
        protected CzceDataCrawler()
        {
            ContentEncoding = Encoding.GetEncoding("GB2312");
        }

        protected virtual Uri BuildUrl(string origConfig, DateTime transactionDate)
        {
            if (string.IsNullOrWhiteSpace(origConfig))
            {
                throw new WebException("Invalid Empty Url: ");
            }
            var url = origConfig.Replace("[DATE]", transactionDate.ToString(DateFormat, DateFormatterProvider));
            url = url.Replace("[YEAR]", transactionDate.ToString("yyyy", DateFormatterProvider));

            return new Uri(url);
        }
    }
}
