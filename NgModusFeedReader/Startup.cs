using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ModusCreateSampleApp.Data;
using ModusCreateSampleApp.Data.Entities;
using NgModusFeedReader.Data.Services;

namespace NgModusFeedReader
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
            services.AddIdentity<User, IdentityRole>(identityOptions =>
            {
                //this will also be our username, so make it unique
                identityOptions.User.RequireUniqueEmail = true;
            })
                    //not using a seperate context/data store, because it's a simple project
                    .AddEntityFrameworkStores<AppDatabaseContext>();

            services.AddAuthentication()
                    .AddCookie()
                    .AddJwtBearer(cfg =>
                    {
                        cfg.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidIssuer = Configuration["Security:JWT:Tokens:Issuer"],
                            ValidAudience = Configuration["Security:JWT:Tokens:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Security:JWT:Tokens:Key"])),
                        };
                    });

            //add our sqlite db
            services.AddDbContext<AppDatabaseContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("AppDatabaseConnectionString"));
            });

            //call up our seeding utility, and have it work as needed
            services.AddTransient<DBSeeder>();

            //create our repository class, and scope it to the lifetime of a request
            services.AddScoped<IAppDbRepository, AppDbRepository>();

            //add the service to manage subscriptions
            services.AddScoped<IFeedManagementService, FeedManagementService>();

            services.AddControllersWithViews();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            //add authentication to the pipeline
            app.UseAuthentication();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
