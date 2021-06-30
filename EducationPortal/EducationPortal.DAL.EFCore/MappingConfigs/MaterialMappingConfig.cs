using EducationPortal.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.DAL.MappingConfigs
{
    public class MaterialMappingConfig : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.ToTable("Materials", "sch");

            builder.HasKey(x => x.Id)
                    .IsClustered();

            builder.Property(x => x.Name)
                    .HasMaxLength(128)
                    .IsRequired()
                    .IsUnicode();
        }
    }
}
