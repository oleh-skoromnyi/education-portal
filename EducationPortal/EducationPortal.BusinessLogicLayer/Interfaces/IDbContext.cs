using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.BusinessLogicLayer
{
    public interface IDbContext<T> where T : class
    {
        public bool Save(T user);
        public T Load(string name);
        public int Count();
    }
}
