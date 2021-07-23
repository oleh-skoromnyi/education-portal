using EducationPortal.BLL;
using EducationPortal.Core;
using EducationPortal.Core.Validation;
using EducationPortal.DAL.Contexts;
using EducationPortal.DAL.Repositories;
using EducationPortal.MVC.Models;
using EducationPortal.WebAPI.Extensions;
using EducationPortal.WebAPI.Filters;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPortal.WebAPI
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
            services.AddControllers();
            services.AddDbContext<DbContext, EducationPortalDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("EFDB")));
            services.AddMvc().AddFluentValidation();
            services.AddSwaggerGen();
            services.AddScoped<AuthentificationFilter>();
            services.AddViewModelValidators();
            services.AddEntityValidators();
            services.AddRepositories();
            services.AddServices();

            services.AddScoped<ITokenService, TokenService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Education portal API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}",
                defaults: "/swagger/");
            });
        }
    }
}
