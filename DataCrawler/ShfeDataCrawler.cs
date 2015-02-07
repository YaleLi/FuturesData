using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesDataCrawler
{
    public abstract class ShfeDataCrawler : DataCrawler
    {
        protected ShfeDataCrawler()
        {
            ContentEncoding = Encoding.UTF8;
        }
    }
}
