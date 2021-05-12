using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.BusinessLogic
{
    public interface IDbContext
    {
        public bool SaveUser(User user);
        public User LoadUser(string login);
        public int UserCount();
    }
}
