namespace FuturesDataTest
{
    using System;
    using System.IO;
    using System.Text;
    using DataCrawler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DataCrawlerTest
    {
        private string LoadLocalFile(string fileName, Encoding encoding)
        {
            string text = "";
            using (var reader = new StreamReader(new FileStream(fileName, FileMode.Open), encoding))
            {
                text = reader.ReadToEnd();
            }
            ;
            return text;
        }

        private string RetrieveWebPage(DateTime date, DataCrawler crawler)
        {
            string webText = "";

            crawler.PullData(date, date, (content, transDate) => webText = content);
            return webText;
        }

        [TestMethod]
        public void DownloadShfeDealerInfoTest()
        {
            string webText = RetrieveWebPage(new DateTime(2014, 1, 6), new ShfeDealerPositionCrawler());
            var localText = LoadLocalFile("..\\..\\Data\\shfe_position_20140106.dat", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));

            webText = RetrieveWebPage(new DateTime(2015, 2, 3), new ShfeDealerPositionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadHolidayShfeDealerInfoTest()
        {
            string webText = RetrieveWebPage(new DateTime(2015, 1, 1), new ShfeDealerPositionCrawler());
            var localText = LoadLocalFile("..\\..\\Data\\shfe_position_20150101.dat", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadShfeTransactionInfoTest()
        {
            string webText = RetrieveWebPage(new DateTime(2014, 1, 6), new ShfeDailyTransactionCrawler());
            var localText = LoadLocalFile("..\\..\\Data\\shfe_transaction_20140106.dat", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));

            webText = RetrieveWebPage(new DateTime(2014, 2, 3), new ShfeDailyTransactionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadHolidayShfeTransactionInfoTest()
        {
            string webText = RetrieveWebPage(new DateTime(2014, 1, 1), new ShfeDailyTransactionCrawler());
            var localText = LoadLocalFile("..\\..\\Data\\shfe_transaction_20140101.dat", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadCzceDealerInfoTest()
        {
            string webText = RetrieveWebPage(new DateTime(2014, 1, 6), new CzceDealerPositionCrawler());
            var localText = LoadLocalFile("..\\..\\Data\\czce_position_20140106.htm", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));

            webText = RetrieveWebPage(new DateTime(2014, 2, 3), new CzceDealerPositionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadHolidayCzceDealerInfoTest()
        {
            string webText = RetrieveWebPage(new DateTime(2015, 1, 1), new CzceDealerPositionCrawler());
            var localText = "";

            Assert.IsTrue(localText.Equals(webText));

            webText = RetrieveWebPage(new DateTime(2015, 2, 3), new CzceDealerPositionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadCzceTransactionInfoTest()
        {
            string webText = RetrieveWebPage(new DateTime(2014, 1, 6), new CzceDailyTransactionCrawler());
            var localText = LoadLocalFile("..\\..\\Data\\czce_transaction_20140106.htm", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));

            webText = RetrieveWebPage(new DateTime(2014, 2, 3), new CzceDailyTransactionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadHolidayCzceTransactionInfoTest()
        {
            string webText = RetrieveWebPage(new DateTime(2015, 1, 1), new CzceDailyTransactionCrawler());
            var localText = "";

            Assert.IsTrue(localText.Equals(webText));

            webText = RetrieveWebPage(new DateTime(2015, 2, 3), new CzceDailyTransactionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }


        [TestMethod]
        public void DownloadDceDealerCommodityInfoTest()
        {
            var webText = RetrieveWebPage(new DateTime(2014, 1, 6), new DceDealerPositionCrawler("a", ""));
            var localText = LoadLocalFile("..\\..\\Data\\dce_a_position_20140106.htm", Encoding.UTF8);

            int length = 7000;
            webText = webText.Substring(0, length);
            localText = localText.Substring(0, length);

            Assert.IsTrue(localText.Equals(webText));

            webText = RetrieveWebPage(new DateTime(2015, 2, 3), new DceDealerPositionCrawler("a", ""));
            length = 5000;
            webText = webText.Substring(0, length);
            localText = localText.Substring(0, length);
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadDceDealerContractInfoTest()
        {
            var webText = RetrieveWebPage(new DateTime(2014, 1, 6), new DceDealerPositionCrawler("a", "1409"));
            var localText = LoadLocalFile("..\\..\\Data\\dce_a1409_position_20140106.htm", Encoding.UTF8);

            int length = 7000;
            webText = webText.Substring(0, length);
            localText = localText.Substring(0, length);

            Assert.IsTrue(localText.Equals(webText));

            webText = RetrieveWebPage(new DateTime(2014, 6, 3), new DceDealerPositionCrawler("a", "1409"));
            length = 5000;
            webText = webText.Substring(0, length);
            localText = localText.Substring(0, length);
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadHolidayDceDealerCommodityInfoTest()
        {
            var webText = RetrieveWebPage(new DateTime(2015, 1, 1), new DceDealerPositionCrawler("a", ""));
            var localText = LoadLocalFile("..\\..\\Data\\dce_a_position_20150101.htm", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));

            webText = RetrieveWebPage(new DateTime(2015, 2, 3), new DceDealerPositionCrawler("a", ""));
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadDceTransactionInfoTest()
        {
            string webText = RetrieveWebPage(new DateTime(2014, 1, 6), new DceDailyTransactionCrawler());
            var localText = LoadLocalFile("..\\..\\Data\\dce_transaction_20140106.htm", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));

            webText = RetrieveWebPage(new DateTime(2015, 2, 3), new DceDailyTransactionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadHolidayDceTransactionInfoTest()
        {
            string webText = RetrieveWebPage(new DateTime(2015, 1, 1), new DceDailyTransactionCrawler());
            var localText = LoadLocalFile("..\\..\\Data\\dce_a_position_20150101.htm", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));

            webText = RetrieveWebPage(new DateTime(2015, 2, 3), new DceDailyTransactionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }
    }
}
