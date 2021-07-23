using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using EducationPortal.Core;
using FluentValidation;

namespace EducationPortal.BLL
{
    public class MaterialService : IMaterialService
    {
        private IRepository<Material> _repository;
        private IValidator<Material> _materialValidator;

        public MaterialService(IRepository<Material> repos, IValidator<Material> materialValidator)
        {
            this._repository = repos;
            this._materialValidator = materialValidator;
        }

        public async Task<bool> AddMaterialAsync(Material material, CancellationToken cancellationToken = default)
        {
            if ((await _materialValidator.ValidateAsync(material, cancellationToken)).IsValid)
            {
                if (await _repository.InsertAsync(material, cancellationToken))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> ChangeMaterialAsync(Material material, CancellationToken cancellationToken = default)
        {
            if ((await _materialValidator.ValidateAsync(material, cancellationToken)).IsValid)
            {
                if (await _repository.UpdateAsync(material, cancellationToken))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<Material> GetMaterialAsync(int materialId, CancellationToken cancellationToken = default)
        {
            var specification = new FindByIdSpecification<Material>(materialId);
            return await _repository.FindAsync(specification, cancellationToken);
        }

        public async Task<PagedList<Material>> GetMaterialsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var specification = new Specification<Material>(x => true);
            return await _repository.LoadListAsync(specification, pageNumber, pageSize, cancellationToken);
        }
    }
}
