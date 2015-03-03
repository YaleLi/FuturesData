using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;

namespace DataType
{
    public class FuturesDataStore : DbContext
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infoes")]
        public virtual DbSet<ContractTransactionInfo> ContractTransactionInfoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infoes")]
        public virtual DbSet<DealerPositionInfo> DealerPositionInfoes { get; set; }

        public virtual DbSet<ContractTransactionFeature> ContractTransactionFeatures { get; set; }
        public virtual DbSet<DealerPositionFeature> DealerPositionFeatures { get; set; }

        public FuturesDataStore(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (null == modelBuilder)
            {
                return;
            }
            modelBuilder.Entity<ContractTransactionInfo>().HasKey(e => e.ID);
            modelBuilder.Entity<DealerPositionInfo>().HasKey(e => e.Id);
            modelBuilder.Entity<ContractTransactionFeature>().HasKey(e => e.Id);
            modelBuilder.Entity<DealerPositionFeature>().HasKey(e => e.Id);
        }

    }
}
