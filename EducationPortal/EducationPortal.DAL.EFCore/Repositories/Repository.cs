using EducationPortal.Core;
using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace EducationPortal.DAL.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : Entity
    {
        protected readonly DbContext context;
        protected readonly DbSet<T> entities;

        public Repository(DbContext context)
        {
            this.context = context;
            this.entities = this.context.Set<T>();
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await entities.CountAsync(cancellationToken);
        }

        public async Task<bool> ExistAsync(Specification<T> specification, CancellationToken cancellationToken = default)
        {
            return await entities.AnyAsync(specification.Expression, cancellationToken);
        }

        public async Task<T> FindAsync(Specification<T> specification, CancellationToken cancellationToken = default)
        {
            var query = entities.AsQueryable();
            if (specification.Include != null)
            {
                foreach (var include in specification.Include)
                {
                    query = query.Include(include);
                }
            }
            return await query.Where(specification.Expression)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<PagedList<T>> LoadListAsync(Specification<T> specification, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = entities.AsQueryable();
            if (specification.Include != null)
            {
                foreach (var include in specification.Include)
                {
                    query = query.Include(include);
                }
            }
            return await query.Where(specification.Expression)
                .OrderBy(x => x.Id)
                .ToPagedListAsync(pageNumber, pageSize, cancellationToken);
        }

        public async Task<bool> InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                entities.Add(entity);
                await context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                entities.Update(entity);
                await context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
