using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngApp.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EngApp
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
            services.AddDbContextPool<AppDb>(option => option.UseSqlServer(Configuration.GetConnectionString("db")));
            services.AddIdentity<IdentityUser, IdentityRole>(Action =>
            {
                Action.User.RequireUniqueEmail = false;
                Action.User.AllowedUserNameCharacters = "ؤ ئ ة ا ب ت  ث ج ح خ د ز ر ز س ش ط ظ ع غ ف ق ك ل م ن ه و ي" +
              "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_!$&";
                Action.Password.RequiredLength = 6;
                Action.Password.RequireNonAlphanumeric = false;
                Action.Password.RequireUppercase = false;
                Action.Password.RequireLowercase = false;
                Action.Password.RequireDigit = false;
                Action.Password.RequiredUniqueChars = 0;
            }
               ).AddEntityFrameworkStores<AppDb>();
            services.AddMvc(option =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                option.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsProduction())
            {

            }else
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
