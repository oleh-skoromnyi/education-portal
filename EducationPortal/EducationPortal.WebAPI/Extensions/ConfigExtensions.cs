using EducationPortal.BLL;
using EducationPortal.Core;
using EducationPortal.Core.Validation;
using EducationPortal.DAL.Repositories;
using EducationPortal.MVC.Models;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPortal.WebAPI.Extensions
{
    public static class ConfigExtensions
    {
        public static void AddViewModelValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<LoginViewModel>, LoginViewModelValidator>();
            services.AddScoped<IValidator<RegistrationViewModel>, RegistrationViewModelValidator>();
            services.AddScoped<IValidator<AdditionViewModel>, AdditionViewModelValidator>();
            services.AddScoped<IValidator<MaterialViewModel>, MaterialViewModelValidation>();
            services.AddScoped<IValidator<CourseViewModel>, CourseViewModelValidator>();
            services.AddScoped<IValidator<UserViewModel>, UserViewModelValidator>();
        }

        public static void AddEntityValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<Course>, CourseValidator>();
            services.AddScoped<IValidator<Material>, MaterialValidator>();
            services.AddScoped<IValidator<RequirenmentSkill>, RequirenmentSkillValidator>();
            services.AddScoped<IValidator<Skill>, SkillValidator>();
            services.AddScoped<IValidator<UserSkill>, UserSkillValidator>();
            services.AddScoped<IValidator<User>, UserValidator>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IRepository<Skill>, Repository<Skill>>();
            services.AddScoped<IRepository<Material>, Repository<Material>>();
            services.AddScoped<IRepository<Course>, Repository<Course>>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISkillService, SkillService>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<ICourseService, CourseService>();
        }
    }
}
