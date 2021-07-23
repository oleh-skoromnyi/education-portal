using EducationPortal.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.DAL.MappingConfigs
{
    public class UserMappingConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "sch");

            builder.HasKey(x => x.Id)
                    .HasName("PK_Users")
                    .IsClustered();

            builder.Property(x => x.Name)
                    .HasMaxLength(128)
                    .IsUnicode();

            builder.Property(x => x.Login)
                    .HasMaxLength(128)
                    .IsRequired()
                    .IsUnicode();

            builder.Property(x => x.Password)
                    .HasMaxLength(1024)
                    .IsRequired()
                    .IsUnicode();

            builder.Property(x => x.Email)
                    .HasMaxLength(128)
                    .IsUnicode();

            builder.Property(x => x.Phone)
                    .HasMaxLength(128)
                    .IsUnicode();

            builder.HasAlternateKey(x => x.Login)
                    .HasName("UK_Login");

            builder.HasMany(x => x.CourseList)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.MaterialList)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.SkillList)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
