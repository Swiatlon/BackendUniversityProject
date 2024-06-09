using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AddressRepository : IAddressReposiotry
    {
        private readonly CompanyContext _context;

        public AddressRepository(CompanyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await _context.Addresses.ToListAsync();
        }

        public async Task<Address> GetByIdAsync(Guid id)
        {
            var address = await _context.Addresses
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (address is null) throw new KeyNotFoundException("Address not found");
            return address;
        }

        public async Task<Address> CreateAsync(Address address)
        {
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Address> UpdateAsync(Address address)
        {
            _context.Entry(address).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task DeleteAsync(Guid id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address is not null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Address not found");
            }
        }
    }
}
