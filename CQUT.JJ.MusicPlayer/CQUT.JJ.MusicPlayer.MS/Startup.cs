using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Application.Methods;
using CQUT.JJ.MusicPlayer.Core.Managers;
using CQUT.JJ.MusicPlayer.Core.Managers.AuthorizationManager;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using CQUT.JJ.MusicPlayer.MS.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CQUT.JJ.MusicPlayer.MS
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
            //注册数据库上下文
            services.AddDbContext<JMDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("JMDbContext"));
            });

            //用户
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<UserManager, UserManager>();
            //菜单
            services.AddScoped<IMenuAppService, MenuAppService>();
            services.AddScoped<MenuManager, MenuManager>();
            //权限
            services.AddScoped<IPermissionManager, PermissionManager>();
            services.AddScoped<PermissionManager, PermissionManager>();

            services.AddSession();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o => o.LoginPath = new PathString("/Admin/Account/Login"));

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(MvcGlobalHandleErrorAttribute)); // by type
                options.Filters.Add(new MvcGlobalHandleErrorAttribute()); // an instance    
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Admin",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
