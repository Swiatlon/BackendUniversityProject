using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class AddressFullDto
    {
        public Guid Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;
        public string? ApartmentNumber { get; set; }
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public EmployeeOnlyDto? Employee { get; set; }
    }
}
