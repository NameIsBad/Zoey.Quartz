using Autofac;
using Zoey.Quartz.Web.Core;
using Zoey.Quartz.Web.Filters;
using Zoey.Quartz.Web.Worker;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using ZoeyQuartz.Quartz.Core;

namespace Zoey.Quartz.Web
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
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.LoginPath = new PathString("/Home/Index");
                //  options.LogoutPath = "";
                options.ClaimsIssuer = CookieAuthenticationDefaults.AuthenticationScheme;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            });

            services.AddHttpContextAccessor();

            services.AddControllersWithViews(options =>
            {
                AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add(typeof(TaskAuthorizeFilter));
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }).AddControllersAsServices();
            services.AddSession();
            services.AddSingleton<DefaultWorker>();

            services.AddQuartz();
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ZoeyQuartzQuartzModule());
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
            }

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
