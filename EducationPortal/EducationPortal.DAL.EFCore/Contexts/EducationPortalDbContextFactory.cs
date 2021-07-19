using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EducationPortal.DAL.Contexts
{
    internal class EducationPortalDbContextFactory : IDesignTimeDbContextFactory<EducationPortalDbContext>
    {
        public EducationPortalDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EducationPortalDbContext>()
              .UseSqlServer(@"Server=.\SQLEXPRESS;Database=EducationPortalDb;Trusted_Connection=True;",
                  o =>
                  {
                      o.MigrationsHistoryTable("Migrations", "sch");
                      o.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                  });

            return new EducationPortalDbContext(optionsBuilder.Options);
        }
    }
}
