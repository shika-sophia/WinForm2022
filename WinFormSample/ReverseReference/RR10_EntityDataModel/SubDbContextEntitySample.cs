/*
 *@see MainPrepareEntityDataModelSample.cs
 *@see MainDbContextEntitySample.cs
 */
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel
{
    public partial class SubDbContextEntitySample : DbContext
    {
        private readonly DbConnection conn;
        public virtual DbSet<PersonRR> PersonRR { get; set; }

        public SubDbContextEntitySample() : this(BuildDbConnection())
        { }//constructor

        public SubDbContextEntitySample(DbConnection conn)
            : base(existingConnection: conn, contextOwnsConnection: true)
        {
            this.PersonRR.Load();
        }//constructor

        private static DbConnection BuildDbConnection()
        {
            //copyFrom 〔MainDbContextEntitySample.cs〕
            string connectionString =
                @"data source =(LocalDB)\MSSQLLocalDB;initial catalog = ASPState;integrated security = True;";

            var entityBld = new EntityConnectionStringBuilder();
            entityBld.Provider = "System.Data.SqlClient";
            entityBld.ProviderConnectionString = connectionString;
            entityBld.Metadata =
                "res://*/WinFormSample.ReverseReference.RR10_EntityDataModel.EntitiDataModelRR.csdl | " +
                "res://*/WinFormSample.ReverseReference.RR10_EntityDataModel.EntitiDataModelRR.ssdl | " +
                "res://*/WinFormSample.ReverseReference.RR10_EntityDataModel.EntitiDataModelRR.msl";

            return new EntityConnection(entityBld.ToString());
        }//BuildDbConnection()


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { }
    }//class
}
