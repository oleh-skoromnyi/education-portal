using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.BusinessLogic
{
    public class User
    {
        public int Id { get; set; }
        //public string Name { get; private set; }
        //public string Email { get; private set; }
        //public string Phone { get; private set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public User()
        {

        }
        public User(int id,string login,string password)
        {
            Id = id;
            Login = login;
            //heshing password
            Password = password;
        }
    }
}
