using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core
{
    public interface IUserService
    {
        public string GetUserLogin(int id);
        public bool StartLearnCourse(int id, Course course);
        public bool LearnMaterial(int id, Material material);
        public PagedList<UserCourse> GetCourses(int id, int pageNumber, int pageSize);
        public PagedList<Course> GetAvailableCourses(int id, int pageNumber, int pageSize);
        public UserCourse GetCourse(int userId, int courseId);
        public bool ChangePersonalData(User user);
        public PagedList<UserSkill> GetSkills(int id, int pageNumber, int pageSize);
        public User GetPersonalData(int id);
    }
}
