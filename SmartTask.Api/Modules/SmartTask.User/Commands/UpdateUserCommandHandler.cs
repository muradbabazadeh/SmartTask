using MediatR;
using SmartTask.Identity.Queries;
using SmartTask.Infrastructure.Commands;
using SmartTask.Infrastructure.Idempotency;
using SmartTask.SharedKernel.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using System.Linq;
using SmartTask.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;

namespace SmartTask.User.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserQueries _userQueries;
        private readonly SmartTaskDbContext _context;

        public UpdateUserCommandHandler(IUserRepository userRepository, IUserQueries userQueries, SmartTaskDbContext context)
        {
            _userRepository = userRepository;
            _userQueries = userQueries;
            _context = context;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userQueries.GetUserEntityAsync(request.Id);

            if (user.Roles.FirstOrDefault(p => p.RoleId == request.RoleId) == null)
            {
                var olduserPermissions = await _context.UserPermisson.Include(p=>p.ParameterValues).Where(p => p.UserId == user.Id).ToListAsync();

                foreach (var item in olduserPermissions)
                {
                    if (item.ParameterValues.Count != 0)
                    {
                        _context.UserPermissonParametrValues.Remove(await _context.UserPermissonParametrValues.Where(p => p.UserPermissionId == item.Id).FirstOrDefaultAsync());
                    }
           
                }
                await _context.SaveChangesAsync();

                _context.UserPermisson.RemoveRange(olduserPermissions);
                _context.UserRoles.Remove(await _context.UserRoles.FirstOrDefaultAsync(p => p.UserId == user.Id));

                await _context.SaveChangesAsync();

                user.SetDetails(request.Email,request.FirstName, request.LastName);

                var userRole = new UserRole();

                userRole.AddToInfo(user.Id, request.RoleId);

                await _context.UserRoles.AddAsync(userRole);

                var rolePermissions = await _context.RolePermissons.Where(p => p.RoleId == request.RoleId).ToListAsync();

                foreach (var rolePermission in rolePermissions)
                {
                    var userPermission = new UserPermission();
                    userPermission.AddToInfo(user.Id, rolePermission.PermissionId.Value);
                    await _context.UserPermisson.AddAsync(userPermission);
                }

                _userRepository.UpdateAsync(user);
                await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                var userPermissions = await _context.UserPermisson.Include(p => p.ParameterValues).Where(p => p.UserId == user.Id).ToListAsync();

                foreach (var rolePermission in userPermissions)
                {
                    var rolePermissionss = await _context.RolePermissons.Where(p => p.PermissionId == rolePermission.PermissionId && p.RoleId == request.RoleId).FirstOrDefaultAsync();
                    var rolePermissionParametr = await _context.RolePermissonParametrValues
                        .Include(p => p.RolePermission).ThenInclude(p => p.Permission)
                        .Where(p => p.RolePermission.Permission.Id == rolePermission.PermissionId && p.RolePermissionId == rolePermissionss.Id).FirstOrDefaultAsync();

                    if (rolePermissionParametr != null)
                    {
                        var parametrValue = new UserPermissionParameterValue();

                        parametrValue.AddToInfo(rolePermissionParametr.PermissionParameterId, rolePermissionParametr.Value, rolePermission.Id);

                        await _context.UserPermissonParametrValues.AddAsync(parametrValue);
                        await _context.SaveEntitiesAsync();
                    }
                    
                }
            }

            else
            {
                user.SetDetails(request.Email, request.FirstName, request.LastName);

                _userRepository.UpdateAsync(user);
                await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            }

            return true;
        }

        public class RegisterUserIdentifiedCommandHandler : IdentifiedCommandHandler<UpdateUserCommand, bool>
        {
            public RegisterUserIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
            {
            }

            protected override bool CreateResultForDuplicateRequest()
            {
                return true; // Ignore duplicate requests
            }
        }
    }
}
