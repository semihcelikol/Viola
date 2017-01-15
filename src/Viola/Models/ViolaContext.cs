using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Viola.Models;

namespace Viola.DAL
{
    public class ViolaContext : IdentityDbContext<User>
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Effort> Efforts { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskAssignedUser> TaskAssignedUsers { get; set; }
        public DbSet<ProjectTeam> ProjectTeams { get; set; }


        public ViolaContext() : base("ViolaDb")
        {
            Database.SetInitializer(new ViolaInitializer());
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modellerdeki çoğul tablo isimlerini engeller.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // asp.net identity ile merge edildiği için gerekli
            modelBuilder.Configurations.Add(new IdentityUserLoginConfiguration());
            modelBuilder.Configurations.Add(new IdentityUserRoleConfiguration());


            // fk tanımlamaları
            // user
            modelBuilder.Entity<User>()
                    .HasRequired(m => m.Company)
                    .WithMany(t => t.Users)
                    .HasForeignKey(m => m.CompanyId)
                    .WillCascadeOnDelete(false);

            // company
            

            // project
            modelBuilder.Entity<Project>()
                    .HasRequired(m => m.CreatedUser)
                    .WithMany(t => t.CreatedProjects)
                    .HasForeignKey(m => m.CreatedUserId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                    .HasOptional(m => m.ModifiedUser)
                    .WithMany(t => t.ModifiedProjects)
                    .HasForeignKey(m => m.ModifiedUserId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                    .HasRequired(m => m.ManagerUser)
                    .WithMany(t => t.ManagerProjects)
                    .HasForeignKey(m => m.ManagerUserId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                    .HasRequired(m => m.Company)
                    .WithMany(t => t.Projects)
                    .HasForeignKey(m => m.CompanyId)
                    .WillCascadeOnDelete(false);


            // effort
            modelBuilder.Entity<Effort>()
                    .HasRequired(m => m.CreatedUser)
                    .WithMany(t => t.CreatedEfforts)
                    .HasForeignKey(m => m.CreatedUserId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Effort>()
                    .HasOptional(m => m.ModifiedUser)
                    .WithMany(t => t.ModifiedEfforts)
                    .HasForeignKey(m => m.ModifiedUserId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Effort>()
                    .HasRequired(m => m.Company)
                    .WithMany(t => t.Efforts)
                    .HasForeignKey(m => m.CompanyId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Effort>()
                    .HasRequired(m => m.Task)
                    .WithMany(t => t.Efforts)
                    .HasForeignKey(m => m.TaskId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Effort>()
                    .HasRequired(m => m.User)
                    .WithMany(t => t.Efforts)
                    .HasForeignKey(m => m.UserId)
                    .WillCascadeOnDelete(false);


            // task
            modelBuilder.Entity<Task>()
                    .HasRequired(m => m.CreatedUser)
                    .WithMany(t => t.CreatedTasks)
                    .HasForeignKey(m => m.CreatedUserId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                    .HasOptional(m => m.ModifiedUser)
                    .WithMany(t => t.ModifiedTasks)
                    .HasForeignKey(m => m.ModifiedUserId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                    .HasRequired(m => m.Company)
                    .WithMany(t => t.Tasks)
                    .HasForeignKey(m => m.CompanyId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                    .HasRequired(m => m.Project)
                    .WithMany(t => t.Tasks)
                    .HasForeignKey(m => m.ProjectId)
                    .WillCascadeOnDelete(false);


            // task assigned user
            modelBuilder.Entity<TaskAssignedUser>()
                    .HasRequired(m => m.CreatedUser)
                    .WithMany(t => t.CreatedTaskAssignedUsers)
                    .HasForeignKey(m => m.CreatedUserId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskAssignedUser>()
                    .HasRequired(m => m.User)
                    .WithMany(t => t.TaskAssignedUsers)
                    .HasForeignKey(m => m.UserId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskAssignedUser>()
                    .HasRequired(m => m.Task)
                    .WithMany(t => t.TaskAssingedUsers)
                    .HasForeignKey(m => m.TaskId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskAssignedUser>()
                    .HasRequired(m => m.Company)
                    .WithMany(t => t.TaskAssingedUsers)
                    .HasForeignKey(m => m.CompanyId)
                    .WillCascadeOnDelete(false);

            // project team
            modelBuilder.Entity<ProjectTeam>()
                    .HasRequired(m => m.CreatedUser)
                    .WithMany(t => t.CreatedProjectTeams)
                    .HasForeignKey(m => m.CreatedUserId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProjectTeam>()
                    .HasRequired(m => m.User)
                    .WithMany(t => t.ProjectTeams)
                    .HasForeignKey(m => m.UserId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProjectTeam>()
                    .HasRequired(m => m.Project)
                    .WithMany(t => t.ProjectTeams)
                    .HasForeignKey(m => m.ProjectId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProjectTeam>()
                    .HasRequired(m => m.Company)
                    .WithMany(t => t.ProjectTeams)
                    .HasForeignKey(m => m.CompanyId)
                    .WillCascadeOnDelete(false);
        }

        // asp.net identity ile merge edildiği için gerekli
        public static ViolaContext Create()
        {
            return new ViolaContext();
        }
    }


    // asp.net identity ile merge edildiği için gerekli
    public class IdentityUserLoginConfiguration : EntityTypeConfiguration<IdentityUserLogin>
    {
        public IdentityUserLoginConfiguration()
        {
            HasKey(iul => iul.UserId);
        }
    }

    public class IdentityUserRoleConfiguration : EntityTypeConfiguration<IdentityUserRole>
    {
        public IdentityUserRoleConfiguration()
        {
            HasKey(iur => iur.RoleId);
        }
    }
}