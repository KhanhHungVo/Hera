using Hera.Common.Core;
using Hera.Common.Core.Internal;
using Hera.Common.WebAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Common.Extensions
{
    public static class IServicesCollectionExtensions
    {
        public static IServiceCollection AddHeraSecurityAsSingleton(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IHeraSecurity), typeof(HeraSecurity));
            return services;
        }

        public static IServiceCollection AddHeraAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtTokenDescriptorOptions jwtTokenDescriptorOptions = new JwtTokenDescriptorOptions();
            var configSection = configuration.GetSection(HeraConstants.APP_SETTING__JWT_TOKEN_DESCRIPTOR);
            configSection.Bind(jwtTokenDescriptorOptions);
            services.Configure<JwtTokenDescriptorOptions>(configSection);

            services.AddAuthentication(authenticationOptions =>
            {
                authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtBearerOptions =>
            {
                jwtBearerOptions.SaveToken = true;
                jwtBearerOptions.ClaimsIssuer = jwtTokenDescriptorOptions.Issuer;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenDescriptorOptions.OAuthSignatureKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenDescriptorOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtTokenDescriptorOptions.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(HeraConstants.POLICY_BASED_ROLE, policy =>
                {
                    policy.Requirements.Add(new HeraRolesAuthorizationRequirement(HeraConstants.CLAIM_HERA_USER));
                });

                options.AddPolicy(HeraConstants.POLICY_ADMIN_ROLE, policy =>
                {
                    policy.Requirements.Add(new HeraRolesAuthorizationRequirement(HeraConstants.CLAIM_HERA_USER_ADMIN));
                });
            });

            services.AddSingleton<IAuthorizationHandler, HeraRolesAuthorizationRequirementHandler>();

            return services;
        }

        public static void AddHeraSwagger(this IServiceCollection services, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment()) return;

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Hera Api",
                    Version = "v1",
                    Description = "Hera api document",
                    Contact = new OpenApiContact
                    {
                        Name = "Hera Global Group",
                        Email = string.Empty,
                        Url = new Uri("https://www.google.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "License",
                        Url = new Uri("https://www.google.com"),
                    }
                });
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

        public static void AddHeraDatabase(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<DbContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString(HeraConstants.CONNECTION_STRINGS__POSTGRES_SQL_CONNECTION));

                if (env.IsDevelopment())
                {
                    opt.EnableSensitiveDataLogging();
                }
            });
        }
    }
}
