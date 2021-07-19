using EducationPortal.Core;
using EducationPortal.BLL;
using EducationPortal.DAL.Repositories;
using EducationPortal.DAL.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using EducationPortal.Core.Validation;
using FluentValidation;
using EducationPortal.MVC.Models;

namespace EducationPortal.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<DbContext,EducationPortalDbContext>(options=>
                options.UseSqlServer(Configuration.GetConnectionString("EFDB")));

            services.AddScoped<IValidator<LoginViewModel>, LoginValidator>();
            services.AddScoped<IValidator<RegistrationViewModel>, RegistrationValidator>();
            services.AddScoped<IValidator<AdditionViewModel>, AdditionViewModelValidator>();
            services.AddScoped<IValidator<MaterialViewModel>, MaterialModelValidation>();
            services.AddScoped<IValidator<CourseViewModel>, CourseViewModelValidator>();

            services.AddScoped<IValidator<Course>, CourseValidator>();
            services.AddScoped<IValidator<Material>, MaterialValidator>();
            services.AddScoped<IValidator<RequirenmentSkill>, RequirenmentSkillValidator>();
            services.AddScoped<IValidator<Skill>, SkillValidator>();
            services.AddScoped<IValidator<UserSkill>, UserSkillValidator>();
            services.AddScoped<IValidator<User>, UserValidator>();

            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IRepository<Skill>, Repository<Skill>>();
            services.AddScoped<IRepository<Material>, Repository<Material>>();
            services.AddScoped<IRepository<Course>, Repository<Course>>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISkillService, SkillService>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<ICourseService, CourseService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
