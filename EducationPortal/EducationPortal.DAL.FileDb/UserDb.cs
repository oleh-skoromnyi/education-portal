using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using EducationPortal.Core;
using System.Linq;

namespace EducationPortal.DAL
{
    public class UserDb : IRepository<User>
    {
        private readonly string defaultPath = "User.db";

        public UserDb(string pathToDb)
        {
            defaultPath = pathToDb;
        }

        public bool Save(User entity)
        {
            List<User> userList = LoadList();
            if (!userList.Any(x => x.Login == entity.Login))
            {
                var temp = entity;
                temp.Id = userList.Count() + 1;
                userList.Add(temp);
                var jsonString = JsonSerializer.Serialize(userList);
                File.WriteAllText(defaultPath, jsonString);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Update(User entity)
        {
            List<User> userList = LoadList();
            int index = userList.FindIndex(x => x.Id == entity.Id);
            if (index != -1)
            {
                userList[index] = entity;
                var jsonString = JsonSerializer.Serialize(userList);
                File.WriteAllText(defaultPath, jsonString);
                return true;
            }
            else
            {
                return false;
            }
        }

        public User Load(int id)
        {
            List<User> userList = LoadList();
            return userList.Find(x => x.Id == id);
        }

        public bool Exist(int id)
        {
            List<User> userList = LoadList();
            return userList.Exists(x => x.Id == id);
        }

        public List<User> LoadList()
        {
            if (!File.Exists(defaultPath))
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
            List<User> userList = LoadList();
            return userList.Count;
        }
        public int FindIndex(string name)
        {
            var items = LoadList();
            int index = items.FindIndex(x => x.Login == name);
            return index + 1;
        }

        public User Find(Specification<User> specification)
        {
            List<User> materialList = LoadList();
            return materialList.Find(specification.IsSatisfiedBy);
        }

        public PagedList<User> LoadList(Specification<User> specification, int pageNumber, int pageSize)
        {
            var items = LoadList().Where(specification.IsSatisfiedBy);
            items = items.OrderBy(x => x.Id).ToList();
            return new PagedList<User>(pageNumber, pageSize, items.Count(), items.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }
    }
}
