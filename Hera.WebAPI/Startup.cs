using Hera.Common.Core;
using Hera.Common.Data;
using Hera.Common.Extensions;
using Hera.Data.Infrastructure;
using Hera.Services;
using Hera.Services.Businesses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hera.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHeraSecurityAsSingleton();
            services.AddHeraAuthentication(Configuration);
            services.AddHeraApiDocument();
            services.AddEntityFrameworkNpgsql().AddDbContext<HeraDbContext>(opt =>
            {
                opt.UseNpgsql(Configuration.GetConnectionString(HeraConstants.CONNECTION_STRINGS__POSTGRES_SQL_CONNECTION));
            });

            services.AddScoped(typeof(IServiceBaseTypeId<,>), typeof(ServiceBaseTypeId<,>));
            services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddTransient(typeof(IRepositoryBaseTypeId<,>), typeof(RepositoryBaseTypeId<,>));
            services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

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
            app.UseHeraCustomApiDocument();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
