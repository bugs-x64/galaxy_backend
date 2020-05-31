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

        public async Task<UserDto> CreateAsync(UserDto user)
        {
            var utcNow = DateTime.UtcNow;
            var entity = _mapper.Map<User>(user);
            entity.Created = utcNow;
            entity.Modified = utcNow;

            await _context.User.AddAsync(entity);

            await _context.SaveChangesAsync(true);

            var userDb = await _context.User.FirstAsync(x => x.Username == user.Username);

            return _mapper.Map<UserDto>(userDb);
        }

        public async Task<UserDto> GetAsync(int id)
        {
            var user = await _context.User.FindAsync(id);

            return user is null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetAsync(string username)
        {
            var user = await _context.User.SingleOrDefaultAsync(x => x.Username.Equals(username));

            return user is null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateAsync(UserDto user)
        {
            var entity = _context.User.FirstOrDefault(item => item.Id == user.Id);

            if (entity == null)
                return false;

            var result = _mapper.Map<User>(user);

            result.Id = entity.Id;

            _context.User.Update(result);

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
                .AsEnumerable()
                .Select(_mapper.Map<UserDto>));
        }
    }
}