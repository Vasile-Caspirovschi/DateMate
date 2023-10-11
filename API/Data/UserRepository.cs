using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static AutoMapper.QueryableExtensions.Extensions;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UserRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            var user = await _dataContext.Users
              .Where(x => x.Username == username)
              .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
              .SingleOrDefaultAsync();
            return user!;
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var getUsersQuery = _dataContext.Users.AsQueryable();
            getUsersQuery = getUsersQuery.Where(user => user.Username != userParams.CurrentUsername);
            getUsersQuery = getUsersQuery.Where(user => user.Gender == userParams.Gender);

            var minDob = DateTime.Today.AddYears(-userParams.MaxAge);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
            getUsersQuery = getUsersQuery.Where(user => user.DateOfBirth >= minDob && user.DateOfBirth <= maxDob); 

            return await PagedList<MemberDto>.CreateAsync(getUsersQuery.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(), userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            var user = await _dataContext.Users.FindAsync(id);
            return user!;
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            var user = await _dataContext.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.Username == username);
            return user!;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _dataContext.Users.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _dataContext.Entry(user).State = EntityState.Modified;
        }
    }
}
