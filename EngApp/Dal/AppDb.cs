using EngApp.Dal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngApp.Dal
{
    public class AppDb:IdentityDbContext
    {
        public AppDb(DbContextOptions option):base(option){}
       protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>().HasKey(x => new { x.customerId });
            builder.Entity<PaperData>().HasKey(x => new { x.paperdataId });
            builder.Entity<Transaction>().HasKey(x => new { x.transactionId });
            builder.Entity<CustTrans>().HasKey(x => new { x.custtransId});
            builder.Entity<Paper>().HasKey(x => new { x.paperId });
            builder.Entity<PayMent>().HasKey(x => new { x.paymentId });
            base.OnModelCreating(builder);
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PaperData> paperDatas { set; get; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<CustTrans> CustTrans { set; get; }
        public DbSet<Paper> papers { get; set; }
        public DbSet<PayMent> payMents { set; get; }

    }
}
