using EducationPortal.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.DAL.MappingConfigs
{
    public class SkillMappingConfig : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ToTable("Skills", "sch");

            builder.HasKey(x => x.Id)
                    .HasName("PK_Skills")
                    .IsClustered();

            builder.Property(x => x.Name)
                    .HasMaxLength(128)
                    .IsRequired()
                    .IsUnicode();

            builder.Property(x => x.Description)
                    .HasMaxLength(1024)
                    .IsUnicode();
        }
    }
}