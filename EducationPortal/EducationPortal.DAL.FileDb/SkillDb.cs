using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public int Count()
        {
            List<Skill> skillList = LoadList();
            return skillList.Count;
        }

        public bool Save(Skill entity)
        {
            List<Skill> skillList = LoadList();
            if (!skillList.Any(x => x.Id == entity.Id))
            {
                var temp = entity;
                temp.Id = skillList.Count() + 1;
                skillList.Add(temp);
                var jsonString = JsonSerializer.Serialize(skillList);
                File.WriteAllText(defaultPath, jsonString);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Update(Skill entity)
        {
            List<Skill> skillList = LoadList();
            int index = skillList.FindIndex(x => x.Id == entity.Id);
            if (index != -1)
            {
                skillList[index] = entity;
                var jsonString = JsonSerializer.Serialize(skillList);
                File.WriteAllText(defaultPath, jsonString);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Skill Load(int id)
        {
            List<Skill> skillList = LoadList();
            return skillList.Find(x => x.Id == id);
        }

        public bool Exist(int id)
        {
            List<Skill> skillList = LoadList();
            return skillList.Exists(x => x.Id == id);
        }

        public List<Skill> LoadList()
        {
            if (!File.Exists(defaultPath))
            {
                File.Create(defaultPath).Close();
            }
            string jsonString = File.ReadAllText(defaultPath);
            if (jsonString == "")
            {
                return new List<Skill>();
            }
            else
            {
                return JsonSerializer.Deserialize<List<Skill>>(jsonString);
            }
        }
        public int FindIndex(string name)
        {
            var items = LoadList();
            int index = items.FindIndex(x => x.Name == name);
            return index + 1;
        }

        public Skill Find(Specification<Skill> specification)
        {
            List<Skill> materialList = LoadList();
            return materialList.Find(specification.IsSatisfiedBy);
        }

        public PagedList<Skill> LoadList(Specification<Skill> specification, int pageNumber, int pageSize)
        {
            var items = LoadList().Where(specification.IsSatisfiedBy);
            items = items.OrderBy(x => x.Id).ToList();
            return new PagedList<Skill>(pageNumber, pageSize, items.Count(), items.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }
    }
}
