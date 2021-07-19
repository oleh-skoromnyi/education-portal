using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EducationPortal.Core
{
    public interface ICourseService
    {
        public Task<bool> AddCourseAsync(int authorId, Course course, CancellationToken cancellationToken = default);
        public Task<bool> ChangeCourseAsync(int userId, Course course, CancellationToken cancellationToken = default);
        public Task<bool> PublishCourseAsync(int userId, int courseId, CancellationToken cancellationToken = default);
        
        public Task<bool> AddSkillsToCourseAsync(int userId, int skillId, int courseId, CancellationToken cancellationToken = default);
        public Task<bool> AddMaterialToCourseAsync(int userId, int materialId,int courseId, CancellationToken cancellationToken = default);
        public Task<bool> AddRequirenmentToCourseAsync(int userId, RequirenmentSkill skill, int courseId, CancellationToken cancellationToken = default);
        
        public Task<bool> RemoveSkillsFromCourseAsync(int userId, int skillId, int courseId, CancellationToken cancellationToken = default);
        public Task<bool> RemoveMaterialFromCourseAsync(int userId, int materialId, int courseId, CancellationToken cancellationToken = default);
        public Task<bool> RemoveRequirenmentFromCourseAsync(int userId, int skillId, int courseId, CancellationToken cancellationToken = default);
        
        public Task<Course> GetCourseAsync(int id, CancellationToken cancellationToken = default);
        public Task<PagedList<Course>> GetCoursesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}
