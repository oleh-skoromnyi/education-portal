using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using EducationPortal.Core;

namespace EducationPortal.BLL
{
    public class CourseService : ICourseService
    {
        private IRepository<Course> _repository;

        public CourseService(IRepository<Course> repos)
        {
            this._repository = repos;
        }

        public bool AddCourse(int authorId, Course course)
        {
            if (_repository.FindIndex(course.Name) == 0)
            {
                course.AuthorId = authorId;
                if (_repository.Save(course))
                {
                    return true;
                }
            }
            return false;
        }

        public bool AddMaterialToCourse(int userId, Material material, int courseId)
        {
            if (_repository.Exist(courseId))
            {
                var specification = new Specification<Course>(x => x.Id == courseId);
                specification.Include.Add(x => x.MaterialList);
                var course = _repository.Find(specification);
                if (course.AuthorId == userId && !course.IsPublished)
                {
                    var temp = new CourseMaterial(material);
                    temp.MaterialId = material.Id;
                    temp.CourseId = courseId;
                    course.MaterialList.Add(temp);
                    if (_repository.Update(course))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AddRequirenmentToCourse(int userId, RequirenmentSkill skill, int courseId)
        {
            if (_repository.Exist(courseId))
            {
                var specification = new Specification<Course>(x => x.Id == courseId);
                specification.Include.Add(x=>x.RequirementSkillList);
                var course = _repository.Find(specification);
                if (course.AuthorId == userId && !course.IsPublished)
                {
                    var temp = skill;
                    temp.SkillId = skill.Skill.Id;
                    temp.CourseId = courseId;
                    course.RequirementSkillList.Add(temp);
                    if (_repository.Update(course))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AddSkillsToCourse(int userId, Skill skill, int courseId)
        {
            if (_repository.Exist(courseId))
            {
                var specification = new Specification<Course>(x => x.Id == courseId);
                specification.Include.Add(x=>x.GivenSkillList);
                var course = _repository.Find(specification);
                if (course.AuthorId == userId && !course.IsPublished)
                {
                    var temp = new CourseGivenSkill(skill);
                    temp.SkillId = skill.Id;
                    temp.CourseId = courseId;
                    course.GivenSkillList.Add(temp);
                    if (_repository.Update(course))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Course GetCourse(int id)
        {
            if (_repository.Exist(id))
            {
                var specification = new Specification<Course>(x => x.Id == id);
                specification.Include.Add(x => x.MaterialList);
                specification.Include.Add(x => x.GivenSkillList);
                specification.Include.Add(x => x.RequirementSkillList);
                return _repository.Find(specification);
            }
            return null;
        }

        public PagedList<Course> GetCourses(int pageNumber, int pageSize)
        {
            var specification = new Specification<Course>(x => true);
            specification.Include.Add(x => x.MaterialList);
            specification.Include.Add(x => x.GivenSkillList);
            specification.Include.Add(x => x.RequirementSkillList);
            return _repository.LoadList(specification, pageNumber, pageSize);
        }

        public bool PublishCourse(int userId, int courseId)
        {
            if (_repository.Exist(courseId))
            {
                var specification = new Specification<Course>(x => x.Id == courseId);
                specification.Include.Add(x => x.MaterialList);
                var course = _repository.Find(specification);
                if (course.AuthorId == userId && !course.IsPublished && course.MaterialList.Any())
                {
                    course.IsPublished = true;
                    if (_repository.Update(course))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool RemoveMaterialFromCourse(int userId, Material material, int courseId)
        {
            if (_repository.Exist(courseId))
            {
                var specification = new Specification<Course>(x => x.Id == courseId);
                specification.Include.Add(x => x.MaterialList);
                var course = _repository.Find(specification);
                if (course.AuthorId == userId && !course.IsPublished)
                {
                    course.MaterialList.Remove(new CourseMaterial(material));
                    if (_repository.Update(course))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool RemoveRequirenmentFromCourse(int userId, RequirenmentSkill skill, int courseId)
        {
            if (_repository.Exist(courseId))
            {
                var specification = new Specification<Course>(x => x.Id == courseId);
                specification.Include.Add(x => x.RequirementSkillList);
                var course = _repository.Find(specification);
                if (course.AuthorId == userId && !course.IsPublished)
                {
                    course.RequirementSkillList.Remove(skill);
                    if (_repository.Update(course))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool RemoveSkillsFromCourse(int userId, Skill skill, int courseId)
        {
            if (_repository.Exist(courseId))
            {
                var specification = new Specification<Course>(x => x.Id == courseId);
                specification.Include.Add(x => x.GivenSkillList);
                var course = _repository.Find(specification);
                if (course.AuthorId == userId && !course.IsPublished)
                {
                    course.GivenSkillList.Remove(new CourseGivenSkill(skill));
                    if (_repository.Update(course))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
