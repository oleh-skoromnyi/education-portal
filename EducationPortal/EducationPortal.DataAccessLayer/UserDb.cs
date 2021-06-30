using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using EducationPortal.BusinessLogicLayer;

namespace EducationPortal.DataAccessLayer
{
    public class UserDb : IDbContext<User>
    {
        private readonly string defaultPath = "User.db";
        public bool Save(User user)
        {
            List<User> userList = LoadUserList();
            if (!userList.Exists(x => x.Login == user.Login))
            {
                userList.Add(user);
                var jsonString = JsonSerializer.Serialize(userList);
                File.WriteAllText(defaultPath, jsonString);
                return true;
            }
            else
            {
                return false;
            }
        }

        public User Load(string login)
        {
            List<User> userList = LoadUserList();
            return userList.Find(x => x.Login == login);
        }

        public bool UserExist(string login)
        {
            List<User> userList = LoadUserList();
            return userList.Exists(x => x.Login == login);
        }

        private List<User> LoadUserList()
        {
            if(!File.Exists(defaultPath))
            {
                File.Create(defaultPath).Close();
            }
            string jsonString = File.ReadAllText(defaultPath);
            if (jsonString == "")
            { 
                return new List<User>();
            }
            else
            {
                return JsonSerializer.Deserialize<List<User>>(jsonString);
            }
        }

        public int Count()
        {
            List<User> userList = LoadUserList();
            return userList.Count;
        }
    }
}
