using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnePipe.API.Filter;
using OnePipe.Core.DatabaseConnection;
using OnePipe.Core.Services;
using OnePipe.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePipe.API
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
            // requires using Microsoft.Extensions.Options
            services.Configure<OnePipeDatabaseSetting>(
                Configuration.GetSection(nameof(OnePipeDatabaseSetting)));

            services.AddSingleton<IOnePipeDatabaseSetting>(sp =>
                sp.GetRequiredService<IOptions<OnePipeDatabaseSetting>>().Value);

            services.AddMvc();
            //services.AddAuthorization(config =>
            //{
            //    config.AddPolicy("UserShouldAccessRecord", opt =>
            //    {
            //        opt.RequireAuthenticatedUser();
            //        opt.AuthenticationSchemes.Add(
            //                JwtBearerDefaults.AuthenticationScheme);
            //        opt.Requirements.Add(new UserShouldAccessRecord());
            //    });

            //    config.AddPolicy("UserShouldUpdateRecord", opt =>
            //    {
            //        opt.RequireAuthenticatedUser();
            //        opt.AuthenticationSchemes.Add(
            //                JwtBearerDefaults.AuthenticationScheme);
            //        opt.Requirements.Add(new UserShouldAccessRecord());
            //    });
            //});
            var key = Encoding.ASCII.GetBytes("123456789");
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnePipe.API", Version = "v1" });
            });

            services.AddIdentityWithMongoStores("mongodb://localhost:27017/OnePipeDB").

            services.AddSingleton<IUsersManagerService, UsersManagerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnePipe.API v1"));
            }

            //app.UseHttpsRedirection();

            app.UseMvc();

            app.UseAuthentication();

            //app.usem(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
