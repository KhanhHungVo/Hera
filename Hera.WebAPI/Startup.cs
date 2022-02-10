using AutoMapper;
using Hera.Common.Core;
using Hera.Common.Extensions;
using Hera.CryptoService;
using Hera.Data.Infrastructure;
using Hera.Services;
using Hera.Services.AutoMapperMapping;
using Hera.Services.Helper;
using Hera.WebAPI.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddAutoMapper(typeof(Startup));
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidationActionFilter));
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddControllers().AddNewtonsoftJson(
                options => options.SerializerSettings.Converters.Add(new EmptyStringJsonConverter())
             );

            services.AddHeraSecurityAsSingleton();
            services.AddHeraAuthentication(Configuration);
            services.AddHeraSwagger(_env);
            services.AddDbContext<HeraDbContext>(opt =>
            {
                opt.UseNpgsql(Configuration.GetConnectionString(HeraConstants.CONNECTION_STRINGS__POSTGRES_SQL_CONNECTION));

                if (_env.IsDevelopment())
                {
                    opt.EnableSensitiveDataLogging();
                }
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHeraCryptoServices();
            services.AddHeraServices();
            services.AddCors();
             
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(config => config.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                app.UseDeveloperExceptionPage();
                
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseHeraExceptionMiddleware();
            app.UseHeraSwagger(env);
            app.UseAuthentication();
            app.UseAuthorization();
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
