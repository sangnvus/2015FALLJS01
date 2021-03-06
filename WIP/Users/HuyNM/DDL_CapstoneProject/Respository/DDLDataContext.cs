﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Respository
{
    public class DDLDataContext : DbContext
    {
        #region "Tables"

        public DbSet<DDL_User> DDL_Users { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ReportUser> ReportUsers { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ReportProject> ReportProjects { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<UpdateLog> UpdateLogs { get; set; }
        public DbSet<Timeline> Timelines { get; set; }
        public DbSet<RewardPkg> RewardPkgs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Remind> Reminds { get; set; }
        public DbSet<Backing> Backings { get; set; }
        #endregion

        public DDLDataContext()
            : base(DDLConstants.ConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // This line to remove "S" character of table name in database.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // This line set cascadedelete to false for all one-many relation.
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}