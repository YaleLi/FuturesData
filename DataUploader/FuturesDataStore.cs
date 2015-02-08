using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;

namespace DataUploader
{
    class FuturesDataStore : DbContext
    {
        public virtual DbSet<ContractTransactionInfo> ContractTransactionInfoes { get; set; }
        public virtual DbSet<DealerPositionInfo> DealerPositionInfoes { get; set; }

        public FuturesDataStore(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractTransactionInfo>().HasKey(e => e.ID);
            modelBuilder.Entity<DealerPositionInfo>().HasKey(e => e.Id);
        }

    }
}
