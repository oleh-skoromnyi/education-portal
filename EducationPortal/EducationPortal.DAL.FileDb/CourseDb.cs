using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        public int Count()
        {
            List<Course> courseList = LoadList();
            return courseList.Count;
        }

        public bool Exist(int id)
        {
            List<Course> courseList = LoadList();
            return courseList.Exists(x => x.Id == id);
        }

        public int FindIndex(string name)
        {
            var items = LoadList();
            int index = items.FindIndex(x => x.Name == name);
            return index + 1;
        }

        public Course Find(int id)
        {
            List<Course> courseList = LoadList();
            return courseList.Find(x => x.Id == id);
        }

        public List<Course> LoadList()
        {
            if (!File.Exists(defaultPath))
            {
                File.Create(defaultPath).Close();
            }
            string jsonString = File.ReadAllText(defaultPath);
            if (jsonString == "")
            {
                return new List<Course>();
            }
            else
            {
                return JsonSerializer.Deserialize<List<Course>>(jsonString);
            }
        }

        public bool Save(Course entity)
        {
            List<Course> courseList = LoadList();
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

        public bool Update(Course entity)
        {
            List<Course> courseList = LoadList();
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

        public Course Find(Specification<Course> specification)
        {
            List<Course> courseList = LoadList();
            return courseList.Find(specification.IsSatisfiedBy);
        }

        public PagedList<Course> LoadList(Specification<Course> specification, int pageNumber, int pageSize)
        {
            if (!File.Exists(defaultPath))
            {
                File.Create(defaultPath).Close();
            }
            string jsonString = File.ReadAllText(defaultPath);
            if (jsonString == "")
            {
                return new PagedList<Course>(pageNumber,pageSize,0,new List<Course>());
            }
            else
            {
                var items = JsonSerializer.Deserialize<List<Course>>(jsonString);
                items = items.OrderBy(x=>x.Id).ToList();
                return new PagedList<Course>(pageNumber, pageSize, items.Count, items.Skip((pageNumber-1)*pageSize).Take(pageSize));
            }
        }
    }
}
