using System;
using System.Collections.Generic;
using FuturesDataCrawler;
using DataParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelFirstTest;

namespace FuturesDataTest
{
    [TestClass]
    public class DataParserTest
    {
        
        [TestMethod]
        public void ShfeTransactionContractListTest()
        {
            DateTime date = new DateTime(2014, 1, 6);
            string webText = TestUtility.RetrieveWebPage(date, new ShfeDailyTransactionCrawler());
            var parser = new ShfeTransactionParser();
            var list = parser.GetContractList(webText);

            var itemList = new List<ContractTransactionInfo>();
            itemList.Add(new ContractTransactionInfo(date, "shfe", "cu", "1403", 51900, 52080, 51670, 51770, 51890, 243308, 220868));
            itemList.Add(new ContractTransactionInfo(date, "shfe", "al", "1412", 13990, 13990, 13960, 13960, 13980, 6, 14));
            itemList.Add(new ContractTransactionInfo(date, "shfe", "pb", "1409", -1, -1, -1, 14440, 14440, 0, 0));
            itemList.Add(new ContractTransactionInfo(date, "shfe", "pb", "1409", -1, -1, -1, 14440, 14440, 0, 0));
            itemList.Add(new ContractTransactionInfo(date, "shfe", "ru", "1411", 17510, 17640, 17290, 17290, 17465, 130, 1060));
            itemList.Add(new ContractTransactionInfo(date, "shfe", "au", "1406", 245.5, 248.35, 244.65, 246.90, 246.35, 240660, 170620));
        }
    }
}
