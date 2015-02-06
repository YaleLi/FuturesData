using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCrawler
{
    public abstract class ShfeDataCrawler : DataCrawler
    {
        public ShfeDataCrawler()
        {
            ContentEncoding = Encoding.UTF8;
        }
    }
}
