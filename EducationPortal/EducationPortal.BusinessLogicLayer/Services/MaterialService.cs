using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using EducationPortal.Core;

namespace EducationPortal.BLL
{
    public class MaterialService : IMaterialService
    {
        private IRepository<Material> _repository;

        public MaterialService(IRepository<Material> repos)
        {
            this._repository = repos;
        }

        public bool AddMaterial(Material material)
        {
            if (material != null)
            {
                if (_repository.FindIndex(material.Name) == 0)
                { 
                    if (_repository.Save(material))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ChangeMaterial(Material material)
        {
            if (_repository.FindIndex(material.Name) != 0)
            {
                if (_repository.Update(material))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Material GetMaterial(int id)
        {
            var specification = new Specification<Material>(x => x.Id == id);
            return _repository.Find(specification);
        }

        public PagedList <Material> GetMaterials(int pageNumber, int pageSize)
        {
            var specification = new Specification<Material>(x => true);
            return _repository.LoadList(specification,pageNumber,pageSize);
        }
    }
}
