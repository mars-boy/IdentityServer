using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TokenValidation.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace TokenValidation
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
            services.AddDbContext<AgilysysContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:AgilysysDbContext"]));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseWhen(context => context.Request.Query.ContainsKey("token"), appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    bool tokenExists = false;
                    using (var serviceScope = app.ApplicationServices.CreateScope())
                    {
                        var services = serviceScope.ServiceProvider;
                        using (var agContext = services.GetService<AgilysysContext>()) {
                            StringValues token = string.Empty;
                            context.Request.Query.TryGetValue("token",out token);
                            var existingToken = agContext.ClientTokens.Where(x => x.Token == token).FirstOrDefault();
                            if (existingToken != null)
                            {
                                tokenExists = true;
                            }
                            else {
                                ClientToken clientToken = new ClientToken()
                                {
                                    Id = Guid.NewGuid(),
                                    Token = token
                                };
                                agContext.ClientTokens.Add(clientToken);
                                agContext.SaveChanges();
                            }
                        }
                    }
                    if (tokenExists) {
                        var jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new {
                            Message = "Token already Used"
                        }));
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        context.Response.Body.Write(jsonData, 0, jsonData.Length);
                        return;
                    }
                    await next();
                });
            });
            app.UseMvc();
        }
    }
}
