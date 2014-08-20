using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Manufacturing.Framework.Datasource;

namespace Manufacturing.Api.Data.Model
{
    public class DataEntities : DbContext
    {
        public DbSet<DataRecord> DataRecords { get; set; }

        public DataEntities(string connString)
        {
            this.Database.Connection.ConnectionString = connString;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<DataRecord>().ToTable("Raw");
        }
    }
}