using Kluster.Shared.Middleware;
using Kluster.UserModule.ModuleSetup;
using Serilog;

namespace Kluster.Host
{
    public static class MiddlewareInitializer
    {
        /// <summary>
        /// Configures middleware for the web application.
        /// </summary>
        /// <param name="app">The web application.</param>
        public static void ConfigureApplication(this WebApplication app)
        {
            app.UseSerilogRequestLogging();
            RegisterSwagger(app);
            RegisterMiddleware(app);
            RegisterModules(app);
        }

        /// <summary>
        /// Registers Swagger for the web application.
        /// </summary>
        /// <param name="app">The web application.</param>
        private static void RegisterSwagger(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                return;
            }

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        /// <summary>
        /// Registers middleware for the web application.
        /// </summary>
        /// <param name="app">The web application.</param>
        private static void RegisterMiddleware(WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseCors(app.Environment.IsDevelopment() ? "AllowAnyOrigin" : "AllowSimpleDev");
            app.MapHealthChecks("/health"); // todo: implement IHealthCheck
        }

        private static void RegisterModules(WebApplication app)
        {
            app.UseUserModule();
        }
    }
}