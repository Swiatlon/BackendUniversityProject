using Domain.Enums;

namespace Application.Dtos
{
    public class EmployeeFullDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Pesel { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public AddressOnlyDto? Address { get; set; }
        public AccountOnlyDto? Account { get; set; }
    }
}
