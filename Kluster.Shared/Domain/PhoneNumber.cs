using System.ComponentModel.DataAnnotations;

namespace Kluster.Shared.Domain
{
    public class PhoneNumber
    {
        [MaxLength(3)]
        public required string CountryCode { get; set; }
        [MaxLength(15)]
        public required string Number { get; set; }
    }
}
