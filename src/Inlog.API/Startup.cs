using AutoMapper;
using Inlog.API.ApiConfiguration;
using Inlog.API.Extensions;
using Inlog.Data.Context;
using KissLog;
using KissLog.Apis.v1.Listeners;
using KissLog.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Text;

namespace Inlog.API
{
    public class Startup
    {

        public IConfiguration Configuration { get; }


        public Startup(IHostingEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            //if (hostEnvironment.IsDevelopment())
            //{
            //    builder.AddUserSecrets<Startup>();
            //}

            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
          
            services.AddDbContext<InlogDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });

          

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.WebApiConfig();

            services.AddSwaggerConfig();

              services.AddLoggingConfiguration(Configuration);

            services.ResolveDependencies();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ILogger>((context) =>
            {
                return Logger.Factory.Get();
            });

            UpdateDatabase();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("Development");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

         
            app.UseKissLogMiddleware(options => {
                ConfigureKissLog(options);
            });

            app.UseLoggingConfiguration();

           app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
        


            app.UseMvcConfiguration();

            app.UseSwaggerConfig(provider);         

       
       
        }

        private void ConfigureKissLog(IOptionsBuilder options)
        {
            // register KissLog.net cloud listener
            options.Listeners.Add(new KissLogApiListener(new KissLog.Apis.v1.Auth.Application(
                Configuration["KissLog:KissLog.OrganizationId"],    //  "b71ee1ce-ab55-4f42-8156-f789fcd3f80e"
                Configuration["KissLog:KissLog.ApplicationId"])     //  "340290ad-3888-4547-b049-245aeb112c8d"
            )
            {
                ApiUrl = Configuration["KissLog:KissLog.ApiUrl"]    //  "https://api.kisslog.net"
            });

            // optional KissLog configuration
            options.Options
                .AppendExceptionDetails((Exception ex) =>
                {
                    StringBuilder sb = new StringBuilder();

                    if (ex is System.NullReferenceException nullRefException)
                    {
                        sb.AppendLine("Important: check for null references");
                    }

                    return sb.ToString();
                });

            // KissLog internal logs
            options.InternalLog = (message) =>
            {
                Debug.WriteLine(message);
            };
        }

        private void UpdateDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<InlogDbContext>();
            var options =
                optionsBuilder
                .UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
                .Options;

            using (var context = new InlogDbContext(options))
            {
                context.Database.Migrate();
            }
        }
    }

   
}
