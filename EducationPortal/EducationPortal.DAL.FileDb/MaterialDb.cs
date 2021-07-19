using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            var materialList = await LoadListAsync(cancellationToken);
            return materialList.Count();
        }

        public async Task<bool> InsertAsync(Material entity, CancellationToken cancellationToken = default)
        {
            var materialList = await LoadListAsync(cancellationToken);
            if (!materialList.Any(x => x.Id == entity.Id))
            {
                var temp = entity;
                temp.Id = materialList.Count() + 1;
                materialList.Add(temp);
                return await SaveListAsync(materialList);
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Material entity, CancellationToken cancellationToken = default)
        {
            var materialList = await LoadListAsync(cancellationToken);
            var index = materialList.FindIndex(x => x.Id == entity.Id);
            if (index != -1)
            {
                materialList[index] = entity;
                return await SaveListAsync(materialList);
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ExistAsync(Specification<Material> specification, CancellationToken cancellationToken = default)
        {
            var materialList = await LoadListAsync(cancellationToken);
            return materialList.AsQueryable().Any(specification.Expression);
        }


        private async Task<List<Material>> LoadListAsync(CancellationToken cancellationToken = default)
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

            var taskArticleRead = File.ReadAllTextAsync(defaultPathArticle, cancellationToken);
            var taskBookRead = File.ReadAllTextAsync(defaultPathBook, cancellationToken);
            var taskTestRead = File.ReadAllTextAsync(defaultPathTest, cancellationToken);
            var taskVideoRead = File.ReadAllTextAsync(defaultPathVideo, cancellationToken);

            var jsonArticleString = await taskArticleRead;
            var jsonBookString = await taskBookRead;
            var jsonTestString = await taskTestRead;
            var jsonVideoString = await taskVideoRead;

            if (jsonArticleString != "")
            {
                result.AddRange(JsonSerializer.Deserialize<List<InternetArticleMaterial>>(jsonArticleString));
            }
            if (jsonBookString != "")
            {
                result.AddRange(JsonSerializer.Deserialize<List<DigitalBookMaterial>>(jsonBookString));
            }
            if (jsonTestString != "")
            {
                result.AddRange(JsonSerializer.Deserialize<List<TestMaterial>>(jsonTestString));
            }
            if (jsonVideoString != "")
            {
                result.AddRange(JsonSerializer.Deserialize<List<VideoMaterial>>(jsonVideoString));
            }
            return result;
        }

        private async Task<bool> SaveListAsync(List<Material> materialList, CancellationToken cancellationToken = default)
        {
            var jsonArticleString = JsonSerializer.Serialize(materialList.OfType<InternetArticleMaterial>());
            var jsonBookString = JsonSerializer.Serialize(materialList.OfType<DigitalBookMaterial>());
            var jsonTestString = JsonSerializer.Serialize(materialList.OfType<TestMaterial>());
            var jsonVideoString = JsonSerializer.Serialize(materialList.OfType<VideoMaterial>());
            try
            {
                var articleWriteTask = File.WriteAllTextAsync(defaultPathArticle, jsonArticleString, cancellationToken);
                var bookWriteTask = File.WriteAllTextAsync(defaultPathBook, jsonBookString, cancellationToken);
                var testWriteTask = File.WriteAllTextAsync(defaultPathTest, jsonTestString, cancellationToken);
                var videoWriteTask = File.WriteAllTextAsync(defaultPathVideo, jsonVideoString, cancellationToken);

                await articleWriteTask;
                await bookWriteTask;
                await testWriteTask;
                await videoWriteTask;
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<Material> FindAsync(Specification<Material> specification, CancellationToken cancellationToken = default)
        {
            var materialList = await LoadListAsync(cancellationToken);
            return materialList.AsQueryable().Where(specification.Expression).SingleOrDefault(); ;
        }

        public async Task<PagedList<Material>> LoadListAsync(Specification<Material> specification, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var items = await LoadListAsync(cancellationToken);
            items = items.AsQueryable().Where(specification.Expression).OrderBy(x => x.Id).ToList();
            return new PagedList<Material>(pageNumber, pageSize, items.Count(), items.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }
    }
}
