using AcmeStockApp.BLL.Interfaces;
using AcmeStockApp.BLL.Services;
using AcmeStockApp.DAL.Interfaces;
using AcmeStockApp.DAL.Repositories;
using AcmeStockApp.Models;
using AcmeStockApp.Models.DBEntities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recaptcha.Web.Configuration;
using Microsoft.AspNetCore.Http;

namespace AcmeStockApp
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
            //adding reCaptcha
            RecaptchaConfigurationManager.SetConfiguration(Configuration);
            services.AddControllersWithViews();


            //add session service
            services.AddSession(options => 
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);     //FromMinutes(5)
            });

            //dependency injection
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IStockAppUserService, StockAppUserService>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenRepository<>), typeof(GenRepository<>));
            services.AddScoped<IStockAppUserRepository, StockAppUserRepository>();

            //global error hanling 
            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(typeof(Controllers.GlobalErrorController));
            //});
           
            //adding dbcontext .. services
            var provider = services.BuildServiceProvider();
            var config = provider.GetRequiredService<IConfiguration>();
            services.AddDbContext<StockDBContext>(options =>
                options.UseSqlServer(config.GetConnectionString("cs")));

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
            }
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/404";
                    await next();
                }
                else if (context.Response.StatusCode == 401)
                {
                    context.Request.Path = "/Error401";
                    await next();
                }
            });

            app.UseExceptionHandler("/Error500");

            //using session service
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Authentication}/{action=Login}/{id?}");

                endpoints.MapControllerRoute(
                    name: "home",
                    pattern: "Home/{action=Index}/{id?}",
                    defaults: new { controller = "Home" });
            });
        }
    }
}
