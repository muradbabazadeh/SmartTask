using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartTask.Domain.AggregatesModel.PermissionAggregate;
using SmartTask.Domain.AggregatesModel.RoleAggregate;
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
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserQueries _userQueries;
        private readonly SmartTaskDbContext _context;
        private readonly IUserManager _userManager;

        public DeleteRoleCommandHandler(IRoleRepository roleRepository, IUserQueries userQueries, SmartTaskDbContext context, IUserManager userManager)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _userQueries = userQueries ?? throw new ArgumentNullException(nameof(userQueries));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _context.Roles.Include(p=>p.Permissions).ThenInclude(p=>p.Permission).FirstOrDefaultAsync(p => p.Id == request.Id);
            _context.RolePermissons.RemoveRange(role.Permissions);

            _context.UserRoles.RemoveRange(await _context.UserRoles.Where(p => p.RoleId == role.Id).ToListAsync());

            var oldRolePermissions = await _context.RolePermissons.Where(p => p.RoleId == role.Id).ToListAsync();

            foreach (var rp in oldRolePermissions)
            {
                _context.RolePermissonParametrValues.RemoveRange(await _context.RolePermissonParametrValues.Where(p => p.RolePermissionId == rp.Id).ToListAsync());
            }

            _context.RolePermissons.RemoveRange(oldRolePermissions);

            _roleRepository.DeleteAsync(role);
            await _roleRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await _context.SaveChangesAsync();

            return true;
        }

        public class DeleteRoleIdentifiedCommandHandler : IdentifiedCommandHandler<DeleteRoleCommand, bool>
        {
            public DeleteRoleIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
            {
            }

            protected override bool CreateResultForDuplicateRequest()
            {
                return true; // Ignore duplicate requests
            }
        }
    }
}
