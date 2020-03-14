using Hera.Common.Core;
using Hera.Common.Data;
using Hera.Common.Extensions;
using Hera.Data.Infrastructure;
using Hera.Data.Repositories;
using Hera.Services;
using Hera.Services.Businesses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace Hera.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHeraSecurityAsSingleton();
            services.AddHeraAuthentication(Configuration);
            services.AddEntityFrameworkNpgsql().AddDbContext<HeraDbContext>(opt =>
            {
                opt.UseNpgsql(Configuration.GetConnectionString(HeraConstants.CONNECTION_STRINGS__POSTGRES_SQL_CONNECTION));
            });

            services.AddScoped(typeof(IServiceBaseTypeId<,>), typeof(ServiceBaseTypeId<,>));
            services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITopicCategoriesService, TopicCategoriesService>();

            services.AddTransient(typeof(IRepositoryBaseTypeId<,>), typeof(RepositoryBaseTypeId<,>));
            services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddCors();

            if (_env.IsDevelopment())
            {
                // Register the Swagger generator, defining 1 or more Swagger documents
                services.AddSwaggerGen(s =>
                {
                    s.SwaggerDoc("v1", new OpenApiInfo { Title = "Hera API", Version = "v1" });
                    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter JWT with Bearer into field",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });
                    s.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    });
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(config => config.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                app.UseDeveloperExceptionPage();
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(s =>
                {
                    s.SwaggerEndpoint("../swagger/v1/swagger.json", "Hera API v1");
                    s.DisplayRequestDuration();
                });
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseHeraExceptionMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
