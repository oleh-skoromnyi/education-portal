﻿using EducationPortal.Core;
using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EducationPortal.DAL.Repositories
{
    public class SkillRepository : IRepository<Skill>
    {
        protected readonly DbContext context;
        protected readonly DbSet<Skill> entities;

        public SkillRepository(DbContext context)
        {
            this.context = context;
            this.entities = this.context.Set<Skill>();
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

        public Skill Find(Specification<Skill> specification)
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

        public PagedList<Skill> LoadList(Specification<Skill> specification, int pageNumber, int pageSize)
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

        public bool Save(Skill entity)
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

        public bool Update(Skill entity)
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
