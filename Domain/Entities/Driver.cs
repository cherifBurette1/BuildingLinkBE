using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Driver
    {
        [Required]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
