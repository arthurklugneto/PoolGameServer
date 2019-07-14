using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PoolGameServer.Persistence;
using PoolGameServer.Repositories;
using PoolServer.Models;
using System.Text;
using PoolServer.Services;
using PoolServer.Configurations;

namespace PoolGameServer
{
    public class Startup
    {
        private ConfigurationSection _authSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            ConfigureFileConfigurations(services);
            ConfigureAuthentication(services);
            ConfigureCoreServices(services);
            ConfigureEntityRepositories(services);
            ConfigureEntityServices(services);

        }

        private void ConfigureFileConfigurations(IServiceCollection services)
        {
            var appJWTSettings = Configuration.GetSection("JWTSettings");
            var userRegisterInitialValuesSection = Configuration.GetSection("UserRegisterInitialValues");
            var serverSettings = Configuration.GetSection("ServerSettings");

            services.Configure<JWTSettings>(appJWTSettings);
            services.Configure<UserRegisterInitialValues>(userRegisterInitialValuesSection);
            services.Configure<ServerSettings>(serverSettings);
        }

        private void ConfigureAuthentication(IServiceCollection services){
            // configure jwt authentication
            var appSettings = Configuration.GetSection("JWTSettings").Get<JWTSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
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
        }

        private void ConfigureCoreServices(IServiceCollection services){
            services.AddScoped<IMongoContext, MongoContext>();  
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICryptographyService,CryptographyService>();
        }

        private void ConfigureEntityRepositories(IServiceCollection services){
            services.AddScoped<IUserRepository, UserRepository>();
        }

        private void ConfigureEntityServices(IServiceCollection services){
            services.AddScoped<IUserService, UserService>();
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
                //app.UseHsts();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
