using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using EducationPortal.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EducationPortal.DAL.Contexts
{
    public class EducationPortalDbContext : DbContext
    {
        public DbSet<Material> Materials { get; set; }
        public DbSet<DigitalBookMaterial> DigitalBookMaterials { get; set; }
        public DbSet<InternetArticleMaterial> InternetArticleMaterials { get; set; }
        public DbSet<TestMaterial> TestMaterials { get; set; }
        public DbSet<VideoMaterial> VideoMaterials { get; set; }
        public DbSet<TestItem> TestItems { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<User> Users { get; set; }

        public EducationPortalDbContext(DbContextOptions<EducationPortalDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("sch");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<UserCourse>(builder =>
            {
                builder.HasKey(x => new { x.CourseId, x.UserId });
            });
            modelBuilder.Entity<UserSkill>(builder =>
            {
                builder.HasKey(x => new { x.SkillId, x.UserId });;
            });
            modelBuilder.Entity<RequirenmentSkill>(builder =>
            {
                builder.HasKey(x => new { x.SkillId, x.CourseId }); ;
            });

            modelBuilder.Entity<CourseMaterial>(builder =>
            {
                builder.HasKey(x => new { x.CourseId, x.MaterialId });
            });
            modelBuilder.Entity<LearnedMaterial>(builder =>
            {
                builder.HasKey(x => new { x.MaterialId, x.UserId }); ;
            });
            modelBuilder.Entity<CourseGivenSkill>(builder =>
            {
                builder.HasKey(x => new { x.SkillId, x.CourseId }); ;
            });
            modelBuilder.Entity<DigitalBookMaterial>().ToTable("DigitalBookMaterial");
            modelBuilder.Entity<InternetArticleMaterial>().ToTable("InternetArticleMaterial");
            modelBuilder.Entity<TestMaterial>().ToTable("TestMaterial");
            modelBuilder.Entity<VideoMaterial>().ToTable("VideoMaterial");
            //Init(modelBuilder);
        }
    }
}
