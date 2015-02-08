using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DataParser;
using DataType;
using FuturesDataCrawler;

namespace DataUploader
{
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

            DateTime startDate = new DateTime(2015, 2, 3);
            DateTime endDate = DateTime.Now;

            var shfeTransactionCrawler = new ShfeDailyTransactionCrawler();
            var shfeTransactionParser = new ShfeTransactionParser();
            shfeTransactionCrawler.PullData(startDate, endDate, (text, transDate) =>
            {
                HandleDailyTransactionData(shfeTransactionParser, text, transDate, dataStore);
            });
            var shfePositionCrawler = new ShfeDealerPositionCrawler();
            var shfePositionParser = new ShfeDealerPositionParser();
            shfePositionCrawler.PullData(startDate, endDate, (text, transDate) =>
            {
                HandlePositionData(shfePositionParser, text, transDate, dataStore);
            });

            var czceTransactionCrawler = new CzceDailyTransactionCrawler();
            var czceTransactionParser = new CzceTransactionParser();
            czceTransactionCrawler.PullData(startDate, endDate, (text, transDate) =>
            {
                HandleDailyTransactionData(czceTransactionParser, text, transDate, dataStore);
            });
            var czcePositionCrawler = new CzceDealerPositionCrawler();
            var czcePositionParser = new CzceDealerPositionParser();
            czcePositionCrawler.PullData(startDate, endDate, (text, transDate) =>
            {
                HandlePositionData(czcePositionParser, text, transDate, dataStore);
            });

            var dceTransactionCrawler = new DceDailyTransactionCrawler();
            var dceTransactionParser = new DceTransactionParser();
            dceTransactionCrawler.PullData(startDate, endDate, (text, transDate) => DceDataHandler(text, transDate, dataStore));

            System.Console.ReadLine();
        }
    }
}
