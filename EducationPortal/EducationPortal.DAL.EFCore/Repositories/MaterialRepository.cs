using EducationPortal.Core;
using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EducationPortal.DAL.Repositories
{
    public class MaterialRepository : IRepository<Material>
    {
        protected readonly DbContext context;
        protected readonly DbSet<Material> entities;

        public MaterialRepository(DbContext context)
        {
            this.context = context;
            this.entities = this.context.Set<Material>();
        }

        public int Count()
        {
            return entities.CountAsync().Result;
        }

        public bool Exist(int id)
        {
            return entities.AnyAsync(x => x.Id == id).Result;
        }

        public int FindIndex(string name)
        {
            return entities.Where(x => x.Name == name).Select(x => x.Id).FirstOrDefault();
        }

        public Material Find(Specification<Material> specification)
        {
            var query = entities.AsQueryable();
            if (specification.Include != null)
            {
                foreach (var include in specification.Include)
                {
                    query = query.Include(include);
                }
            }
            return query.Where(specification.Expression)
                .SingleOrDefault();
        }

        public PagedList<Material> LoadList(Specification<Material> specification, int pageNumber, int pageSize)
        {
            var query = entities.AsQueryable();
            if (specification.Include != null)
            {
                foreach (var include in specification.Include)
                {
                    query = query.Include(include);
                }
            }
            return query.Where(specification.Expression)
                .OrderBy(x => x.Id).AsQueryable()
                .ToPagedList(pageNumber, pageSize);
        }

        public bool Save(Material entity)
        {
            try
            {
                entities.Add(entity);
                context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Update(Material entity)
        {
            try
            {
                entities.Update(entity);
                context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}