using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using EducationPortal.Core;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;

namespace EducationPortal.BLL
{
    public class CourseService : ICourseService
    {
        private IRepository<Course> _courseRepository;
        private IRepository<Skill> _skillRepository;
        private IRepository<Material> _materialRepository;
        private IValidator<Course> _courseValidator;
        private IValidator<RequirenmentSkill> _requirenmentSkillValidator;

        public CourseService(IRepository<Course> repos, IRepository<Material> materialRepository,
            IRepository<Skill> skillRepository, IValidator<Course> courseValidator,
            IValidator<RequirenmentSkill> requirenmentSkillValidator)
        {
            this._courseRepository = repos;
            this._materialRepository = materialRepository;
            this._skillRepository = skillRepository;
            this._courseValidator = courseValidator;
            this._requirenmentSkillValidator = requirenmentSkillValidator;
        }

        public async Task<bool> AddCourseAsync(int authorId, Course course, CancellationToken cancellationToken = default)
        {
            if ((await _courseValidator.ValidateAsync(course)).IsValid)
            {
                course.AuthorId = authorId;
                if (await _courseRepository.InsertAsync(course, cancellationToken))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> ChangeCourseAsync(int userId, Course course, CancellationToken cancellationToken = default)
        {
            if ((await _courseValidator.ValidateAsync(course)).IsValid)
            {
                var specification = new FindByIdSpecification<Course>(course.Id);
                var changingCourse = await _courseRepository.FindAsync(specification);
                changingCourse.Name = course.Name;
                changingCourse.Description = course.Description;
                if (await _courseRepository.InsertAsync(changingCourse, cancellationToken))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> AddMaterialToCourseAsync(int userId, int materialId, int courseId, CancellationToken cancellationToken = default)
        {
            var courseSpecification = new FindByIdSpecification<Course>(courseId);
            var materialSpecification = new FindByIdSpecification<Material>(materialId);
            if (await _courseRepository.ExistAsync(courseSpecification, cancellationToken))
            {
                courseSpecification.Include.Add(x => x.MaterialList);
                var course = await _courseRepository.FindAsync(courseSpecification, cancellationToken);
                var material = await _materialRepository.FindAsync(materialSpecification, cancellationToken);
                if (course.AuthorId == userId && !course.IsPublished && !course.MaterialList.Any(x => x.MaterialId == materialId))
                {
                    var temp = new CourseMaterial(material);
                    temp.MaterialId = material.Id;
                    temp.CourseId = courseId;
                    course.MaterialList.Add(temp);
                    if (await _courseRepository.UpdateAsync(course, cancellationToken))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> AddRequirenmentToCourseAsync(int userId, RequirenmentSkill skill, int courseId, CancellationToken cancellationToken = default)
        {
            if ((await _requirenmentSkillValidator.ValidateAsync(skill)).IsValid)
            {
                var specification = new FindByIdSpecification<Course>(courseId);
                if (await _courseRepository.ExistAsync(specification, cancellationToken))
                {
                    specification.Include.Add(x => x.RequirementSkillList);
                    var course = await _courseRepository.FindAsync(specification, cancellationToken);
                    if (course.AuthorId == userId && !course.IsPublished && !course.RequirementSkillList.Any(x => x.SkillId == skill.SkillId))
                    {
                        var temp = skill;
                        temp.SkillId = skill.SkillId;
                        temp.CourseId = courseId;
                        course.RequirementSkillList.Add(temp);
                        if (await _courseRepository.UpdateAsync(course, cancellationToken))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public async Task<bool> AddSkillsToCourseAsync(int userId, int skillId, int courseId, CancellationToken cancellationToken = default)
        {
            var courseSpecification = new FindByIdSpecification<Course>(courseId);
            var skillSpecification = new FindByIdSpecification<Skill>(skillId);
            if (await _courseRepository.ExistAsync(courseSpecification, cancellationToken))
            {
                courseSpecification.Include.Add(x => x.GivenSkillList);
                var courseTask = _courseRepository.FindAsync(courseSpecification, cancellationToken);
                var skillTask = _skillRepository.FindAsync(skillSpecification, cancellationToken);
                var course = await courseTask;
                var skill = await skillTask;
                if (course.AuthorId == userId && !course.IsPublished && !course.GivenSkillList.Any(x => x.SkillId == skillId))
                {
                    var temp = new CourseGivenSkill(skill);
                    temp.SkillId = skill.Id;
                    temp.CourseId = courseId;
                    course.GivenSkillList.Add(temp);
                    if (await _courseRepository.UpdateAsync(course, cancellationToken))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<Course> GetCourseAsync(int courseId, CancellationToken cancellationToken = default)
        {
            var specification = new FindByIdSpecification<Course>(courseId);
            if (await _courseRepository.ExistAsync(specification, cancellationToken))
            {
                specification.Include.Add(x => x.MaterialList);
                specification.Include.Add(x => x.GivenSkillList);
                specification.Include.Add(x => x.RequirementSkillList);
                return await _courseRepository.FindAsync(specification, cancellationToken);
            }
            return null;
        }

        public async Task<PagedList<Course>> GetCoursesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var specification = new Specification<Course>(x => true);
            specification.Include.Add(x => x.MaterialList);
            specification.Include.Add(x => x.GivenSkillList);
            specification.Include.Add(x => x.RequirementSkillList);
            return await _courseRepository.LoadListAsync(specification, pageNumber, pageSize, cancellationToken);
        }

        public async Task<bool> PublishCourseAsync(int userId, int courseId, CancellationToken cancellationToken = default)
        {
            var specification = new FindByIdSpecification<Course>(courseId);
            if (await _courseRepository.ExistAsync(specification, cancellationToken))
            {
                specification.Include.Add(x => x.MaterialList);
                var course = await _courseRepository.FindAsync(specification, cancellationToken);
                if (course.AuthorId == userId && !course.IsPublished && course.MaterialList.Any())
                {
                    course.IsPublished = true;
                    if (await _courseRepository.UpdateAsync(course, cancellationToken))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> RemoveMaterialFromCourseAsync(int userId, int materialId, int courseId, CancellationToken cancellationToken = default)
        {
            var specification = new FindByIdSpecification<Course>(courseId);
            if (await _courseRepository.ExistAsync(specification, cancellationToken))
            {
                specification.Include.Add(x => x.MaterialList);
                var course = await _courseRepository.FindAsync(specification, cancellationToken);
                if (course.AuthorId == userId && !course.IsPublished)
                {
                    course.MaterialList.RemoveAll(x => x.MaterialId == materialId);
                    if (await _courseRepository.UpdateAsync(course, cancellationToken))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> RemoveRequirenmentFromCourseAsync(int userId, int skillId, int courseId, CancellationToken cancellationToken = default)
        {
            var specification = new FindByIdSpecification<Course>(courseId);
            if (await _courseRepository.ExistAsync(specification, cancellationToken))
            {
                specification.Include.Add(x => x.RequirementSkillList);
                var course = await _courseRepository.FindAsync(specification, cancellationToken);
                if (course.AuthorId == userId && !course.IsPublished)
                {
                    course.RequirementSkillList.RemoveAll(x => x.SkillId == skillId);
                    if (await _courseRepository.UpdateAsync(course, cancellationToken))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> RemoveSkillsFromCourseAsync(int userId, int skillId, int courseId, CancellationToken cancellationToken = default)
        {
            var specification = new FindByIdSpecification<Course>(courseId);
            if (await _courseRepository.ExistAsync(specification, cancellationToken))
            {
                specification.Include.Add(x => x.GivenSkillList);
                var course = await _courseRepository.FindAsync(specification, cancellationToken);
                if (course.AuthorId == userId && !course.IsPublished)
                {
                    course.GivenSkillList.RemoveAll(x => x.SkillId == skillId);
                    if (await _courseRepository.UpdateAsync(course, cancellationToken))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
