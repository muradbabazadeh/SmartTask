using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartTask.Domain.AggregatesModel.PermissionAggregate;
using SmartTask.Domain.AggregatesModel.RoleAggregate;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Identity.Auth;
using SmartTask.Identity.Queries;
using SmartTask.Identity.ViewModels;
using SmartTask.Infrastructure.Commands;
using SmartTask.Infrastructure.Database;
using SmartTask.Infrastructure.Idempotency;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartTask.Identity.Commands
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserQueries _userQueries;
        private readonly SmartTaskDbContext _context;
        private readonly IUserManager _userManager;

        public UpdateRoleCommandHandler(IRoleRepository roleRepository, IUserQueries userQueries, SmartTaskDbContext context, IUserManager userManager)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _userQueries = userQueries ?? throw new ArgumentNullException(nameof(userQueries));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {

            //PermissionDTO permissions = await _userManager.GetPermissionAsync(PermissionName.Setting_Add);

            //if (permissions == null)
            //{
            //    throw new UnauthorizedAccessException($"You don't have {PermissionName.Setting_Add} permission");
            //}

            var role = await _userQueries.GetRoleAsyncById(request.Id);

            var oldRolePermissions = await _context.RolePermissons.Where(p => p.RoleId == role.Id).ToListAsync();

            foreach(var rp in oldRolePermissions)
            {
                _context.RolePermissonParametrValues.RemoveRange(await _context.RolePermissonParametrValues.Where(p => p.RolePermissionId == rp.Id).ToListAsync());
            }

            _context.RolePermissons.RemoveRange(oldRolePermissions);

            role.SetDetails(request.Name);

            foreach (var p in request.Permissons)
            {
                role.AddPermission(p.Id);
            }

            _roleRepository.UpdateAsync(role);
            await _roleRepository.UnitOfWork.SaveChangesAsync(cancellationToken);


            var rolePermissions = await _context.RolePermissons.Where(p => p.RoleId == role.Id).ToListAsync();

            foreach (var rolePermission in rolePermissions)
            {
                var permission = request.Permissons.FirstOrDefault(p => p.Id == rolePermission.PermissionId);
                if (permission.ScopeId != null && permission.Value != null)
                {
                    var parametrValue = new RolePermissionParameterValue();

                    parametrValue.AddToInfo(permission.ScopeId.Value, rolePermission.Id, permission.Value);

                    await _context.RolePermissonParametrValues.AddAsync(parametrValue);
                }
            }
            await _context.SaveEntitiesAsync();

            var users = await _context.Users.Include(p=>p.Permissions).ThenInclude(p=>p.ParameterValues).Include(p => p.Roles).Where(p => p.Roles.FirstOrDefault().RoleId == role.Id).ToListAsync();

            foreach (var user in users)
            {
                foreach(var userPermission in user.Permissions)
                {
                    _context.UserPermissonParametrValues.RemoveRange(userPermission.ParameterValues);
                }
                _context.UserPermisson.RemoveRange(user.Permissions);

                foreach (var p in request.Permissons)
                {
                    user.AddPermission(p.Id);
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                foreach (var rolePermission in user.Permissions)
                {
                    var permission = request.Permissons.FirstOrDefault(p => p.Id == rolePermission.PermissionId);
                    if (permission.ScopeId != null && permission.Value != null)
                    {
                        var parametrValue = new UserPermissionParameterValue();

                        parametrValue.AddToInfo(permission.ScopeId.Value, permission.Value, rolePermission.Id);

                        await _context.UserPermissonParametrValues.AddAsync(parametrValue);
                    }
                }


            }

            await _context.SaveChangesAsync();



            return true;
        }

        public class UpdateRoleIdentifiedCommandHandler : IdentifiedCommandHandler<UpdateRoleCommand, bool>
        {
            public UpdateRoleIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
            {
            }

            protected override bool CreateResultForDuplicateRequest()
            {
                return true; // Ignore duplicate requests
            }
        }
    }
}
