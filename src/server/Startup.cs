using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using server.Core;

namespace server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection servicesCollection)
        {
            servicesCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = "https://tim-lab3.eu.auth0.com";
                options.Audience = "https://api.tim-lab3.com";
            });
            servicesCollection.AddAuthorization(options => {
                options.AddPolicy(Policies.RequireAdmin, policy => policy.RequireClaim("http://api.tim-lab3/roles", "admin"));
            });

            servicesCollection.AddCors(options =>
            {
                options.AddPolicy("MyCorsPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader();
                        builder.AllowAnyOrigin().AllowAnyMethod();
                    });
            });

            servicesCollection.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder applicationBulider, IHostingEnvironment EnviromentHosting)
        {
            if (EnviromentHosting.IsDevelopment())
            {
                applicationBulider.UseDeveloperExceptionPage();
            }
            else
            {
                applicationBulider.UseHsts();
            }
            
            applicationBulider.UseCors("MyCorsPolicy");
            applicationBulider.UseAuthentication();
            applicationBulider.UseHttpsRedirection();
            applicationBulider.UseMvc();
        }
    }
}
