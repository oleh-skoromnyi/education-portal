using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EducationPortal.Core
{
    public interface IRepository<T> where T : Entity
    {
        public Task<bool> InsertAsync(T entity, CancellationToken cancellationToken = default);
        public Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        public Task<bool> ExistAsync(Specification<T> specification, CancellationToken cancellationToken = default);
        public Task<T> FindAsync(Specification<T> specification, CancellationToken cancellationToken = default);
        public Task<PagedList<T>> LoadListAsync(Specification<T> specification, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        public Task<int> CountAsync(CancellationToken cancellationToken = default);
    }
}
