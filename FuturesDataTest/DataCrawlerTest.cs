using DataCrawler;

namespace FuturesDataTest
{
    using System;
    using System.IO;
    using System.Text;
    using FuturesDataCrawler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DataCrawlerTest
    {
        [TestMethod]
        public void DownloadShfeDealerInfoTest()
        {
            string webText = TestUtility.RetrieveWebPage(new DateTime(2014, 1, 6), new ShfeDealerPositionCrawler());
            var localText = TestUtility.LoadLocalFile("..\\..\\Data\\shfe_position_20140106.dat", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));

            webText = TestUtility.RetrieveWebPage(new DateTime(2015, 2, 3), new ShfeDealerPositionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadHolidayShfeDealerInfoTest()
        {
            string webText = TestUtility.RetrieveWebPage(new DateTime(2015, 1, 1), new ShfeDealerPositionCrawler());
            var localText = TestUtility.LoadLocalFile("..\\..\\Data\\shfe_position_20150101.dat", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadShfeTransactionInfoTest()
        {
            string webText = TestUtility.RetrieveWebPage(new DateTime(2014, 1, 6), new ShfeDailyTransactionCrawler());
            var localText = TestUtility.LoadLocalFile("..\\..\\Data\\shfe_transaction_20140106.dat", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));

            webText = TestUtility.RetrieveWebPage(new DateTime(2014, 2, 3), new ShfeDailyTransactionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadHolidayShfeTransactionInfoTest()
        {
            string webText = TestUtility.RetrieveWebPage(new DateTime(2014, 1, 1), new ShfeDailyTransactionCrawler());
            var localText = TestUtility.LoadLocalFile("..\\..\\Data\\shfe_transaction_20140101.dat", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadCzceDealerInfoTest()
        {
            string webText = TestUtility.RetrieveWebPage(new DateTime(2014, 1, 6), new CzceDealerPositionCrawler());
            var localText = TestUtility.LoadLocalFile("..\\..\\Data\\czce_position_20140106.htm", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));

            webText = TestUtility.RetrieveWebPage(new DateTime(2014, 2, 3), new CzceDealerPositionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadHolidayCzceDealerInfoTest()
        {
            string webText = TestUtility.RetrieveWebPage(new DateTime(2015, 1, 1), new CzceDealerPositionCrawler());
            var localText = "";

            Assert.IsTrue(localText.Equals(webText));

            webText = TestUtility.RetrieveWebPage(new DateTime(2015, 2, 3), new CzceDealerPositionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadCzceTransactionInfoTest()
        {
            string webText = TestUtility.RetrieveWebPage(new DateTime(2014, 1, 6), new CzceDailyTransactionCrawler());
            var localText = TestUtility.LoadLocalFile("..\\..\\Data\\czce_transaction_20140106.htm", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));

            webText = TestUtility.RetrieveWebPage(new DateTime(2014, 2, 3), new CzceDailyTransactionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadHolidayCzceTransactionInfoTest()
        {
            string webText = TestUtility.RetrieveWebPage(new DateTime(2015, 1, 1), new CzceDailyTransactionCrawler());
            var localText = "";

            Assert.IsTrue(localText.Equals(webText));

            webText = TestUtility.RetrieveWebPage(new DateTime(2015, 2, 3), new CzceDailyTransactionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }


        [TestMethod]
        public void DownloadDceDealerCommodityInfoTest()
        {
            var webText = TestUtility.RetrieveWebPage(new DateTime(2014, 1, 6), new DceDealerPositionCrawler("a", ""));
            var localText = TestUtility.LoadLocalFile("..\\..\\Data\\dce_a_position_20140106.htm", Encoding.UTF8);

            int length = 7000;
            webText = webText.Substring(0, length);
            localText = localText.Substring(0, length);

            Assert.IsTrue(localText.Equals(webText));

            webText = TestUtility.RetrieveWebPage(new DateTime(2015, 2, 3), new DceDealerPositionCrawler("a", ""));
            length = 5000;
            webText = webText.Substring(0, length);
            localText = localText.Substring(0, length);
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadDceDealerContractInfoTest()
        {
            var webText = TestUtility.RetrieveWebPage(new DateTime(2014, 1, 6), new DceDealerPositionCrawler("a", "1409"));
            var localText = TestUtility.LoadLocalFile("..\\..\\Data\\dce_a1409_position_20140106.htm", Encoding.UTF8);

            int length = 7000;
            webText = webText.Substring(0, length);
            localText = localText.Substring(0, length);

            Assert.IsTrue(localText.Equals(webText));

            webText = TestUtility.RetrieveWebPage(new DateTime(2014, 6, 3), new DceDealerPositionCrawler("a", "1409"));
            length = 5000;
            webText = webText.Substring(0, length);
            localText = localText.Substring(0, length);
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadHolidayDceDealerCommodityInfoTest()
        {
            var webText = TestUtility.RetrieveWebPage(new DateTime(2015, 1, 1), new DceDealerPositionCrawler("a", ""));
            var localText = TestUtility.LoadLocalFile("..\\..\\Data\\dce_a_position_20150101.htm", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));

            webText = TestUtility.RetrieveWebPage(new DateTime(2015, 2, 3), new DceDealerPositionCrawler("a", ""));
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadDceTransactionInfoTest()
        {
            string webText = TestUtility.RetrieveWebPage(new DateTime(2014, 1, 6), new DceDailyTransactionCrawler());
            var localText = TestUtility.LoadLocalFile("..\\..\\Data\\dce_transaction_20140106.htm", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));

            webText = TestUtility.RetrieveWebPage(new DateTime(2015, 2, 3), new DceDailyTransactionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadHolidayDceTransactionInfoTest()
        {
            string webText = TestUtility.RetrieveWebPage(new DateTime(2015, 1, 1), new DceDailyTransactionCrawler());
            var localText = TestUtility.LoadLocalFile("..\\..\\Data\\dce_a_position_20150101.htm", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));

            webText = TestUtility.RetrieveWebPage(new DateTime(2015, 2, 3), new DceDailyTransactionCrawler());
            Assert.IsFalse(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadDceCommodityCodeTest()
        {
            string webText = TestUtility.RetrieveWebPage(new DateTime(2015, 2, 3), new DceCommodityCodeCrawler());
            //var localText = TestUtility.LoadLocalFile("..\\..\\Data\\dce_commodity_code.htm", Encoding.UTF8);

            Assert.IsTrue(webText.IndexOf("select name=\"Pu00021_Input.variety\"")>0);
        }
    }
}
