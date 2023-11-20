using Kluster.Shared;
using Kluster.Shared.Configuration;
using Kluster.Shared.Filters;
using Kluster.UserModule.ModuleSetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Kluster.Host
{
    public static class ServiceInitializer
    {
        public static void RegisterApplicationServices(this IServiceCollection services, IWebHostEnvironment environment)
        {
            SetupControllers(services);
            RegisterSwagger(services);
            RegisterFilters(services);
            SetupAuthentication(services, environment);
            BindConfigFiles(services, environment);
        }

        private static void SetupControllers(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        private static void RegisterSwagger(IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // setup swagger to accept bearer tokens
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
            });
        }

        private static void RegisterFilters(IServiceCollection services)
        {
            services.AddScoped<CustomValidationFilter>();
        }

        private static void SetupAuthentication(IServiceCollection services, IWebHostEnvironment environment)
        {
            var jwtSettings = new JwtSettings();
            if (environment.IsDevelopment())
            {
                jwtSettings = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build().Get<JwtSettings>();
            }

            if (environment.IsProduction())
            {
                // get the jwt settings from azure key vault.
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(
                x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = jwtSettings?.Audience ??
                            throw new InvalidOperationException("Audience is null!"),
                        ValidIssuer = jwtSettings.Issuer ??
                            throw new InvalidOperationException("Security Key is null!"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey ??
                            throw new InvalidOperationException("Security Key is null!"))),
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RoleClaimType = JwtClaims.Role
                    };
                });
            services.AddAuthorization();
        }

        // bind config files here and use them everywhere else.
        private static void BindConfigFiles(this IServiceCollection services, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                var configuration = new ConfigurationBuilder()
                        .AddJsonFile("database.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables().Build();

                services.Configure<DatabaseSettings>(options => configuration.GetSection("DatabaseSettings").Bind(options));
            }

            // TODO: USE -> var dbSettings = services.BuildServiceProvider().GetService<IOptions<DatabaseSettings>>()?.Value;
            // if not development, use key vault
        }

        private static void RegisterModules(IServiceCollection services)
        {

        }
    }
}
