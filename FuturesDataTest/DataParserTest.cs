﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using FuturesDataCrawler;
using DataParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataType;

namespace FuturesDataTest
{
    [TestClass]
    public class DataParserTest
    {
        private void ValidateValues(IEnumerable<ContractTransactionInfo> contracts, IEnumerable<ContractTransactionInfo> webData)
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

        private void ValidateValues(IEnumerable<SingleDealerPosition> predifined, IEnumerable<DealerPositionInfo> webData)
        {
            foreach (var testData in predifined)
            {
                var dealerInfo =
                    webData.Where(info => info.Commodity.Equals(testData.Commodity) && info.Month.Equals(testData.Month));

                int count = dealerInfo.Count();
                if (null == dealerInfo || count != 1 || !testData.ExistIn(dealerInfo.First()))
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

            var testData = new List<ContractTransactionInfo>();
            testData.Add(new ContractTransactionInfo(date, "shfe", "cu", "1403", 51900, 52080, 51670, 51770, 51890,
                243308, 220868));
            testData.Add(new ContractTransactionInfo(date, "shfe", "al", "1412", 13990, 13990, 13960, 13960, 13980, 6,
                14));
            testData.Add(new ContractTransactionInfo(date, "shfe", "pb", "1409", -1, -1, -1, 14550, 14550, 0, 0));
            testData.Add(new ContractTransactionInfo(date, "shfe", "fu", "1404", -1, -1, -1, 4150, 4150, 0, 12));
            testData.Add(new ContractTransactionInfo(date, "shfe", "ru", "1411", 17510, 17640, 17290, 17290, 17465, 130,
                1060));
            testData.Add(new ContractTransactionInfo(date, "shfe", "au", "1406", 245.5, 248.35, 244.65, 246.90, 246.35,
                240660, 170620));

            ValidateValues(testData, listFromWeb);
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

        [TestMethod]
        public void DceCommodityCodeParseTest()
        {
            string code1 = DceCommodityCodeHelper.GetCommodityCode("豆一");
            string code2 = DceCommodityCodeHelper.GetCommodityCode("大豆");

            Assert.IsTrue(code1.Equals("a") && code2.Equals("s"));
        }
        [TestMethod]
        public void DceTransactionContractListTest()
        {
            DateTime date = new DateTime(2014, 1, 6);
            string webText = TestUtility.RetrieveWebPage(date, new DceDailyTransactionCrawler());
            var parser = new DceTransactionParser();
            var listFromWeb = parser.GetContractList(webText, date);

            var testData = new List<ContractTransactionInfo>();

            testData.Add(new ContractTransactionInfo(date, "dce", "a", "1401", -1, -1, -1, 4425, 4425, 0, 22));
            testData.Add(new ContractTransactionInfo(date, "dce", "fb", "1405", 69.45, 69.55, 66.65, 66.75, 67.25, 348528, 88992));
            testData.Add(new ContractTransactionInfo(date, "dce", "y", "1412", -1, -1, -1, 6992, 6992, 0, 14));
            testData.Add(new ContractTransactionInfo(date, "dce", "bb", "1409", 123.4, 123.9, 119.45, 121.5, 121.45, 46, 194));
            testData.Add(new ContractTransactionInfo(date, "dce", "fb", "1412", -1, -1, -1, 68.2, 68.2, 0, 2));
            testData.Add(new ContractTransactionInfo(date, "dce", "i", "1409", 878, 882, 871, 873, 874, 9560, 37276));
            testData.Add(new ContractTransactionInfo(date, "dce", "l", "1409", 10750, 10800, 10725, 10755, 10760, 10508, 28282));

            ValidateValues(testData, listFromWeb);
        }

        [TestMethod]
        public void DceHolidayTransactionContractListTest()
        {
            DateTime date = new DateTime(2015, 1, 1);
            string webText = TestUtility.RetrieveWebPage(date, new DceDailyTransactionCrawler());
            var parser = new DceTransactionParser();
            var listFromWeb = parser.GetContractList(webText, date);

            Assert.IsTrue(listFromWeb.Count == 0);
        }


        [TestMethod]
        public void DceCommodityDealerPositionParserTest()
        {
            DateTime date = new DateTime(2014, 1, 6);
            string webText = TestUtility.RetrieveWebPage(date, new DceDealerPositionCrawler("a", ""));
            var parser = new DceDealerPositionParser();
            var listFromWeb = parser.GetDealerPositionList(webText, date);

            var testData = new List<SingleDealerPosition>();
            testData.Add(new SingleDealerPosition(date, "a", "", InformationType.VolumeInfo, 1, "国投期货", 5167));
            testData.Add(new SingleDealerPosition(date, "a", "", InformationType.SellInfo, 145, "京都期货", 0));
            testData.Add(new SingleDealerPosition(date, "a", "", InformationType.BuyInfo, 141, "深圳金汇", 0));
            testData.Add(new SingleDealerPosition(date, "a", "", InformationType.VolumeInfo, 145, "平安期货", 2));
            testData.Add(new SingleDealerPosition(date, "a", "", InformationType.BuyInfo, 25, "南华期货", 1024));

            ValidateValues(testData, listFromWeb);
        }

        [TestMethod]
        public void DceContractDealerPositionParserTest()
        {
            DateTime date = new DateTime(2014, 1, 6);
            string webText = TestUtility.RetrieveWebPage(date, new DceDealerPositionCrawler("a", "1409"));
            var parser = new DceDealerPositionParser();
            var listFromWeb = parser.GetDealerPositionList(webText, date);

            var testData = new List<SingleDealerPosition>();
            testData.Add(new SingleDealerPosition(date, "a", "1409", InformationType.VolumeInfo, 1, "国投期货", 1649));
            testData.Add(new SingleDealerPosition(date, "a", "1409", InformationType.SellInfo, 121, "中信新际", 0));
            testData.Add(new SingleDealerPosition(date, "a", "1409", InformationType.BuyInfo, 120, "先融期货", 0));
            testData.Add(new SingleDealerPosition(date, "a", "1409", InformationType.VolumeInfo, 123, "天富期货", 1));
            testData.Add(new SingleDealerPosition(date, "a", "1409", InformationType.BuyInfo, 10, "新湖期货", 1075));

            ValidateValues(testData, listFromWeb);
        }

        [TestMethod]
        public void DceHolidayContractDealerPositionParserTest()
        {
            DateTime date = new DateTime(2015, 1, 1);
            string webText = TestUtility.RetrieveWebPage(date, new DceDealerPositionCrawler("a", "1409"));
            var parser = new DceDealerPositionParser();
            var listFromWeb = parser.GetDealerPositionList(webText, date);

            Assert.IsTrue(listFromWeb.Count == 0);
        }

        [TestMethod]
        public void CzceTransactionContractListTest()
        {
            DateTime date = new DateTime(2014, 1, 6);
            string webText = TestUtility.RetrieveWebPage(date, new CzceDailyTransactionCrawler());
            var parser = new CzceTransactionParser();
            var listFromWeb = parser.GetContractList(webText, date);

            var testData = new List<ContractTransactionInfo>();
            testData.Add(new ContractTransactionInfo(date, "czce", "cf", "401", 19080, 19080, 19080, 19080, 19080, 582,896));
            testData.Add(new ContractTransactionInfo(date, "czce", "wH", "411", 0, 0, 0, 0, 2742, 0, 6));
            testData.Add(new ContractTransactionInfo(date, "czce", "TA", "405", 7210, 7212, 7136, 7146, 7174, 238492, 422160));
            testData.Add(new ContractTransactionInfo(date, "czce", "pm", "409", 2551, 2551, 2551, 2551, 2551, 2, 42));

            ValidateValues(testData, listFromWeb);

        }

        [TestMethod]
        public void CzceHolidayTransactionContractListTest()
        {
            DateTime date = new DateTime(2015, 1, 1);
            string webText = TestUtility.RetrieveWebPage(date, new CzceDailyTransactionCrawler());
            var parser = new CzceTransactionParser();
            var listFromWeb = parser.GetContractList(webText, date);

            Assert.IsTrue(listFromWeb.Count == 0);
        }

        [TestMethod]
        public void CzceContractDealerPositionParserTest()
        {
            DateTime date = new DateTime(2014, 1, 6);
            string webText = TestUtility.RetrieveWebPage(date, new CzceDealerPositionCrawler());
            var parser = new CzceDealerPositionParser();
            var listFromWeb = parser.GetDealerPositionList(webText, date);

            var testData = new List<SingleDealerPosition>();
            testData.Add(new SingleDealerPosition(date, "CF", "", InformationType.VolumeInfo, 1, "光大期货", 5071));
            testData.Add(new SingleDealerPosition(date, "RS", "", InformationType.BuyInfo, 8, "徽商期货", 1));
            testData.Add(new SingleDealerPosition(date, "CF", "401", InformationType.VolumeInfo, 5, "上海金源", 1));
            testData.Add(new SingleDealerPosition(date, "CF", "401", InformationType.BuyInfo, 9, "北京中期", 3));
            testData.Add(new SingleDealerPosition(date, "FG", "401", InformationType.SellInfo, 4, "中证期货", 1));
            testData.Add(new SingleDealerPosition(date, "TA", "401", InformationType.SellInfo, 20, "浙江中大", 1430));
            testData.Add(new SingleDealerPosition(date, "TC", "401", InformationType.SellInfo, 3, "光大期货", 1));
            testData.Add(new SingleDealerPosition(date, "SR", "405", InformationType.BuyInfo, 3, "中粮期货", 16869));
            testData.Add(new SingleDealerPosition(date, "wh", "405", InformationType.SellInfo, 20, "美尔雅", 1066));

            ValidateValues(testData, listFromWeb);
        }

        [TestMethod]
        public void CzceContractDealerPositionParser_20120713Test()
        {
            DateTime date = new DateTime(2012, 7, 13);
            string webText = TestUtility.RetrieveWebPage(date, new CzceDealerPositionCrawler());
            var parser = new CzceDealerPositionParser();
            var listFromWeb = parser.GetDealerPositionList(webText, date);

            var testData = new List<SingleDealerPosition>();
            testData.Add(new SingleDealerPosition(date, "CF", "", InformationType.VolumeInfo, 1, "光大期货", 5071));
            testData.Add(new SingleDealerPosition(date, "RS", "", InformationType.BuyInfo, 8, "徽商期货", 1));
            testData.Add(new SingleDealerPosition(date, "CF", "401", InformationType.VolumeInfo, 5, "上海金源", 1));
            testData.Add(new SingleDealerPosition(date, "CF", "401", InformationType.BuyInfo, 9, "北京中期", 3));
            testData.Add(new SingleDealerPosition(date, "FG", "401", InformationType.SellInfo, 4, "中证期货", 1));
            testData.Add(new SingleDealerPosition(date, "TA", "401", InformationType.SellInfo, 20, "浙江中大", 1430));
            testData.Add(new SingleDealerPosition(date, "TC", "401", InformationType.SellInfo, 3, "光大期货", 1));
            testData.Add(new SingleDealerPosition(date, "SR", "405", InformationType.BuyInfo, 3, "中粮期货", 16869));
            testData.Add(new SingleDealerPosition(date, "wh", "405", InformationType.SellInfo, 20, "美尔雅", 1066));

            ValidateValues(testData, listFromWeb);
        }

        [TestMethod]
        public void CzceContractDealerPositionParser_20100304Test()
        {
            DateTime date = new DateTime(2010, 3, 4);
            string webText = TestUtility.RetrieveWebPage(date, new CzceDealerPositionCrawler());
            var parser = new CzceDealerPositionParser();
            var listFromWeb = parser.GetDealerPositionList(webText, date);

            var testData = new List<SingleDealerPosition>();
            testData.Add(new SingleDealerPosition(date, "CF", "", InformationType.VolumeInfo, 1, "光大期货", 5071));
            testData.Add(new SingleDealerPosition(date, "RS", "", InformationType.BuyInfo, 8, "徽商期货", 1));
            testData.Add(new SingleDealerPosition(date, "CF", "401", InformationType.VolumeInfo, 5, "上海金源", 1));
            testData.Add(new SingleDealerPosition(date, "CF", "401", InformationType.BuyInfo, 9, "北京中期", 3));
            testData.Add(new SingleDealerPosition(date, "FG", "401", InformationType.SellInfo, 4, "中证期货", 1));
            testData.Add(new SingleDealerPosition(date, "TA", "401", InformationType.SellInfo, 20, "浙江中大", 1430));
            testData.Add(new SingleDealerPosition(date, "TC", "401", InformationType.SellInfo, 3, "光大期货", 1));
            testData.Add(new SingleDealerPosition(date, "SR", "405", InformationType.BuyInfo, 3, "中粮期货", 16869));
            testData.Add(new SingleDealerPosition(date, "wh", "405", InformationType.SellInfo, 20, "美尔雅", 1066));

            ValidateValues(testData, listFromWeb);
        }

        [TestMethod]
        public void CzceHolidayContractDealerPositionParserTest()
        {
            DateTime date = new DateTime(2015, 1, 1);
            string webText = TestUtility.RetrieveWebPage(date, new CzceDealerPositionCrawler());
            var parser = new CzceDealerPositionParser();
            var listFromWeb = parser.GetDealerPositionList(webText, date);

            Assert.IsTrue(listFromWeb.Count == 0);
        }


        [TestMethod]
        public void ShfeTopTransactionContractTest()
        {
            DateTime date = new DateTime(2014, 1, 6);
            string webText = TestUtility.RetrieveWebPage(date, new ShfeDailyTransactionCrawler());
            var parser = new ShfeTransactionParser();

            var listFromWeb = parser.GetTopContracts(webText, 2, date);

            string[] topContracts = new string[]
            {
                "cu1403", "cu1404", "al1403", "al1404", "zn1403", "zn1404",
                "pb1401", "pb1402", "au1412", "au1406", "ag1401", "ag1406",
                "rb1405", "rb1410", "ru1405", "ru1409"
            };

            var listOfCommodity = listFromWeb.GroupBy(c => c.Commodity);
            foreach (var contracts in listOfCommodity)
            {
                if (contracts.Count() > 2)
                {
                    Assert.Fail();
                }
            }

            var webTopContracts = listFromWeb.Select(c => c.Commodity + c.Contract);
            var joinResult = webTopContracts.Join(topContracts, s => s, s => s, (s1, s2) => s1);
            int jc = joinResult.Count();
            Assert.IsTrue(joinResult.Count() == topContracts.Length);
        }

        [TestMethod]
        public void DceTopTransactionContractTest()
        {
            DateTime date = new DateTime(2014, 1, 6);
            string webText = TestUtility.RetrieveWebPage(date, new DceDailyTransactionCrawler());
            var parser = new DceTransactionParser();

            var listFromWeb = parser.GetTopContracts(webText, 2, date);

            string[] topContracts = new string[]
            {
                "a1405", "a1409", "c1405", "c1409", "bb1405", "fb1405",
                "fb1409", "jd1405", "jd1409", "m1405", "m1409", "y1405", "y1409"
            };

            var listOfCommodity = listFromWeb.GroupBy(c => c.Commodity);
            foreach (var contracts in listOfCommodity)
            {
                if (contracts.Count() > 2)
                {
                    Assert.Fail();
                }
            }

            var webTopContracts = listFromWeb.Select(c => c.Commodity + c.Contract);
            var joinResult = webTopContracts.Join(topContracts, s => s, s => s, (s1, s2) => s1);
            int jc = joinResult.Count();
            Assert.IsTrue(joinResult.Count() == topContracts.Length);
        }

        [TestMethod]
        public void CzceTopTransactionContractTest()
        {
            DateTime date = new DateTime(2014, 1, 6);
            string webText = TestUtility.RetrieveWebPage(date, new CzceDailyTransactionCrawler());
            var parser = new CzceTransactionParser();

            var listFromWeb = parser.GetTopContracts(webText, 2, date);


            string[] topContracts = new string[]
            {
                "cf405", "cf411", "fg405", "fg409", "me405", "me409",
                "pm405", "pm401", "rm405", "rm409", "ta405", "ta409",
                "wh405", "wh401"
            };

            var listOfCommodity = listFromWeb.GroupBy(c => c.Commodity);
            foreach (var contracts in listOfCommodity)
            {
                if (contracts.Count() > 2)
                {
                    Assert.Fail();
                }
            }

            var webTopContracts = listFromWeb.Select(c => c.Commodity + c.Contract);
            var joinResult = webTopContracts.Join(topContracts, s => s, s => s, (s1, s2) => s1);
            int jc = joinResult.Count();
            Assert.IsTrue(joinResult.Count() == topContracts.Length);
        }
    }
}
