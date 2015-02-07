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
                var target = webData.Where(web => web.Commodity.Equals(contract.Commodity) && web.Contract.Equals(contract.Contract));
                int count = target.Count();

                if (null == target || target.Count()!=1 || !contract.Equals(target.First()))
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
            itemList.Add(new ContractTransactionInfo(date, "shfe", "cu", "1403", 51900, 52080, 51670, 51770, 51890, 243308, 220868));
            itemList.Add(new ContractTransactionInfo(date, "shfe", "al", "1412", 13990, 13990, 13960, 13960, 13980, 6, 14));
            itemList.Add(new ContractTransactionInfo(date, "shfe", "pb", "1409", -1, -1, -1, 14550, 14550, 0, 0));
            itemList.Add(new ContractTransactionInfo(date, "shfe", "fu", "1404", -1, -1, -1, 4150, 4150, 0, 12));
            itemList.Add(new ContractTransactionInfo(date, "shfe", "ru", "1411", 17510, 17640, 17290, 17290, 17465, 130, 1060));
            itemList.Add(new ContractTransactionInfo(date, "shfe", "au", "1406", 245.5, 248.35, 244.65, 246.90, 246.35, 240660, 170620));

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
    }
}
