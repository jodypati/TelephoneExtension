using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CoreAPI.DataProvider;
using CoreAPI.Helpers;
using CoreAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CoreAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }
        public static string BiofarmaConnectionString { get; private set; }
        public static string SecretCode { get; private set; }
        public static string activeDirectoryDomain { get; private set; }
        public static string activeDirectoryUsername { get; private set; }
        public static string activeDirectoryPassword { get; private set; }
        public static string userNumber { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json").Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddOptions();
            var activeDirectory = Configuration.GetSection("ActiveDirectory");
            services.Configure<ActiveDirectory>(activeDirectory);
            services.AddTransient<ITelephoneExtensionDataProvider, TelephoneExtensionDataProvider>();
            services.AddTransient<IUserDataProvider, UserDataProvider>();
            services.AddMvc();

            var appSettings = Configuration.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserDataProvider>();
                        var telephoneExtensionService = context.HttpContext.RequestServices.GetRequiredService<ITelephoneExtensionDataProvider>();
                        var userId = context.Principal.Identity.Name;
                        userNumber = userId;
                        var user = userService.GetByUserNumber(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseHsts();
            //}

            //app.UseHttpsRedirection();
            //app.UseMvc();
            BiofarmaConnectionString = Configuration["ConnectionStrings:BioFarmaConnection"];
            ActiveDirectory ad = new ActiveDirectory();

            activeDirectoryDomain = Configuration["ActiveDirectory:Domain"];
            activeDirectoryUsername = Configuration["ActiveDirectory:Username"];
            activeDirectoryPassword = Configuration["ActiveDirectory:Password"];
            SecretCode = Configuration["Secret"];
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }


    }
}
