using Autofac;
using EducationPortal.Core;
using EducationPortal.DAL;
using EducationPortal.BLL;
using System.Collections.Generic;
using EducationPortal.DAL.Repositories;
using EducationPortal.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using EducationPortal.UI.Commands;

namespace EducationPortal.UI
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            var pageSettings = new PageSettings { PageNumber = 1, PageSize = 10 };
            builder.RegisterInstance(pageSettings);

            EFDBConfig(ref builder);

            builder.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();
            builder.RegisterType<CourseService>().As<ICourseService>().InstancePerLifetimeScope();
            builder.RegisterType<SkillService>().As<ISkillService>().InstancePerLifetimeScope();
            builder.RegisterType<MaterialService>().As<IMaterialService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();

            builder.RegisterType<LoginCommand>().As<ICommand>();
            builder.RegisterType<RegisterCommand>().As<ICommand>();

            builder.RegisterType<MaterialMenuCommand>().As<ICommand>();
            builder.RegisterType<SkillMenuCommand>().As<ICommand>();
            builder.RegisterType<CourseMenuCommand>().As<ICommand>();
            builder.RegisterType<UserMenuCommand>().As<ICommand>();

            builder.RegisterType<SkillMenuAddSkillCommand>().As<ICommand>();
            builder.RegisterType<SkillMenuGetSkillsCommand>().As<ICommand>();

            builder.RegisterType<CourseMenuCreateCourseCommand>().As<ICommand>();
            builder.RegisterType<CourseMenuAddAdditionsCommand>().As<ICommand>();
            builder.RegisterType<CourseMenuPublishCourseCommand>().As<ICommand>();
            builder.RegisterType<CourseMenuGetCoursesCommand>().As<ICommand>();

            builder.RegisterType<MaterialMenuAddArticleMaterialCommand>().As<ICommand>();
            builder.RegisterType<MaterialMenuAddBookMaterialCommand>().As<ICommand>();
            builder.RegisterType<MaterialMenuAddVideoMaterialCommand>().As<ICommand>();
            builder.RegisterType<MaterialMenuAddTestMaterialCommand>().As<ICommand>();
            builder.RegisterType<MaterialMenuGetMaterialsCommand>().As<ICommand>();

            builder.RegisterType<UserMenuGetPersonalDataCommand>().As<ICommand>();
            builder.RegisterType<UserMenuChangePersonalDataCommand>().As<ICommand>();
            builder.RegisterType<UserMenuGetUserSkillsCommand>().As<ICommand>();
            builder.RegisterType<UserMenuGetUserCoursesCommand>().As<ICommand>();
            builder.RegisterType<UserMenuStartCourseCommand>().As<ICommand>();
            builder.RegisterType<UserMenuCoursePassingCommand>().As<ICommand>();

            builder.RegisterType<BackCommand>().As<ICommand>();
            builder.RegisterType<LogoutCommand>().As<ICommand>();
            builder.RegisterType<ExitCommand>().As<ICommand>();

            builder.RegisterType<CommandProcessor>();
            builder.RegisterType<CommandManager>();
            return builder.Build();
        }

        private static void FileDbConfig(ref ContainerBuilder builder)
        {
            builder.RegisterType<UserDb>().As<IRepository<User>>().WithParameter("pathToDb", "User.db");
            builder.RegisterType<CourseDb>().As<IRepository<Course>>().WithParameter("pathToDb", "Course.db");
            builder.RegisterType<SkillDb>().As<IRepository<Skill>>().WithParameter("pathToDb", "Skill.db");
            var parameters = new List<NamedParameter>();
            parameters.Add(new NamedParameter("pathToVideoDb", "Material.Video.db"));
            parameters.Add(new NamedParameter("pathToArticleDb", "Material.Article.db"));
            parameters.Add(new NamedParameter("pathToBookDb", "Material.Book.db"));
            parameters.Add(new NamedParameter("pathToTestDb", "Material.Test.db"));
            builder.RegisterType<MaterialDb>().As<IRepository<Material>>().WithParameters(parameters);
        }

        private static void EFDBConfig(ref ContainerBuilder builder)
        {
            var connectionString = @"Server=.\SQLEXPRESS;Database=EducationPortalDb;Trusted_Connection=True;";
            var optionsBuilder = new DbContextOptionsBuilder<EducationPortalDbContext>()
                  .UseSqlServer(connectionString);
            builder.RegisterType<EducationPortalDbContext>().As<DbContext>().WithParameter(
                "options", optionsBuilder.Options);

            builder.RegisterType<UserRepository>().As<IRepository<User>>();
            builder.RegisterType<CourseRepository>().As<IRepository<Course>>();
            builder.RegisterType<SkillRepository>().As<IRepository<Skill>>();
            builder.RegisterType<MaterialRepository>().As<IRepository<Material>>();
        }
    }
}
