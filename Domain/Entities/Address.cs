using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string City { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Street { get; set; }
        [Required]
        [StringLength(10)]
        public string HouseNumber { get; set; }
        [StringLength(10)]
        public string? ApartmentNumber { get; set; }
        public string PostalCode { get; set; }
        [Required]
        public string Country { get; set; }
        public Guid? EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }
    }
}
