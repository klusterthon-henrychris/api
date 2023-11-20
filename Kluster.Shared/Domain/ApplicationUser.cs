using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Kluster.Shared.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public override string Id { get; set; } = "U-" + Guid.NewGuid().ToString();
        [MaxLength(50)] public required string FirstName { get; set; } = string.Empty;
        [MaxLength(50)] public required string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Can be either client or business.
        /// </summary>
        public required string UserType { get; set; }
    }
}
