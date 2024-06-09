using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAddressReposiotry
    {
        Task<IEnumerable<Address>> GetAllAsync();
        Task<Address> GetByIdAsync(Guid id);
        Task<Address> CreateAsync(Address address);
        Task<Address> UpdateAsync(Address address);
        Task DeleteAsync(Guid id);
    }
}
