using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EducationPortal.Core;
using FluentValidation;

namespace EducationPortal.BLL
{
    public class UserService : IUserService
    {
        private IRepository<User> _userRepository;
        private IRepository<Course> _courseRepository;
        private IRepository<Material> _materialRepository;
        private IValidator<User> _userValidator;

        public UserService(IRepository<User> repos, IRepository<Course> courseRepos,
            IRepository<Material> materialRepos, IValidator<User> userValidator)
        {
            this._userRepository = repos;
            this._courseRepository = courseRepos;
            this._materialRepository = materialRepos;
            this._userValidator = userValidator;
        }

        public async Task<string> GetUserLoginAsync(int userId, CancellationToken cancellationToken = default)
        {
            var specification = new FindByIdSpecification<User>(userId);
            if (await _userRepository.ExistAsync(specification, cancellationToken))
            {
                return (await _userRepository.FindAsync(specification, cancellationToken)).Login;
            }
            else
            {
                return "Anonim";
            }
        }

        public async Task<bool> ChangePersonalDataAsync(User user, CancellationToken cancellationToken = default)
        {
            if ((await _userValidator.ValidateAsync(user, cancellationToken)).IsValid)
            {
                if (await _userRepository.ExistAsync(new FindByIdSpecification<User>(user.Id), cancellationToken))
                {
                    if (await _userRepository.UpdateAsync(user, cancellationToken))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<PagedList<Course>> GetAvailableCoursesAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var userSpecification = new FindByIdSpecification<User>(userId);
            userSpecification.Include.Add(x => x.SkillList);
            userSpecification.Include.Add(x => x.CourseList);
            var user = await _userRepository.FindAsync(userSpecification, cancellationToken);

            var courseSpecification = new Specification<Course>(x => true);
            courseSpecification.Include.Add(x => x.RequirementSkillList);
            var coursesList = await _courseRepository.LoadListAsync(courseSpecification, pageNumber, pageSize, cancellationToken);
            var items = coursesList.Items.Where(x => !user.CourseList.Any(y => y.CourseId == x.Id) && x.RequirementSkillList
                                            .All(y => user.SkillList
                                            .Any(z => z.SkillId == y.SkillId
                                                    && z.Level >= y.Level)));

            return new PagedList<Course>(coursesList.PageNumber, coursesList.PageSize, coursesList.PageCount, items);
        }

        public async Task<UserCourse> GetCourseAsync(int userId, int courseId, CancellationToken cancellationToken = default)
        {
            var userSpecification = new FindByIdSpecification<User>(userId);
            userSpecification.Include.Add(x => x.CourseList);
            var user = await _userRepository.FindAsync(userSpecification, cancellationToken);
            return user.CourseList.First(x => x.Course.Id == courseId);
        }

        public async Task<PagedList<Course>> GetCreatedNotPublishedCoursesAsync(int userId, int pageNumber,int pageSize, CancellationToken cancellationToken = default)
        {
            var courseSpecification = new Specification<Course>(x=>!x.IsPublished && x.AuthorId == userId);
            var courseList = await _courseRepository.LoadListAsync(courseSpecification, pageNumber, pageSize, cancellationToken);
            return courseList;
        }

        public async Task<PagedList<UserCourse>> GetCoursesAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var userSpecification = new FindByIdSpecification<User>(userId);
            userSpecification.Include.Add(x => x.CourseList);
            var user = await _userRepository.FindAsync(userSpecification, cancellationToken);
            int count = 0;
            var courseList = new List<UserCourse>();
            if (user.CourseList != null)
            {
                count = user.CourseList.Count;
                courseList = user.CourseList.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            }
            return new PagedList<UserCourse>(pageNumber, pageSize, count, courseList);
        }

        public async Task<User> GetPersonalDataAsync(int userId, CancellationToken cancellationToken = default)
        {
            var userSpecification = new FindByIdSpecification<User>(userId);
            userSpecification.Include.Add(x => x.CourseList);
            userSpecification.Include.Add(x => x.SkillList);
            userSpecification.Include.Add(x => x.MaterialList);
            return await _userRepository.FindAsync(userSpecification, cancellationToken);
        }

        public async Task<PagedList<UserSkill>> GetSkillsAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var userSpecification = new FindByIdSpecification<User>(userId);
            userSpecification.Include.Add(x => x.SkillList);
            var user = await _userRepository.FindAsync(userSpecification, cancellationToken);
            int count = 0;
            var skillList = new List<UserSkill>();
            if (user.SkillList != null)
            {
                count = user.SkillList.Count;
                skillList = user.SkillList.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            }
            return new PagedList<UserSkill>(pageNumber, pageSize, count, skillList);
        }

