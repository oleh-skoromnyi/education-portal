using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EducationPortal.Core
{
    public interface IMaterialService
    {
        public Task<bool> AddMaterialAsync(Material material, CancellationToken cancellationToken = default);
        public Task<bool> ChangeMaterialAsync(Material material, CancellationToken cancellationToken = default);
        public Task<PagedList<Material>> GetMaterialsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        public Task<Material> GetMaterialAsync(int id, CancellationToken cancellationToken = default);
    }
}
