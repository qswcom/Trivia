using System;
using System.Collections.Generic;
using System.Reflection;
using Com.Qsw.Module.User.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Com.Qsw.TriviaServer.AppServer.Main
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region HttpClientFactory

            services.AddHttpClient();

            #endregion

            #region Compression

            services.AddResponseCompression();

            #endregion

            #region Authentication

            // Add Authentication later

            #endregion

            #region Authorization

            // Add Authorization later

            #endregion

            #region Hub
            
            services.AddSignalR(o => { o.EnableDetailedErrors = true; });

            #endregion

            #region

            IMvcBuilder mvcBuilder = services.AddControllers(options => { options.Filters.Add<ContextFilter>(); });
            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Latest);

            mvcBuilder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            #endregion

            #region MVC

            IList<string> controllerAssemblyNames = new List<string>
                {};

            foreach (string controllerAssemblyName in controllerAssemblyNames)
            {
                Assembly assembly = Assembly.Load(controllerAssemblyName);
                mvcBuilder.AddApplicationPart(assembly);
            }

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCompression();

            #region Authentication

            // Add later

            #endregion

            #region Authorization

            // Add later

            #endregion

            #region Hub

            #endregion
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                try
                {
                    endpoints.MapHub<NotificationHub>("/Hub/NotificationHub");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error on set hub configure middleware, message: {e}.");
                }

                endpoints.MapControllers();
            });
        }
    }
}