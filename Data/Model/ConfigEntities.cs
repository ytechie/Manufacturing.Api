using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Manufacturing.Framework.Dto;

namespace Manufacturing.Api.Data.Model
{
    public class ConfigEntities : DbContext
    {
        public DbSet<DatasourceConfiguration> Datasources { get; set; } 

        public ConfigEntities(string connString)
        {
            this.Database.Connection.ConnectionString = connString;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}