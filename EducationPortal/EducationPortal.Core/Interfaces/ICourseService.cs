using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core
{
    public interface ICourseService
    {
        public bool AddCourse(int authorId, Course course);
        public bool AddMaterialToCourse(int userId, Material material,int courseId);
        public bool RemoveMaterialFromCourse(int userId, Material material, int courseId);
        public bool AddSkillsToCourse(int userId, Skill skill, int courseId);
        public bool RemoveSkillsFromCourse(int userId, Skill skill, int courseId);
        public bool AddRequirenmentToCourse(int userId, RequirenmentSkill skill, int courseId);
        public bool RemoveRequirenmentFromCourse(int userId, RequirenmentSkill skill, int courseId);
        public bool PublishCourse(int userId, int courseId);
        public Course GetCourse(int id);
        public PagedList<Course> GetCourses(int pageNumber, int pageSize);
    }
}
