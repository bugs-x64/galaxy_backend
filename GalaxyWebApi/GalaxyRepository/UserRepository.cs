using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaxyDto;
using GalaxyRepository.Contracts;
using GalaxyRepository.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<UserDto> GetAsync(int id)
        {
            var user = await _context.User.FindAsync(id);

            if(user == null)
                return new UserDto();

            var result = new UserDto()
            {
                Username = user.Username,
                Birthdate = user.Birthdate,
                Created = user.Created,
                FirstName = user.FirstName,
                Amount = user.Amount,
                Modified = user.Modified,
                Id = user.Id
            };

            return result;
        }

        public async Task<UserDto> GetAsync(string username)
        {
            var user = await _context.User.SingleOrDefaultAsync(x => x.Username.Equals(username));

            if (user == null)
                return null;

            var result = new UserDto()
            {
                Username = user.Username,
                Birthdate = user.Birthdate,
                Created = user.Created,
                FirstName = user.FirstName,
                Amount = user.Amount,
                Modified = user.Modified,
                Id = user.Id
            };

            return result;
        }

        public async Task<bool> UpdateAsync(UserDto user)
        {
            var entity = _context.User.FirstOrDefault(item => item.Id == user.Id);

            if (entity == null)
                return false;

            entity.Username = user.Username;
            entity.Birthdate = user.Birthdate;
            entity.Created = user.Created;
            entity.FirstName = user.FirstName;
            entity.Amount = user.Amount;
            entity.Modified = user.Modified;

            _context.User.Update(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
                return false;

            _context.User.Remove(user);

            await _context.SaveChangesAsync();

            return true;
        }

        public Task<IEnumerable<UserDto>> GetUsersAsync(int pageNumber, int pageSize)
        {
            var skipCount = pageNumber * pageSize;
            return Task.FromResult(_context.User
                .Skip(skipCount)
                .Take(pageSize)
                .Select(user => new UserDto
                {
                    Username = user.Username,
                    Birthdate = user.Birthdate,
                    Created = user.Created,
                    FirstName = user.FirstName,
                    Amount = user.Amount,
                    Modified = user.Modified,
                    Id = user.Id
                })
                .AsEnumerable());
        }
    }
}