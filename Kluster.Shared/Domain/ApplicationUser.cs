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

        [MaxLength(200)] public required string Address { get; set; }
        // ensure phone number and email are never null.

        /// <summary>
        /// Can be either Admin or User.
        ///  todo: refactor and update database to use Role not UserType
        /// </summary>
        [MaxLength(10)]
        public required string Role { get; set; }
    }
}