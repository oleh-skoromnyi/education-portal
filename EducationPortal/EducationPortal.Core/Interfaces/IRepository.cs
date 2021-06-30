using System.Collections.Generic;

namespace EducationPortal.Core
{
    public interface IRepository<T> where T : Entity
    {
        public bool Save(T entity);
        public bool Update(T entity);
        public bool Exist(int id);
        public T Find(Specification<T> specification);
        public PagedList<T> LoadList(Specification<T> specification, int pageNumber, int pageSize);
        public int Count();
        public int FindIndex(string name);
    }
}
