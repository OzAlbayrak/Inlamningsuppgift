using Microsoft.AspNetCore.Identity;

namespace Inlamningsuppgift.Models.Entities
{
    public class UserProfileEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string StreetName { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Company { get; set; }
        public string? ImageName { get; set; }
        public virtual IdentityUser User { get; set; } = null!;
    }
}
