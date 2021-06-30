using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core
{
    public interface IMaterialService
    {
        public bool AddMaterial(Material material);
        public bool ChangeMaterial(Material material);
        public PagedList<Material> GetMaterials(int pageNumber, int pageSize);
        public Material GetMaterial(int id);
    }
}
