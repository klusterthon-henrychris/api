using Kluster.NotificationModule.Models;

namespace Kluster.NotificationModule.Services.Contracts;

public interface IMailService
{
    Task<bool> SendAsync(MailData mailData, CancellationToken ct);

    /// <summary>
    /// Templates are stored in Kluster.Host/Templates.
    /// <remarks>
    /// Passing in "index", would return contents of a file called index.html, if any.
    /// Passing in "test/new", would return new.html, from the folder named "test", inside the Templates folder.
    /// If it doesn't exist, get ready for an exception.
    /// </remarks>
    /// </summary>
    /// <param name="pathToTemplate">This can be the name of the file, or a path</param>
    /// <returns></returns>
    public string LoadTemplate(string pathToTemplate);
}