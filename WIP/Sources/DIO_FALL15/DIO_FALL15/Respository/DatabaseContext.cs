using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DIO_FALL15.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DIO_FALL15.Respository
{
    public class DatabaseContext: DbContext
    {
        // Tables.
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Back> Backs { get; set; }

        public DatabaseContext() : base("DatabaseContext")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // This line to remove "S" character of table name in database.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}