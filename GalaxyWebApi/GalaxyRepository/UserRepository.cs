using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaxyDto;
using GalaxyRepository.Contracts;
using GalaxyRepository.Models;
using Microsoft.EntityFrameworkCore;
using UserDto = GalaxyDto.UserDto;

namespace GalaxyRepository
{
    public class UserRepository : IUserRepository
    {//todo добавить автомаппер
        private readonly GalaxyContext _context;

        public UserRepository(GalaxyContext context)
        {
            _context = context;
        }

        public async Task<UserDto> CreateAsync(UserDto user)
        {
            var utcNow = DateTime.UtcNow;
            await _context.User.AddAsync(new User()
            {
                Username = user.Username,
                Created = utcNow,
                Amount = user.Amount,
                Birthdate = user.Birthdate,
                FirstName = user.FirstName,
                Modified = utcNow
            });
            await _context.SaveChangesAsync(true);

            var userDb = await _context.User.FirstAsync(x => x.Username == user.Username);

            return new UserDto()
            {
                Username = userDb.Username,
                Created = userDb.Created,
                Amount = userDb.Amount,
                Birthdate = userDb.Birthdate,
                FirstName = userDb.FirstName,
                Id = userDb.Id,
                Modified = userDb.Modified
            };
        }

        public Task<UserDto> GetAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateAsync(UserDto user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<UserDto>> GetUsersAsync(int pageNumber, int pageSize)
        {
            throw new System.NotImplementedException();
        }
    }
}