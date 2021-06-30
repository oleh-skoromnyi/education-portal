using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using EducationPortal.Core;

namespace EducationPortal.DAL
{
    public class MaterialDb : IRepository<Material>
    {
        private readonly string defaultPathVideo = "Material.Video.db";
        private readonly string defaultPathArticle = "Material.Article.db";
        private readonly string defaultPathBook = "Material.Book.db";
        private readonly string defaultPathTest = "Material.Test.db";

        public MaterialDb(string pathToVideoDb, string pathToArticleDb, string pathToBookDb, string pathToTestDb)
        {
            defaultPathVideo = pathToVideoDb;
            defaultPathArticle = pathToArticleDb;
            defaultPathBook = pathToBookDb;
            defaultPathTest = pathToTestDb;
        }

        public int Count()
        {
            List<Material> materialList = LoadList();
            return materialList.Count;
        }

        public bool Save(Material entity)
        {
            List<Material> materialList = LoadList();
            if (!materialList.Any(x => x.Id == entity.Id))
            {
                var temp = entity;
                temp.Id = materialList.Count() + 1;
                materialList.Add(temp);
                SaveList(materialList);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Update(Material entity)
        {
            List<Material> materialList = LoadList();
            int index = materialList.FindIndex(x => x.Id == entity.Id);
            if (index != -1)
            {
                materialList[index] = entity;
                SaveList(materialList);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Material Find(int id)
        {
            List<Material> materialList = LoadList();
            return materialList.Find(x => x.Id == id);
        }

        public bool Exist(int id)
        {
            List<Material> materialList = LoadList();
            return materialList.Exists(x => x.Id == id);
        }

        public List<Material> LoadList()
        {
            var result = new List<Material>();
            if (!File.Exists(defaultPathArticle))
            {
                File.Create(defaultPathArticle).Close();
            }
            if (!File.Exists(defaultPathBook))
            {
                File.Create(defaultPathBook).Close();
            }
            if (!File.Exists(defaultPathTest))
            {
                File.Create(defaultPathTest).Close();
            }
            if (!File.Exists(defaultPathVideo))
            {
                File.Create(defaultPathVideo).Close();
            }

            string jsonString;
            jsonString = File.ReadAllText(defaultPathArticle);
            if (jsonString != "")
            {
                result.AddRange(JsonSerializer.Deserialize<List<InternetArticleMaterial>>(jsonString));
            }
            jsonString = File.ReadAllText(defaultPathBook);
            if (jsonString != "")
            {
                result.AddRange(JsonSerializer.Deserialize<List<DigitalBookMaterial>>(jsonString));
            }
            jsonString = File.ReadAllText(defaultPathTest);
            if (jsonString != "")
            {
                result.AddRange(JsonSerializer.Deserialize<List<TestMaterial>>(jsonString));
            }
            jsonString = File.ReadAllText(defaultPathVideo);
            if (jsonString != "")
            {
                result.AddRange(JsonSerializer.Deserialize<List<VideoMaterial>>(jsonString));
            }

            return result;
        }

        private void SaveList(List<Material> materialList)
        {
            string jsonString;
            jsonString = JsonSerializer.Serialize(materialList.OfType<InternetArticleMaterial>());
            File.WriteAllText(defaultPathArticle, jsonString);
            jsonString = JsonSerializer.Serialize(materialList.OfType<DigitalBookMaterial>());
            File.WriteAllText(defaultPathBook, jsonString);
            jsonString = JsonSerializer.Serialize(materialList.OfType<TestMaterial>());
            File.WriteAllText(defaultPathTest, jsonString);
            jsonString = JsonSerializer.Serialize(materialList.OfType<VideoMaterial>());
            File.WriteAllText(defaultPathVideo, jsonString);
        }

        public int FindIndex(string name)
        {
            var items = LoadList();
            int index = items.FindIndex(x => x.Name == name);
            return index + 1;
        }

        public Material Find(Specification<Material> specification)
        {
            List<Material> materialList = LoadList();
            return materialList.Find(specification.IsSatisfiedBy);
        }

        public PagedList<Material> LoadList(Specification<Material> specification, int pageNumber, int pageSize)
        {
            var items = LoadList().Where(specification.IsSatisfiedBy);
            items = items.OrderBy(x => x.Id).ToList();
            return new PagedList<Material>(pageNumber, pageSize, items.Count(), items.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }
    }
}
