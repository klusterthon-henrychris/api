using System.ComponentModel.DataAnnotations;

namespace Kluster.Shared.Domain;

public class Client
{
    [MaxLength(DomainConstants.MaxIdLength)]
    public string Id { get; set; } = "C-" + Guid.NewGuid();

    [MaxLength(DomainConstants.MaxNameLength)]
    public required string FirstName { get; set; }

    [MaxLength(DomainConstants.MaxNameLength)]
    public required string LastName { get; set; }

    [MaxLength(DomainConstants.MaxNameLength)]
    public string? BusinessName { get; set; }

    [MaxLength(DomainConstants.MaxAddressLength)] public required string Address { get; set; }
    [MaxLength(DomainConstants.MaxEmailAddressLength)] public required string EmailAddress { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [MaxLength(DomainConstants.MaxIdLength)] public required string BusinessId { get; set; }
    public Business Business { get; set; } = null!;
}