using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GalaxyDto;
using GalaxyRepository.Contracts;
using GalaxyRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace GalaxyRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly GalaxyContext _context;
        private readonly IMapper _mapper;

        public UserRepository(GalaxyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private Task<User> GetUserAsync(string username) =>
            _context.User.SingleOrDefaultAsync(user => user.Username.Equals(username));

        public async Task<UserDto> CreateAsync(UserDto user)
        {
            var utcNow = DateTime.UtcNow;
            var entity = _mapper.Map<User>(user);
            entity.Created = utcNow;
            entity.Modified = utcNow;

            await _context.User.AddAsync(entity);

            await _context.SaveChangesAsync(true);

            var userDb = await GetUserAsync(user.Username);

            return _mapper.Map<UserDto>(userDb);
        }

        public async Task<UserDto> GetAsync(string username)
        {
            var user = await GetUserAsync(username);

            return user is null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateAsync(UserDto user)
        {
            var entity = await GetUserAsync(user.Username);

            if (entity == null)
                return false;

            entity.Amount = user.Amount;
            entity.Birthdate = user.Birthdate;
            entity.Modified = DateTime.UtcNow;

            _context.User.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(string username)
        {
            var user = await GetUserAsync(username);
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
                .AsEnumerable()
                .Select(_mapper.Map<UserDto>));
        }
    }
}