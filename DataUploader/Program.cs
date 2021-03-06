﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using DataParser;
using DataType;
using FuturesDataCrawler;
using Utility;

namespace DataUploader
{
    class ConsoleLogger : ILogger
    {
        private static string _fileName = "log.txt";
        private static StreamWriter _logWriter = null;
        private static Object _mutexLock = new object();

        public ConsoleLogger()
        {
            if (null == _logWriter)
            {
                lock (_mutexLock)
                {
                    if (null == _logWriter)
                    {
                        _logWriter = new StreamWriter(new FileStream(_fileName, FileMode.Append, FileAccess.Write));
                    }
                }
            }
        }
        public void Log(string message)
        {
            _logWriter.WriteLine(message);
            _logWriter.Flush();
            System.Console.WriteLine(message);
        }

        public void Log(Exception runtimeException)
        {
            _logWriter.WriteLine("XXXXXXXXXXXXXXXXXX=====Exception====XXXXXXXXXXXXXXXXXXXXX");
            _logWriter.WriteLine(runtimeException.Message);
            _logWriter.WriteLine("XXXXXXXXXXXXXXXXXX=====Exception End====XXXXXXXXXXXXXXXXXXXXX");
            _logWriter.Flush();

            System.Console.WriteLine("XXXXXXXXXXXXXXXXXX=====Exception====XXXXXXXXXXXXXXXXXXXXX");
            System.Console.WriteLine(runtimeException.Message);
            System.Console.WriteLine("XXXXXXXXXXXXXXXXXX=====Exception End====XXXXXXXXXXXXXXXXXXXXX");
        }
    }
    class Program
    {
        #region useless
        /*
    private static void ProtectConfiguration()
    {
        // Get the application configuration file.
        System.Configuration.Configuration config =
                ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.None);

        // Define the Rsa provider name.
        string provider = "RsaProtectedConfigurationProvider";

        // Get the section to protect.
        ConfigurationSection connStrings =
            config.ConnectionStrings;

        if (connStrings != null)
        {
            if (!connStrings.SectionInformation.IsProtected)
            {
                if (!connStrings.ElementInformation.IsLocked)
                {
                    // Protect the section.
                    connStrings.SectionInformation.ProtectSection(provider);

                    connStrings.SectionInformation.ForceSave = true;
                    config.Save(ConfigurationSaveMode.Full);

                    Console.WriteLine("Section {0} is now protected by {1}",
                        connStrings.SectionInformation.Name,
                        connStrings.SectionInformation.ProtectionProvider.Name);

                }
                else
                    Console.WriteLine(
                         "Can't protect, section {0} is locked",
                         connStrings.SectionInformation.Name);
            }
            else
                Console.WriteLine(
                    "Section {0} is already protected by {1}",
                    connStrings.SectionInformation.Name,
                    connStrings.SectionInformation.ProtectionProvider.Name);

        }
        else
            Console.WriteLine("Can't get the section {0}",
                connStrings.SectionInformation.Name);

    }

    private static void UnProtectConfiguration()
    {

        // Get the application configuration file.
        System.Configuration.Configuration config =
                ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.None);

        // Get the section to unprotect.
        ConfigurationSection connStrings =
            config.ConnectionStrings;

        if (connStrings != null)
        {
            if (connStrings.SectionInformation.IsProtected)
            {
                if (!connStrings.ElementInformation.IsLocked)
                {
                    // Unprotect the section.
                    connStrings.SectionInformation.UnprotectSection();

                    connStrings.SectionInformation.ForceSave = true;
                    config.Save(ConfigurationSaveMode.Full);

                    Console.WriteLine("Section {0} is now unprotected.",
                        connStrings.SectionInformation.Name);

                }
                else
                    Console.WriteLine(
                         "Can't unprotect, section {0} is locked",
                         connStrings.SectionInformation.Name);
            }
            else
                Console.WriteLine(
                    "Section {0} is already unprotected.",
                    connStrings.SectionInformation.Name);

        }
        else
            Console.WriteLine("Can't get the section {0}",
                connStrings.SectionInformation.Name);

    }
    */
        #endregion

        public static ConsoleLogger Logger { get; }= new ConsoleLogger();
        

