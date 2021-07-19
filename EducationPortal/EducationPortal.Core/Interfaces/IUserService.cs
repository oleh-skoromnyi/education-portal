using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EducationPortal.Core
{
    public interface IUserService
    {
        public Task<string> GetUserLoginAsync(int id, CancellationToken cancellationToken = default);
        public Task<bool> StartLearnCourseAsync(int id, int courseId, CancellationToken cancellationToken = default);
        public Task<bool> LearnMaterialAsync(int id, int materialId, CancellationToken cancellationToken = default);
        public Task<PagedList<UserCourse>> GetCoursesAsync(int id, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        public Task<PagedList<Course>> GetAvailableCoursesAsync(int id, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        public Task<UserCourse> GetCourseAsync(int userId, int courseId, CancellationToken cancellationToken = default);
        public Task<bool> ChangePersonalDataAsync(User user, CancellationToken cancellationToken = default);
        public Task<PagedList<UserSkill>> GetSkillsAsync(int id, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        public Task<User> GetPersonalDataAsync(int id, CancellationToken cancellationToken = default);
        public Task<PagedList<Course>> GetCreatedNotPublishedCoursesAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}
