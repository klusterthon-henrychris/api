using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Kluster.BusinessModule.ModuleSetup;
using Kluster.Messaging.ModuleSetup;
using Kluster.NotificationModule.ModuleSetup;
using Kluster.PaymentModule.ModuleSetup;
using Kluster.Shared.Configuration;
using Kluster.Shared.Constants;
using Kluster.Shared.Filters;
using Kluster.UserModule.ModuleSetup;
using Kluster.UserModule.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace Kluster.Host
{
    public static class ServiceInitializer
    {
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            using var scope = builder.Services.BuildServiceProvider().CreateScope();
            var configuration = scope.ServiceProvider.GetService<IConfiguration>();
            var seqSettings = configuration?.GetSection(nameof(SeqSettings)).Get<SeqSettings>();

            builder.Host.UseSerilog((_, lc) => lc
                .WriteTo.Console(new JsonFormatter())
                .WriteTo.Seq(seqSettings?.BaseUrl ?? "http://localhost:5341")
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
            );
        }

        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            BindConfigFiles(services);
            RegisterModules(services);
            SetupControllers(services);
            RegisterSwagger(services);
            RegisterFilters(services);
            SetupAuthentication(services);
            SetupCors(services);
        }

        private static void SetupControllers(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            services.AddHealthChecks().AddCheck<CustomHealthCheck>("custom-health-check");
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

        private static void SetupAuthentication(IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var jwtSettings = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<JwtSettings>>().Value;

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
                            ValidAudience = jwtSettings.Audience ??
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

        private static void BindConfigFiles(this IServiceCollection services)
        {
            var baseConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Program>()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            Console.WriteLine("Trying to fetch secrets configuration from key vault.");
            var secretsConfiguration = GetSecretsConfigurationAsync(baseConfiguration).GetAwaiter().GetResult();
            Console.WriteLine("Fetched secrets configuration from key vault.");

            ConfigureSettings<DatabaseSettings>(services, secretsConfiguration);
            ConfigureSettings<RabbitMqSettings>(services, secretsConfiguration);
            ConfigureSettings<MailSettings>(services, secretsConfiguration);
            ConfigureSettings<PaystackSettings>(services, secretsConfiguration);
            ConfigureSettings<JwtSettings>(services, secretsConfiguration);
            ConfigureSettings<KeyVault>(services, secretsConfiguration);
            Console.WriteLine("Secrets have been bound to classes from key vault.");
        }

        private static async Task<IConfiguration> GetSecretsConfigurationAsync(IConfiguration baseConfiguration)
        {
            var keyVaultName = baseConfiguration["KeyVault:Vault"];
            var kvUri = "https://" + keyVaultName + ".vault.azure.net";
            var client = new SecretClient(new Uri(kvUri),
                new ClientSecretCredential(baseConfiguration["KeyVault:AZURE_TENANT_ID"],
                    baseConfiguration["KeyVault:AZURE_CLIENT_ID"],
                    baseConfiguration["KeyVault:AZURE_CLIENT_SECRET"]));
            Console.WriteLine($"Created KeyVault Uri {kvUri}.");

            var secretsManager = new KeyVaultPrefixManager("KlusterApi");
            var secrets = await secretsManager.GetAllSecretsWithPrefixAsync(client);
            if (secrets is null)
            {
                throw new InvalidOperationException("SOMETHING WENT WRONG. SECRETS WEREN'T RETRIEVED CORRECTLY.");
            }

            return new ConfigurationBuilder()
                .AddConfiguration(baseConfiguration)
                .AddInMemoryCollection(secrets!)
                .Build();
        }

        private static void ConfigureSettings<T>(IServiceCollection services, IConfiguration? configuration)
            where T : class, new()
        {
            services.Configure<T>(options => configuration?.GetSection(typeof(T).Name).Bind(options));
        }

        private static void SetupCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSimpleDev",
                    builder =>
                    {
                        builder.AllowAnyMethod().WithOrigins("https://simple-biz.fly.dev").AllowAnyHeader();
                    });

                options.AddPolicy("AllowAnyOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
        }

        private static void RegisterModules(IServiceCollection services)
        {
            services.AddUserModule();
            services.AddBusinessModule();
            services.AddPaymentModule();
            services.AddMessagingModule();
            services.AddNotificationModule();
        }
    }
}