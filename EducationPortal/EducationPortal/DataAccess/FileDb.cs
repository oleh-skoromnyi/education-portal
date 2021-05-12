using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using EducationPortal.BusinessLogic;

namespace EducationPortal.DataAccess
{
    public class FileDb:IDbContext
    {
        public bool SaveUser(User user)
        {
            if (!UserExist(user.Login))
            { 
                string jsonString = JsonSerializer.Serialize(user);
                File.WriteAllText($"{user.Login}.db", jsonString);
                return true;
            }
            else
            {
                return false;
            }
        }

        public User LoadUser(string login)
        {
            if (UserExist(login))
            {
                string jsonString = File.ReadAllText($"{login}.db");
                Type userType = typeof(User);
                User user = (User)JsonSerializer.Deserialize(jsonString, userType);
                return user;
            }
            else
            {
                return null;
            }
            
        }

        public bool UserExist(string login)
        {
            if (File.Exists($"{login}.db"))
            { 
                return true; 
            }
            else
            {
                return false; 
            }
        }

        public int UserCount()
        {
            return Directory.GetFiles("/", "*.db").Length;
        }
    }
}
