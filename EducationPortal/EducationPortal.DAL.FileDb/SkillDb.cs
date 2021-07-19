using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using EducationPortal.Core;

namespace EducationPortal.DAL
{
    public class SkillDb : IRepository<Skill>
    {
        private readonly string defaultPath = "Skill.db";

        public SkillDb(string pathToDb)
        {
            defaultPath = pathToDb;
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            var skillList = await LoadListAsync(cancellationToken);
            return skillList.Count;
        }

        public async Task<bool> InsertAsync(Skill entity, CancellationToken cancellationToken = default)
        {
            var skillList = await LoadListAsync(cancellationToken);
            if (!skillList.Any(x => x.Id == entity.Id))
            {
                var temp = entity;
                temp.Id = skillList.Count() + 1;
                skillList.Add(temp);
                var jsonString = JsonSerializer.Serialize(skillList);
                await File.WriteAllTextAsync(defaultPath, jsonString, cancellationToken);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Skill entity, CancellationToken cancellationToken = default)
        {
            var skillList = await LoadListAsync(cancellationToken);
            var index = skillList.FindIndex(x => x.Id == entity.Id);
            if (index != -1)
            {
                skillList[index] = entity;
                var jsonString = JsonSerializer.Serialize(skillList);
                await File.WriteAllTextAsync(defaultPath, jsonString, cancellationToken);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Skill> LoadAsync(int id, CancellationToken cancellationToken = default)
        {
            var skillList = await LoadListAsync(cancellationToken);
            return skillList.Find(x => x.Id == id);
        }

        public async Task<bool> ExistAsync(Specification<Skill> specification, CancellationToken cancellationToken = default)
        {
            var skillList = await LoadListAsync(cancellationToken);
            return skillList.AsQueryable().Any(specification.Expression);
        }

        private async Task<List<Skill>> LoadListAsync(CancellationToken cancellationToken = default)
        {
            if (!File.Exists(defaultPath))
            {
                File.Create(defaultPath).Close();
            }
            string jsonString = await File.ReadAllTextAsync(defaultPath, cancellationToken);
            if (jsonString == "")
            {
                return new List<Skill>();
            }
            else
            {
                return JsonSerializer.Deserialize<List<Skill>>(jsonString);
            }
        }

        public async Task<Skill> FindAsync(Specification<Skill> specification, CancellationToken cancellationToken = default)
        {
            var materialList = await LoadListAsync(cancellationToken);
            return materialList.AsQueryable().Where(specification.Expression).SingleOrDefault();
        }

        public async Task<PagedList<Skill>> LoadListAsync(Specification<Skill> specification, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var items = await LoadListAsync(cancellationToken);
            items = items.AsQueryable().Where(specification.Expression).OrderBy(x => x.Id).ToList();
            return new PagedList<Skill>(pageNumber, pageSize, items.Count(), items.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }
    }
}
