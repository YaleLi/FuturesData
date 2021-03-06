﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPreprocessor
{
    class Program
    {
        static void Main(string[] args)
        {
            //var processor = new ContractTransactionProcessor();
            var processor = new DealerPositionProcessor();

            DateTime startDate = new DateTime(2014, 1, 1);
            DateTime endDate = new DateTime(2015, 2, 28);
            processor.ProcessData(startDate, endDate);


            System.Console.WriteLine("Finished.....");
            System.Console.ReadLine();
        }
    }
}
