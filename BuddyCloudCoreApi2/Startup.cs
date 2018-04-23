using BuddyCloudCoreApi2.JwtToken.Security;
using BuddyCloudCoreApi2.DAL.DataContext;
using BuddyCloudCoreApi2.DAL.Repository;
using BuddyCloudCoreApi2.Services.Service;
using BuddyCloudCoreApi2.Services.Interfaces;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;

namespace BuddyCloudCoreApi2
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
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetSection("Database:ConnectionString").Value));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = Configuration.GetSection("AppConfiguration:Issuer").Value,
                            ValidAudience = Configuration.GetSection("AppConfiguration:Issuer").Value,
                            IssuerSigningKey = JwtSecurityKey.Create(Configuration.GetSection("JwtSettings:SecurityKey").Value)
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                                return Task.CompletedTask;
                            },
                            OnTokenValidated = context =>
                            {
                                Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                                return Task.CompletedTask;
                            }
                        };
                    });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder => builder.WithOrigins("http://localhost:6761")
                                                                           .AllowAnyMethod()
                                                                           .AllowAnyHeader()
                                                                           .AllowCredentials());
            });

            services.AddMvc();

            services.AddTransient(typeof(IEFRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IInventoryService, InventoryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseCors("AllowSpecificOrigin");
            app.UseMvc();
        }
    }
}