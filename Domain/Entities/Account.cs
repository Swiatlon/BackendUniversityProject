using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Username { get; set; }
        [StringLength(50, MinimumLength = 8)]
        public string? Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public Guid? EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }
    }
}
