using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Surname { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [StringLength(11)]
        [RegularExpression(@"\d{11}")]
        public string Pesel { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public Guid? AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; }
        public Guid? AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }

        public int CalculateAge()
        {
            var age = DateTime.Now.Year - BirthDate.Year;
            if (DateTime.Now.DayOfYear < BirthDate.DayOfYear)
            {
                age = age - 1;
            }
            return age;
        }
    }
}
