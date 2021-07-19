using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<bool> InsertAsync(User entity, CancellationToken cancellationToken = default)
        {
            var userList = await LoadListAsync(cancellationToken);
            if (!userList.Any(x => x.Login == entity.Login))
            {
                var temp = entity;
                temp.Id = userList.Count() + 1;
                userList.Add(temp);
                var jsonString = JsonSerializer.Serialize(userList);
                await File.WriteAllTextAsync(defaultPath, jsonString, cancellationToken);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(User entity, CancellationToken cancellationToken = default)
        {
            var userList = await LoadListAsync(cancellationToken);
            var index = userList.FindIndex(x => x.Id == entity.Id);
            if (index != -1)
            {
                userList[index] = entity;
                var jsonString = JsonSerializer.Serialize(userList);
                await File.WriteAllTextAsync(defaultPath, jsonString, cancellationToken);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ExistAsync(Specification<User> specification, CancellationToken cancellationToken = default)
        {
            var userList = await LoadListAsync(cancellationToken);
            return userList.AsQueryable().Any(specification.Expression);
        }

        private async Task<List<User>> LoadListAsync(CancellationToken cancellationToken = default)
        {
            if (!File.Exists(defaultPath))
            {
                File.Create(defaultPath).Close();
            }
            var jsonString = await File.ReadAllTextAsync(defaultPath, cancellationToken);
            if (jsonString == "")
            {
                return new List<User>();
            }
            else
            {
                return JsonSerializer.Deserialize<List<User>>(jsonString);
            }
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            var userList = await LoadListAsync(cancellationToken);
            return userList.Count;
        }

        public async Task<User> FindAsync(Specification<User> specification, CancellationToken cancellationToken = default)
        {
            var materialList = await LoadListAsync(cancellationToken);
            return materialList.AsQueryable().Where(specification.Expression).SingleOrDefault();
        }

        public async Task<PagedList<User>> LoadListAsync(Specification<User> specification, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var items = await LoadListAsync(cancellationToken);
            items = items.AsQueryable().Where(specification.Expression).OrderBy(x => x.Id).ToList();
            return new PagedList<User>(pageNumber, pageSize, items.Count(), items.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }
    }
}