        private static void WriteTransactionDataToStore(IEnumerable<ContractTransactionInfo> contracts, FuturesDataStore dataStore)
        {
            foreach (var contract in contracts)
            {
                if (dataStore.ContractTransactionInfoes.Find(contract.ID) == null)
                {
                    dataStore.ContractTransactionInfoes.Add(contract);
                }
                else
                {
                    var webContract = dataStore.ContractTransactionInfoes.Where(c => c.ID.Equals(contract.ID)).First();
                    if (!webContract.Equals(contract))
                    {
                        System.Console.WriteLine("data store (Daily Transaction): " + webContract.ID + "not equal to online");
                    }
                }
            }
            dataStore.SaveChanges();

        }
        private static void HandleDailyTransactionData(ITransactionParser parser, string htmlText, DateTime transDate, FuturesDataStore dataStore)
        {
            var contracts = parser.GetTopContracts(htmlText, 2, transDate);
            WriteTransactionDataToStore(contracts, dataStore);
        }

        private static void HandlePositionData(IDealerPositionParser parser, string htmlText, DateTime transDate, FuturesDataStore dataStore)
        {
            var positions = parser.GetDealerPositionList(htmlText, transDate);
            foreach (var pos in positions)
            {
                if (dataStore.DealerPositionInfoes.Find(pos.Id) == null)
                {
                    dataStore.DealerPositionInfoes.Add(pos);
                }
                else
                {
                    var webPos = dataStore.DealerPositionInfoes.Where(c => c.Id.Equals(pos.Id)).First();
                    if (!webPos.Equals(pos))
                    {
                        System.Console.WriteLine("data store (DealerPosition): " + webPos.Id + "not equal to online");
                        dataStore.DealerPositionInfoes.AddOrUpdate(pos);
                    }
                }
            }

            dataStore.SaveChanges();

        }
        private static void DceDataHandler(string transactionText, DateTime transactionDate, FuturesDataStore dataStore)
        {
            var dceTransactionParser = new DceTransactionParser();
            var tops = dceTransactionParser.GetTopContracts(transactionText, 2, transactionDate);
            WriteTransactionDataToStore(tops, dataStore);

            var dcePositionParser = new DceDealerPositionParser();

            foreach (var contract in tops)
            {
                var dcePositionCrawler = new DceDealerPositionCrawler(contract.Commodity, contract.Contract);
                dcePositionCrawler.RuntimeLogger = Logger;

                dcePositionCrawler.PullData(transactionDate, transactionDate, (htmlText, targetDate) =>
                {
                    HandlePositionData(dcePositionParser, htmlText, targetDate, dataStore);
                });
            }
        }

        static void Main(string[] args)
        {
            var connection = ConfigurationManager.ConnectionStrings["CloudDBConnect"];
            var dataStore = new FuturesDataStore(connection.ConnectionString);

            DateTime startDate = new DateTime(2015, 2, 1);
            DateTime endDate = new DateTime(2015, 2, 28); ;

            var dceTransactionCrawler = new DceDailyTransactionCrawler();
            dceTransactionCrawler.RuntimeLogger = Logger;
            var dceTransactionParser = new DceTransactionParser();
            dceTransactionCrawler.PullData(startDate, endDate, (text, transDate) => DceDataHandler(text, transDate.Date, dataStore));

            var shfeTransactionCrawler = new ShfeDailyTransactionCrawler();
            shfeTransactionCrawler.RuntimeLogger = Logger;
            var shfeTransactionParser = new ShfeTransactionParser();
            shfeTransactionCrawler.PullData(startDate, endDate, (text, transDate) =>
            {
                HandleDailyTransactionData(shfeTransactionParser, text, transDate.Date, dataStore);
            });
            var shfePositionCrawler = new ShfeDealerPositionCrawler();
            shfePositionCrawler.RuntimeLogger = Logger;
            var shfePositionParser = new ShfeDealerPositionParser();
            shfePositionCrawler.PullData(startDate, endDate, (text, transDate) =>
            {
                HandlePositionData(shfePositionParser, text, transDate.Date, dataStore);
            });

            var czceTransactionCrawler = new CzceDailyTransactionCrawler();
            czceTransactionCrawler.RuntimeLogger = Logger;
            var czceTransactionParser = new CzceTransactionParser();
            czceTransactionCrawler.PullData(startDate, endDate, (text, transDate) =>
            {
                HandleDailyTransactionData(czceTransactionParser, text, transDate.Date, dataStore);
            }); 
            var czcePositionCrawler = new CzceDealerPositionCrawler();
            czcePositionCrawler.RuntimeLogger = Logger;
            var czcePositionParser = new CzceDealerPositionParser();
            czcePositionCrawler.PullData(startDate, endDate, (text, transDate) =>
            {
                HandlePositionData(czcePositionParser, text, transDate.Date, dataStore);
            });

            System.Console.WriteLine("\n\n\n\n==================================\nFinished!!!!");
            System.Console.ReadLine();
        }
    }
}
