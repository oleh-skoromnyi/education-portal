using EducationPortal.Core;
using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EducationPortal.DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        protected readonly DbContext context;
        protected readonly DbSet<User> entities;

        public UserRepository(DbContext context)
        {
            this.context = context;
            this.entities = this.context.Set<User>();
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
            return entities.Where(x => x.Login == name).Select(x => x.Id).FirstOrDefault();
        }

        public User Find(Specification<User> specification)
        {
            var query = entities.AsQueryable();
            if (specification.Include != null)
            {
                foreach (var include in specification.Include)
                {
                    query = query.Include(include);
                }
            }
            return query.Where(specification.IsSatisfiedBy)
                .SingleOrDefault();
        }

        public PagedList<User> LoadList(Specification<User> specification, int pageNumber, int pageSize)
        {
            var query = entities.AsQueryable();
            if (specification.Include != null)
            {
                foreach (var include in specification.Include)
                {
                    query = query.Include(include);
                }
            }
            return query.Where(specification.IsSatisfiedBy)
                .OrderBy(x => x.Id).AsQueryable()
                .ToPagedList(pageNumber, pageSize);
        }

        public bool Save(User entity)
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

        public bool Update(User entity)
        {
            try
            {
                entities.Update(entity);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
    }
}
