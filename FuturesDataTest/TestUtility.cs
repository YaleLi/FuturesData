using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesDataTest
{
    using FuturesDataCrawler;
    class TestUtility
    {
        public static string LoadLocalFile(string fileName, Encoding encoding)
        {
            string text = "";
            using (var reader = new StreamReader(new FileStream(fileName, FileMode.Open), encoding))
            {
                text = reader.ReadToEnd();
            }
            ;
            return text;
        }

        public static string RetrieveWebPage(DateTime date, DataCrawler crawler)
        {
            string webText = "";

            crawler.PullData(date, date, (content, transDate) => webText = content);
            return webText;
        }

    }
}
