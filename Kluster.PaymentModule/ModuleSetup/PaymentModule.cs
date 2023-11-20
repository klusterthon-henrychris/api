namespace Kluster.PaymentModule.ModuleSetup
{
    public static class PaymentModule
    {
        public static void AddPaymentModule(this IServiceCollection services)
        {
            services.AddCore();
        }

        public static void UsePaymentModule(this WebApplication app)
        {
        }
    }
}
