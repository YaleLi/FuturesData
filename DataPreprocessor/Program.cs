using System;
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
            var processor = new ContractTransactionProcessor();

            processor.ProcessData(DateTime.Now.AddDays(-7), DateTime.Now);
        }
    }
}
