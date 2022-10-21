/*
 *@see MainPrepareEntityDataModelSample.cs
 *@see MainDbContextEntitySample.cs
 */
using System.Data.Common;
using System.Data.Entity;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel
{
    public partial class SubDbContextEntitySample : DbContext
    {
        public virtual DbSet<PersonRR> PersonRR { get; set; }

        public SubDbContextEntitySample(DbConnection conn)
            : base(existingConnection: conn, contextOwnsConnection: true)
        { }//constructor
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { }
    }//class
}
