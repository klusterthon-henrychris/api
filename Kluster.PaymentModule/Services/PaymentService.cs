using Kluster.PaymentModule.Data;
using Kluster.PaymentModule.Services.Contracts;
using Kluster.Shared.Messaging.Commands.Payment;
using Microsoft.EntityFrameworkCore;

namespace Kluster.PaymentModule.Services;

public class PaymentService(PaymentModuleDbContext context) : IPaymentService
{
    public async Task DeleteAllPaymentsLinkedToBusiness(DeletePaymentsForBusiness command)
    {
        var payments = await context.Payments
            .Where(x => x.BusinessId == command.BusinessId)
            .ToListAsync();
        
        context.RemoveRange(payments);
        await context.SaveChangesAsync();
    }
    
    public async Task DeleteAllPaymentsLinkedToClient(DeletePaymentsForClient command)
    {
        var payments = await context.Payments
            .Where(x => x.ClientId == command.ClientId)
            .ToListAsync();
        
        context.RemoveRange(payments);
        await context.SaveChangesAsync();
    }
    
    public async Task DeleteAllPaymentsLinkedToInvoice(DeletePaymentsForInvoice command)
    {
        var payments = await context.Payments
            .Where(x => x.InvoiceId == command.InvoiceId)
            .ToListAsync();
        
        context.RemoveRange(payments);
        await context.SaveChangesAsync();
    }
}