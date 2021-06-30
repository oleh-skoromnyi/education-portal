using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EducationPortal.Core;

namespace EducationPortal.BLL
{
    public class UserService : IUserService
    {
        private IRepository<User> _userRepository;
        private IRepository<Course> _courseRepository;

        public UserService(IRepository<User> repos, IRepository<Course> courseRepos)
        {
            this._userRepository = repos;
            this._courseRepository = courseRepos;
        }

        public string GetUserLogin(int id)
        {
            if (_userRepository.Exist(id))
            {
                var specification = new Specification<User>(x => x.Id == id);
                return _userRepository.Find(specification).Login;
            }
            else
            {
                return "Anonim";
            }
        }

        public bool ChangePersonalData(User user)
        {
            if (_userRepository.FindIndex(user.Login) != 0)
            {
                if (_userRepository.Update(user))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public PagedList<Course> GetAvailableCourses(int id, int pageNumber, int pageSize)
        {
            var userSpecification = new Specification<User>(x => x.Id == id);
            userSpecification.Include.Add(x => x.SkillList);
            userSpecification.Include.Add(x => x.CourseList); 
            var user = _userRepository.Find(userSpecification);

            var courseSpecification = new Specification<Course>(x => !user.CourseList.Any(y=>y.CourseId == x.Id));
            courseSpecification.Include.Add(x => x.RequirementSkillList);
            var coursesList = _courseRepository.LoadList(courseSpecification, pageNumber, pageSize);
            var items = coursesList.Items.Where(x => x.RequirementSkillList
                                            .All(y => user.SkillList
                                            .Any(z => z.SkillId == y.SkillId
                                                    && z.Level >= y.Level)));

            return new PagedList<Course>(coursesList.PageNumber, coursesList.PageSize, coursesList.PageCount, items);
        }

        public UserCourse GetCourse(int userId, int courseId)
        {
            var userSpecification = new Specification<User>(x => x.Id == userId);
            userSpecification.Include.Add(x => x.CourseList);
            var user = _userRepository.Find(userSpecification);
            return user.CourseList.First(x => x.Course.Id == courseId);
        }

        public PagedList<UserCourse> GetCourses(int id,int pageNumber,int pageSize)
        {
            var userSpecification = new Specification<User>(x => x.Id == id);
            userSpecification.Include.Add(x => x.CourseList);
            var user = _userRepository.Find(userSpecification);
            int count = 0;
            var courseList = new List<UserCourse>();
            if (user.CourseList != null)
            {
                count = user.CourseList.Count;
                courseList = user.CourseList.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            }
            return new PagedList<UserCourse>(pageNumber,pageSize,count,courseList);
        }

        public User GetPersonalData(int id)
        {
            var userSpecification = new Specification<User>(x => x.Id == id);
            userSpecification.Include.Add(x => x.CourseList);
            userSpecification.Include.Add(x => x.SkillList);
            userSpecification.Include.Add(x => x.MaterialList);
            return _userRepository.Find(userSpecification);
        }

        public PagedList<UserSkill> GetSkills(int id, int pageNumber, int pageSize)
        {
            var userSpecification = new Specification<User>(x => x.Id == id);
            userSpecification.Include.Add(x => x.SkillList);
            var user = _userRepository.Find(userSpecification);
            int count = 0;
            var skillList = new List<UserSkill>();
            if (user.SkillList != null)
            {
                count = user.SkillList.Count;
                skillList = user.SkillList.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            }
            return new PagedList<UserSkill>(pageNumber, pageSize, count, skillList);
        }

        public bool LearnMaterial(int id, Material material)
        {
            if (_userRepository.Exist(id))
            {
                var userSpecification = new Specification<User>(x => x.Id == id);
                userSpecification.Include.Add(x => x.MaterialList);
                var user = _userRepository.Find(userSpecification);
                if (user.MaterialList == null)
                {
                    user.MaterialList = new List<LearnedMaterial>();
                }
                if (!user.MaterialList.Any(x => x.MaterialId == material.Id))
                {
                    var temp = new LearnedMaterial(material);
                    temp.UserId = id;
                    temp.MaterialId = material.Id;
                    user.MaterialList.Add(temp);
                    if (_userRepository.Update(user))
                    {
                        return FindProgressOfCourses(id);
                    }
                }
            }
            return false;
        }

        public bool StartLearnCourse(int id, Course course)
        {
            if (_userRepository.Exist(id))
            {
                var userSpecification = new Specification<User>(x => x.Id == id);
                userSpecification.Include.Add(x => x.CourseList);
                var user = _userRepository.Find(userSpecification);
                if (user.CourseList == null)
                {
                    user.CourseList = new List<UserCourse>();
                }
                var temp = new UserCourse(course);
                temp.Course = null;
                temp.UserId = id;
                temp.CourseId = course.Id;
                user.CourseList.Add(temp);
                if (_userRepository.Update(user))
                {
                    return FindProgressOfCourses(id);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private bool FindProgressOfCourses(int id)
        {
            var userSpecification = new Specification<User>(x => x.Id == id);
            userSpecification.Include.Add(x => x.CourseList);
            userSpecification.Include.Add(x => x.SkillList);
            userSpecification.Include.Add(x => x.MaterialList);
            var user = _userRepository.Find(userSpecification); 
            if (user.MaterialList == null)
            {
                user.MaterialList = new List<LearnedMaterial>();
            }
            if (user.SkillList == null)
            {
                user.SkillList = new List<UserSkill>();
            }
            foreach (var userCourse in user.CourseList)
            {
                if (!userCourse.IsComplete)
                {
                    var courseSpecification = new Specification<Course>(x => x.Id == userCourse.CourseId);
                    courseSpecification.Include.Add(x => x.MaterialList);
                    courseSpecification.Include.Add(x => x.GivenSkillList);
                    var course = _courseRepository.Find(courseSpecification);
                    userCourse.Progress = 0;
                    
                    foreach (var material in course.MaterialList)
                    {
                        if (user.MaterialList.Any(x => x.MaterialId == material.MaterialId))
                        {
                            userCourse.Progress++;
                        }
                    }

                    if (userCourse.Progress == course.MaterialList.Count())
                    {
                        userCourse.IsComplete = true;
                        foreach (var skill in course.GivenSkillList)
                        {
                            int index = user.SkillList.FindIndex(x => x.SkillId == skill.SkillId);
                            if (index != -1)
                            {
                                user.SkillList[index].Level++;
                            }
                            else
                            {
                                var temp = new UserSkill { SkillId = skill.SkillId, Level = 1 };
                                temp.UserId = id;
                                user.SkillList.Add(temp);
                            }
                        }
                    }
                }
            }
            var result = _userRepository.Update(user);
            return result;
        }

        private bool IsAvailable(List<UserSkill> current, List<RequirenmentSkill> need)
        {
            foreach (var requirenment in need)
            {
                int index = current.FindIndex(x => x.Skill.Id == requirenment.Skill.Id);
                if (!(index != -1 && current[index].Level >= requirenment.Level))
                    return false;
            }
            return true;
        }
    }
}
