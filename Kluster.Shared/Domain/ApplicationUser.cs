using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Kluster.Shared.Domain
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(DomainConstants.MaxNameLength)]
        public required string FirstName { get; set; }

        [MaxLength(DomainConstants.MaxNameLength)]
        public required string LastName { get; set; }

        [MaxLength(DomainConstants.MaxAddressLength)] public required string Address { get; set; }
        // ensure phone number and email are never null.

        /// <summary>
        /// Can be either Admin or User.
        /// </summary>
        [MaxLength(DomainConstants.MaxEnumLength)]
        public required string Role { get; set; }
    }
}