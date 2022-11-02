using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel.EF_CodeFirstProduct
{
    class SubDbContextEntityProductRR : DbContext
    {
        public DbSet<ProductModelRR> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { }
    }//class
}
