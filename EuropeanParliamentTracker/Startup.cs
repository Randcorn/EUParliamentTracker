using AutoMapper;
using EuropeanParliamentTracker.Application.Interfaces;
using EuropeanParliamentTracker.Application.Repositories;
using EuropeanParliamentTracker.DataIntegrations.CountriesIntegration;
using EuropeanParliamentTracker.DataIntegrations.ParliamentariansIntegration;
using EuropeanParliamentTracker.DataIntegrations.VotesIntegration;
using EuropeanParliamentTracker.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EuropeanParliamentTracker
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton(GetMapper());

            services.AddTransient<INationalPartiesRepository, NationalPartiesRepository>();
            services.AddTransient<ParliamentariansIntegration>();
            services.AddTransient<CountriesIntegration>();
            services.AddTransient<VotesIntegration>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var connection = @"Server=(localdb)\mssqllocaldb;Database=EuropeanParliamentTracker;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<DatabaseContext>
                (options => options.UseSqlServer(connection, x => x.MigrationsAssembly("EuropeanParliamentTracker.Domain")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public IMapper GetMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {

            });
            return mappingConfig.CreateMapper();
        }
    }
}
