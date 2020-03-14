using Hera.Common.Core;
using Hera.Common.WebAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
            }).AddJwtBearer(jwtBearerOptions =>
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
            });

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
    }
}