        public async Task<bool> LearnMaterialAsync(int userId, int materialId, CancellationToken cancellationToken = default)
        {
            var userSpecification = new FindByIdSpecification<User>(userId);
            if (await _userRepository.ExistAsync(userSpecification, cancellationToken))
            {
                var materialSpecification = new FindByIdSpecification<Material>(materialId);
                userSpecification.Include.Add(x => x.MaterialList);
                var userTask = _userRepository.FindAsync(userSpecification, cancellationToken);
                var materialTask = _materialRepository.FindAsync(materialSpecification, cancellationToken);
                var user = await userTask;
                var material = await materialTask;
                if (user.MaterialList == null)
                {
                    user.MaterialList = new List<LearnedMaterial>();
                }
                if (!user.MaterialList.Any(x => x.MaterialId == material.Id))
                {
                    var temp = new LearnedMaterial(material);
                    temp.UserId = userId;
                    temp.MaterialId = material.Id;
                    user.MaterialList.Add(temp);
                    if (await _userRepository.UpdateAsync(user, cancellationToken))
                    {
                        return await FindProgressOfCoursesAsync(userId, cancellationToken);
                    }
                }
            }
            return false;
        }

        public async Task<bool> StartLearnCourseAsync(int userId, int courseId, CancellationToken cancellationToken = default)
        {
            var userSpecification = new FindByIdSpecification<User>(userId);
            var courseSpecification = new FindByIdSpecification<Course>(courseId);
            if (await _userRepository.ExistAsync(userSpecification, cancellationToken))
            {
                userSpecification.Include.Add(x => x.CourseList);

                var userTask = _userRepository.FindAsync(userSpecification, cancellationToken);
                var courseTask = _courseRepository.FindAsync(courseSpecification, cancellationToken);

                var user = await userTask;
                var course = await courseTask;

                if (user.CourseList == null)
                {
                    user.CourseList = new List<UserCourse>();
                }

                var temp = new UserCourse(course);
                temp.Course = null;
                temp.UserId = userId;
                temp.CourseId = course.Id;
                user.CourseList.Add(temp);

                if (await _userRepository.UpdateAsync(user, cancellationToken))
                {
                    return await FindProgressOfCoursesAsync(userId, cancellationToken);
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
        private async Task<bool> FindProgressOfCoursesAsync(int userId, CancellationToken cancellationToken = default)
        {
            var userSpecification = new FindByIdSpecification<User>(userId);
            userSpecification.Include.Add(x => x.CourseList);
            userSpecification.Include.Add(x => x.SkillList);
            userSpecification.Include.Add(x => x.MaterialList);
            var user = await _userRepository.FindAsync(userSpecification, cancellationToken);
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
                    var courseSpecification = new FindByIdSpecification<Course>(userCourse.CourseId);
                    courseSpecification.Include.Add(x => x.MaterialList);
                    courseSpecification.Include.Add(x => x.GivenSkillList);
                    var course = await _courseRepository.FindAsync(courseSpecification, cancellationToken);
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
                                temp.UserId = userId;
                                user.SkillList.Add(temp);
                            }
                        }
                    }
                }
            }
            var result = await _userRepository.UpdateAsync(user, cancellationToken);
            return result;
        }
    }
}
