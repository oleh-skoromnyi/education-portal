using EducationPortal.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.DAL.MappingConfigs
{
    public class CourseMappingConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses", "sch");

            builder.HasKey(x => x.Id)
                    .HasName("PK_Courses")
                    .IsClustered();

            builder.Property(x => x.Name)
                    .HasMaxLength(128)
                    .IsRequired()
                    .IsUnicode();

            builder.Property(x => x.Description)
                    .HasMaxLength(1024)
                    .IsUnicode();

            builder.HasMany(x => x.GivenSkillList)
                .WithOne(x=>x.Course)
                .HasForeignKey(x=>x.CourseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.RequirementSkillList)
                .WithOne(x => x.Course)
                .HasForeignKey(x => x.CourseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.MaterialList)
                .WithOne(x => x.Course)
                .HasForeignKey(x => x.CourseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
