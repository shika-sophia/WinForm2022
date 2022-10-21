/*
 *@see MainPrepareEntityDataModelSample.cs
 *@see MainDbContextEntitySample.cs
 */
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel
{
    public partial class SubDbContextEntityPersonRR : DbContext
    {
        public virtual DbSet<PersonRR> PersonRR { get; set; }

        public SubDbContextEntityPersonRR() : this(BuildDbConnection())
        { }//constructor

        public SubDbContextEntityPersonRR(DbConnection conn)
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
                "res://*/WinFormSample.ReverseReference.RR10_EntityDataModel.EntityDataModelRR.csdl | " +
                "res://*/WinFormSample.ReverseReference.RR10_EntityDataModel.EntityDataModelRR.ssdl | " +
                "res://*/WinFormSample.ReverseReference.RR10_EntityDataModel.EntityDataModelRR.msl";

            return new EntityConnection(entityBld.ToString());
        }//BuildDbConnection()

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { }
    }//class
}
