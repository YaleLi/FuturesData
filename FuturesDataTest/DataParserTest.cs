using System;
using System.Collections.Generic;
using System.Linq;
using FuturesDataCrawler;
using DataParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataType;

namespace FuturesDataTest
{
    [TestClass]
    public class DataParserTest
    {
        private void ValidateValues(List<ContractTransactionInfo> contracts, List<ContractTransactionInfo> webData)
        {
            foreach (var contract in contracts)
            {
                var target =
                    webData.Where(
                        web => web.Commodity.Equals(contract.Commodity) && web.Contract.Equals(contract.Contract));
                int count = target.Count();

                if (null == target || target.Count() != 1 || !contract.Equals(target.First()))
                {
                    Assert.Fail();
                    break;
                }

            }
        }

        private void ValidateValues(List<SingleDealerPosition> predifined, List<DealerPositionInfo> webData)
        {
            foreach (var testData in predifined)
            {
                var dealerInfo =
                    webData.Where(info => info.Commodity.Equals(testData.Commodity) && info.Month.Equals(testData.Month));

                if (null == dealerInfo || dealerInfo.Count() != 1 || !testData.ExistIn(dealerInfo.First()))
                {
                    Assert.Fail();
                    break;
                }
            }
        }

        [TestMethod]
        public void ShfeTransactionContractListTest()
        {
            DateTime date = new DateTime(2014, 1, 6);
            string webText = TestUtility.RetrieveWebPage(date, new ShfeDailyTransactionCrawler());
            var parser = new ShfeTransactionParser();
            var listFromWeb = parser.GetContractList(webText, date);

            var itemList = new List<ContractTransactionInfo>();
            itemList.Add(new ContractTransactionInfo(date, "shfe", "cu", "1403", 51900, 52080, 51670, 51770, 51890,
                243308, 220868));
            itemList.Add(new ContractTransactionInfo(date, "shfe", "al", "1412", 13990, 13990, 13960, 13960, 13980, 6,
                14));
            itemList.Add(new ContractTransactionInfo(date, "shfe", "pb", "1409", -1, -1, -1, 14550, 14550, 0, 0));
            itemList.Add(new ContractTransactionInfo(date, "shfe", "fu", "1404", -1, -1, -1, 4150, 4150, 0, 12));
            itemList.Add(new ContractTransactionInfo(date, "shfe", "ru", "1411", 17510, 17640, 17290, 17290, 17465, 130,
                1060));
            itemList.Add(new ContractTransactionInfo(date, "shfe", "au", "1406", 245.5, 248.35, 244.65, 246.90, 246.35,
                240660, 170620));

            ValidateValues(itemList, listFromWeb);
        }

        [TestMethod]
        public void ShfeHolidayTransactionContractListTest()
        {
            DateTime date = new DateTime(2015, 1, 1);
            string webText = TestUtility.RetrieveWebPage(date, new ShfeDailyTransactionCrawler());
            var parser = new ShfeTransactionParser();
            var listFromWeb = parser.GetContractList(webText, date);

            Assert.IsTrue(listFromWeb.Count == 0);
        }


        [TestMethod]
        public void ShfeDealerPositionParserTest()
        {
            DateTime date = new DateTime(2014, 1, 6);
            string webText = TestUtility.RetrieveWebPage(date, new ShfeDealerPositionCrawler());
            var parser = new ShfeDealerPositionParser();
            var listFromWeb = parser.GetDealerPositionList(webText, date);

            var testData = new List<SingleDealerPosition>();
            testData.Add(new SingleDealerPosition(date, "cu", "1403", InformationType.VolumeInfo, 1, "中证期货", 14654));
            testData.Add(new SingleDealerPosition(date, "cu", "1403", InformationType.SellInfo, 20, "光大期货", 1337));
            testData.Add(new SingleDealerPosition(date, "zn", "1401", InformationType.SellInfo, 13, "经易期货", 10));
            testData.Add(new SingleDealerPosition(date, "au", "1401", InformationType.BuyInfo, 10, "中原期货", 3));
            testData.Add(new SingleDealerPosition(date, "ru", "1409", InformationType.SellInfo, 20, "财富期货", 500));

            ValidateValues(testData, listFromWeb);
        }

        [TestMethod]
        public void ShfeHolidayDealerPositionParserTest()
        {
            DateTime date = new DateTime(2015, 1, 1);
            string webText = TestUtility.RetrieveWebPage(date, new ShfeDealerPositionCrawler());
            var parser = new ShfeDealerPositionParser();
            var listFromWeb = parser.GetDealerPositionList(webText, date);

            Assert.IsTrue(listFromWeb.Count == 0);
        }
    }
}

