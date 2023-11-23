using System.Text;
using Kluster.BusinessModule.ModuleSetup;
using Kluster.Messaging.ModuleSetup;
using Kluster.PaymentModule.ModuleSetup;
using Kluster.Shared.Configuration;
using Kluster.Shared.Constants;
using Kluster.Shared.Filters;
using Kluster.UserModule.ModuleSetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Kluster.Host
{
    public static class ServiceInitializer
    {
        public static void RegisterApplicationServices(this IServiceCollection services,
            IWebHostEnvironment environment)
        {
            BindConfigFiles(services, environment);
            RegisterModules(services);
            SetupControllers(services);
            RegisterSwagger(services);
            RegisterFilters(services);
            SetupAuthentication(services, environment);
            SetupCors(services, environment);
        }

        private static void SetupControllers(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
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
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
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
                var configuration = new ConfigurationBuilder()
                    .AddUserSecrets<Program>()
                    .Build();

                // configure app wide
                services.Configure<JwtSettings>(options =>
                    configuration.GetSection(nameof(JwtSettings)).Bind(options));

                jwtSettings = services.BuildServiceProvider().GetService<IOptionsSnapshot<JwtSettings>>()?.Value;
            }

            if (environment.IsProduction())
            {
                // todo: get the jwt settings from azure key vault.
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

        // bind config files here and use them through all modules.
        private static void BindConfigFiles(this IServiceCollection services, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var configuration = scope.ServiceProvider.GetService<IConfiguration>();

                services.Configure<DatabaseSettings>(options =>
                    configuration?.GetSection(nameof(DatabaseSettings)).Bind(options));

                services.Configure<RabbitMqSettings>(options =>
                    configuration?.GetSection(nameof(RabbitMqSettings)).Bind(options));
            }

            // todo: if not development, use key vault for appSettings.
        }

        private static void SetupCors(this IServiceCollection services, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("AllowAnyOrigin",
                        builder =>
                        {
                            builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        });
                });
            }
        }

        private static void RegisterModules(IServiceCollection services)
        {
            services.AddUserModule();
            services.AddBusinessModule();
            services.AddPaymentModule();
            services.AddMessagingModule();
        }
    }
}