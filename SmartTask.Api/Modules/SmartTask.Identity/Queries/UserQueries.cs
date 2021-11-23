using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartTask.Domain.AggregatesModel.RoleAggregate;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Identity.ViewModels;
using SmartTask.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using SmartTask.Infrastructure.Constants;
using SmartTask.Identity.ViewModels.FilterModel;

namespace SmartTask.Identity.Queries
{
    public class UserQueries : IUserQueries
    {
        private readonly SmartTaskDbContext _context;
        private readonly IMapper _mapper;

        public UserQueries(SmartTaskDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<User> FindAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetUserWithPermissionsAsync(int userId)
        {
            return await _context.Users
           //.Include(u => u.Roles)
           //     .ThenInclude(r => r.Role)
           //         .ThenInclude(r => r.Permissions)
           //             .ThenInclude(p => p.Permission)
           //                 .ThenInclude(p => p.Parameters)
           // .Include(u => u.Roles)
           //     .ThenInclude(r => r.Role)
           //         .ThenInclude(r => r.Permissions)
           //             .ThenInclude(p => p.ParameterValues)
            .Include(u => u.Permissions)
                .ThenInclude(p => p.ParameterValues)
            .Include(u => u.Permissions)
                .ThenInclude(p => p.Permission)
                    .ThenInclude(p => p.Parameters)
                                .Where(u => u.Id == userId && u.IsDeleted == false)
                                .AsNoTracking()
                                .SingleOrDefaultAsync();
        }

        public async Task<IDictionary<string, PermissionDTO>> GetPermissionsAsync(int userId)
        {
            var user = await GetUserWithPermissionsAsync(userId);

            return GetPermissions(user);
        }

        public async Task<User> FindByNameAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted == false);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted == false);
        }

        public async Task<UserProfileDTO> GetUserProfileAsync(int userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId && u.IsDeleted == false).Include(p=>p.Roles)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if (user == null) return null;
            var profile = _mapper.Map<UserProfileDTO>(user);
            profile.Permissions = await GetPermissionsAsync(user.Id);
            return profile;
        }

        private IDictionary<string, PermissionDTO> GetPermissions(User user)
        {
            if (user == null) return null;

            var permissions = user.Permissions;

            var permissionDTOs = _mapper.Map<List<PermissionDTO>>(permissions);
            var rolePermissions = user.Roles.SelectMany(r => r.Role.Permissions.Where(rp => !permissions.Select(p => p.Permission.Id).Contains(rp.Permission.Id)));
            permissionDTOs.AddRange(_mapper.Map<List<PermissionDTO>>(rolePermissions));

            var distinctPermissions = permissionDTOs.GroupBy(p => p.Name).Select(p => p.First());
            return distinctPermissions.ToDictionary(k => k.Name);
        }

        public async Task<UserAllProfileDto> GetAllAsync(LoadMoreDTO loadMore)
        {
            var users = _context.Users.Include(p=>p.Permissions).ThenInclude(p=>p.ParameterValues).Include(p => p.Roles).ThenInclude(p=>p.Role).AsNoTracking().Where(u=>u.IsDeleted == false);
            var count = (await _context.Users.Where(p=>p.IsDeleted == false).CountAsync());
            if (loadMore.SortField != null)
            {
                if (loadMore.OrderBy)
                {
                    users = users.OrderBy($"p=>p.{loadMore.SortField}");
                }
                else
                {
                    users = users.OrderBy($"p=>p.{loadMore.SortField} descending");
                }
            }
            if (loadMore.Skip != null && loadMore.Take != null)
            {
                users = users.Skip(loadMore.Skip.Value).Take(loadMore.Take.Value);
            }


            var outputModel = new UserAllProfileDto();

            var usersd = await users.ToListAsync();
            outputModel.Data = _mapper.Map<IEnumerable<UserProfileDTO>>(await users.ToListAsync());
            foreach (var item in outputModel.Data)
            {
                item.Permissions = await GetPermissionsAsync(item.Id);
            }
            outputModel.TotalCount = count;

            return outputModel;
        }


        public async Task<User> GetUserEntityAsync(int? userId)
        {
            var user = await _context.Users.Include(p => p.Permissions).ThenInclude(p => p.ParameterValues).Include(p => p.Roles).ThenInclude(p => p.Role)
               .Where(u => u.Id == userId)
               .AsNoTracking()
               .SingleOrDefaultAsync();

            if (user == null) return null;

            return user;
        }

        public async Task<string> GetExistingUser(string email)
        {
            var user = await _context.Users.Include(p => p.Permissions).ThenInclude(p => p.ParameterValues).Include(p => p.Roles).ThenInclude(p => p.Role).FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                return email;
            }

            return "";
        }

        public async Task<Role> GetRoleAsyncById(int? id)
        {
            var role = await _context.Roles.Include(p=>p.Permissions).ThenInclude(p=>p.ParameterValues).FirstOrDefaultAsync(p => p.Id == id);

            if (role == null) return null;

            return role;
        }

        public async Task<IEnumerable<PermissionAllDTO>> GetAllPermission()
        {
            var permissions = await _context.Permissions.Include(p => p.Parameters).OrderByDescending(p=>p.Parameters.Count).ToListAsync();

            return _mapper.Map<IEnumerable<PermissionAllDTO>>(permissions);
        }

        public async Task<IEnumerable<RoleAllDTO>> GetAllRole()
        {
            var permissions = await _context.Roles.Include(p => p.Permissions).ThenInclude(p=>p.Permission).Include(p => p.Permissions).ThenInclude(p=>p.ParameterValues).ToListAsync();

            return _mapper.Map<IEnumerable<RoleAllDTO>>(permissions);
        }

        public async Task<IEnumerable<PermissionByUserIdDTO>> GetPermissionByUserIdAsync(int userId)
        {
            var permissions = await _context.UserPermisson.Include(p=>p.Permission).Where(p => p.UserId == userId).ToListAsync();

            return _mapper.Map<IEnumerable<PermissionByUserIdDTO>>(permissions);
        }

        public async Task<UserAllProfileDto> FilterAsync(LoadMoreDTO loadMore, UserFilterDto userFilter)
        {
            var filteredUser = _context.Users.Include(p => p.Permissions).ThenInclude(p => p.ParameterValues).Include(p => p.Roles).ThenInclude(p => p.Role).Where(p => p.IsDeleted == false && (p.FirstName.Contains(userFilter.FirstName) || userFilter.FirstName == null)
             && (p.LastName.Contains(userFilter.LastName) || userFilter.LastName == null)
             && (p.Email.Contains(userFilter.Email) || userFilter.Email == null)
             && (p.Roles.Any(p=>p.RoleId == userFilter.RoleId)|| userFilter.RoleId == null)).AsQueryable();

            var count = (await filteredUser.CountAsync());

            if (loadMore.SortField != null)
            {
                if (loadMore.OrderBy)
                {
                    filteredUser = filteredUser.OrderBy($"p=>p.{loadMore.SortField}");
                }
                else
                {
                    filteredUser = filteredUser.OrderBy($"p=>p.{loadMore.SortField} descending");
                }
            }

            if (loadMore.Skip != null && loadMore.Take != null)
            {
                filteredUser = filteredUser.Skip(loadMore.Skip.Value).Take(loadMore.Take.Value);
            }

            var outputModel = new UserAllProfileDto();

            outputModel.Data = _mapper.Map<IEnumerable<UserProfileDTO>>(await filteredUser.ToListAsync());
            outputModel.TotalCount = count;

            return outputModel;
        }

        public async Task<UserProfileDTO> GetUserByIdAsync(int userId)
        {

            var user = await _context.Users
                .Where(u => u.Id == userId && u.IsDeleted == false).Include(p => p.Permissions).ThenInclude(p => p.ParameterValues).Include(p => p.Roles).ThenInclude(p => p.Role)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if (user == null) return null;
            var profile = _mapper.Map<UserProfileDTO>(user);
            profile.Permissions = await GetPermissionsAsync(user.Id);
            return profile;
        }

        public async Task<RoleAllDTO> GetByIdRole(int id)
        {
            var role = await _context.Roles.Include(p => p.Permissions).ThenInclude(p => p.Permission).Include(p => p.Permissions).ThenInclude(p => p.ParameterValues).FirstOrDefaultAsync(p => p.Id == id);

            return _mapper.Map<RoleAllDTO>(role);
        }
    }
}
