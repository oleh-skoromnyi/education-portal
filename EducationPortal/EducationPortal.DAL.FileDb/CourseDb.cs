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
    public class CourseDb : IRepository<Course>
    {
        private readonly string defaultPath = "Course.db";

        public CourseDb(string pathToDb)
        {
            defaultPath = pathToDb;
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            var courseList = await LoadListAsync(cancellationToken);
            return courseList.Count;
        }

        public async Task<bool> ExistAsync(Specification<Course> specification, CancellationToken cancellationToken = default)
        {
            var courseList = await LoadListAsync(cancellationToken);
            return courseList.AsQueryable().Any(specification.Expression);
        }

        public async Task<Course> FindAsync(Specification<Course> specification, CancellationToken cancellationToken = default)
        {
            var courseList = await LoadListAsync();
            return courseList.AsQueryable().Where(specification.Expression).SingleOrDefault();
        }


        public async Task<List<Course>> LoadListAsync(CancellationToken cancellationToken = default)
        {
            if (!File.Exists(defaultPath))
            {
                File.Create(defaultPath).Close();
            }
            var jsonString = await File.ReadAllTextAsync(defaultPath, cancellationToken);
            if (jsonString == "")
            {
                return new List<Course>();
            }
            else
            {
                return JsonSerializer.Deserialize<List<Course>>(jsonString);
            }
        }

        public async Task<PagedList<Course>> LoadListAsync(Specification<Course> specification, int pageNumber, int pageSize,CancellationToken cancellationToken = default)
        {
            if (!File.Exists(defaultPath))
            {
                File.Create(defaultPath).Close();
            }
            var jsonString = await File.ReadAllTextAsync(defaultPath, cancellationToken);
            if (jsonString == "")
            {
                return new PagedList<Course>(pageNumber,pageSize,0,new List<Course>());
            }
            else
            {
                var items = JsonSerializer.Deserialize<List<Course>>(jsonString).AsQueryable()
                    .Where(specification.Expression);
                return new PagedList<Course>(pageNumber, pageSize, items.LongCount(), items.Skip(--pageNumber*pageSize).Take(pageSize));
            }
        }


        public async Task<bool> InsertAsync(Course entity, CancellationToken cancellationToken = default)
        {
            var courseList = await LoadListAsync(cancellationToken);
            if (!courseList.Any(x => x.Name == entity.Name))
            {
                var temp = entity;
                temp.Id = courseList.Count()+1;
                courseList.Add(temp);
                var jsonString = JsonSerializer.Serialize(courseList);
                File.WriteAllText(defaultPath, jsonString);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Course entity, CancellationToken cancellationToken = default)
        {
            var courseList = await LoadListAsync(cancellationToken);
            int index = courseList.FindIndex(x => x.Id == entity.Id);
            if (index != -1)
            {
                courseList[index] = entity;
                var jsonString = JsonSerializer.Serialize(courseList);
                File.WriteAllText(defaultPath, jsonString);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
