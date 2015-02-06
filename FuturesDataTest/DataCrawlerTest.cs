﻿namespace FuturesDataTest
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
            var reader = new StreamReader(new FileStream(fileName, FileMode.Open), encoding);
            return reader.ReadToEnd();
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
        }

        [TestMethod]
        public void DownloadHolidayCzceDealerInfoTest()
        {
            string webText = RetrieveWebPage(new DateTime(2015, 1, 1), new CzceDealerPositionCrawler());
            var localText = "";

            Assert.IsTrue(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadCzceTransactionInfoTest()
        {
            string webText = RetrieveWebPage(new DateTime(2014, 1, 6), new CzceDailyTransactionCrawler());
            var localText = LoadLocalFile("..\\..\\Data\\czce_transaction_20140106.htm", Encoding.UTF8);

            Assert.IsTrue(localText.Equals(webText));
        }

        [TestMethod]
        public void DownloadHolidayCzceTransactionInfoTest()
        {
            string webText = RetrieveWebPage(new DateTime(2015, 1, 1), new CzceDailyTransactionCrawler());
            var localText = "";

            Assert.IsTrue(localText.Equals(webText));
        }


        [TestMethod]
        public void DownloadDceDealerCommodityInfoTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void DownloadDceDealerContractInfoTest()
        {
            throw new NotImplementedException();
        }
    }
}