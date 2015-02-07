using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;

namespace DataParser
{
    interface ITransactionParser
    {
        List<ContractTransactionInfo> GetContractList(string htmlText, DateTime transactionDate);
        List<ContractTransactionInfo> GetTopContracts(string htmlText, int count, DateTime transactionDate);
    }
}
