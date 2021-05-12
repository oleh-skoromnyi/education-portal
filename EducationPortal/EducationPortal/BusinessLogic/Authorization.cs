using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.BusinessLogic
{
    public class Authorization
    {
        private IDbContext dbContext;
        private User user;

        public Authorization(IDbContext context)
        {
            this.dbContext = context;
        }
        public bool Login(string login, string password)
        {
            User temp = dbContext.LoadUser(login);
            if(temp.Password == password)
            {
                this.user = temp;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Register(string login, string password)
        {
            User temp = new User(
                dbContext.UserCount(),
                login,
                password);
            return dbContext.SaveUser(temp);
        }
        public string GetLogin()
        {
            if (user != null)
            {
                return user.Login;
            }
            else
            {
                return "Anonim";
            }

        }
    }
}
