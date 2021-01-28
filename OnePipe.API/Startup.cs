using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnePipe.API.Filter;
using OnePipe.Core.DatabaseConnection;
using System.Text;
using Microsoft.AspNetCore.Identity;
using OnePipe.Core;
using OnePipe.Core.Entities;
using OpePipe.Data;
using IUsersManagerService = OnePipe.API.Services.IUsersManagerService;
using UsersManagerService = OnePipe.API.Services.UsersManagerService;


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

            services.AddScoped<IOnePipeDatabaseSetting>(sp =>
                sp.GetRequiredService<IOptions<OnePipeDatabaseSetting>>().Value);

           

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            
            var key = Encoding.ASCII.GetBytes("@#bo5lterer2d!4547d7r6");
        
            services.AddIdentityWithMongoStoresUsingCustomTypes
                <Users, UserRole>("mongodb://localhost:27017/OnePipeHrDB").AddDefaultTokenProviders();
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
            services.AddAuthorization(config =>
            {
                config.AddPolicy("AccessAllEmployee", opt =>
                {
                    opt.RequireAuthenticatedUser();
                    opt.AuthenticationSchemes.Add(
                        JwtBearerDefaults.AuthenticationScheme);
                    opt.RequireRole(new List<string> { "ADMINISTRATIVE" , "HR"});

                });

                config.AddPolicy("AccessManagerEmployee", opt =>
                {
                    opt.RequireAuthenticatedUser();
                    opt.AuthenticationSchemes.Add(
                        JwtBearerDefaults.AuthenticationScheme);
                    opt.RequireRole(new List<string> { "Manager" });
                });
            });
           // services.AddScoped<IUsersManagerService, UsersManagerService>();
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "OnePipe.API", Version = "v1" });
            });

         

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnePipe.API v1"));
            //app.UseHttpsRedirection();


            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
