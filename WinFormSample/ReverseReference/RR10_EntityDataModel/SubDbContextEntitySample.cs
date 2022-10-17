using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel
{
    public partial class SubDbContextEntitySample : DbContext
    {
        public virtual DbSet<PersonRR> PersonRR { get; set; }

        public SubDbContextEntitySample(DbConnection conn)
            : base(existingConnection: conn, contextOwnsConnection: true)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { }
    }//class
}
