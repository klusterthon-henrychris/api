using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Kluster.Shared.Domain
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(30)]
        public override string Id { get; set; } = "U-" + Guid.NewGuid();
        [MaxLength(50)] public required string FirstName { get; set; } = string.Empty;
        [MaxLength(50)] public required string LastName { get; set; } = string.Empty;
        // ensure phone number and email are never null.
        
        /// <summary>
        /// Can be either client or business.
        /// </summary>
        public required string UserType { get; set; }
    }
}
