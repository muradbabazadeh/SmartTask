using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartTask.Domain.AggregatesModel.PermissionAggregate;
using SmartTask.Domain.AggregatesModel.RoleAggregate;
using SmartTask.Identity.Auth;
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
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly SmartTaskDbContext _context;
        private readonly IUserManager _userManager;

        public CreateRoleCommandHandler(IRoleRepository roleRepository, SmartTaskDbContext context, IUserManager userManager)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<bool> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {

            //PermissionDTO permission = await _userManager.GetPermissionAsync(PermissionName.Setting_Add);

            //if (permission == null)
            //{
            //    throw new UnauthorizedAccessException($"You don't have {PermissionName.Setting_Add} permission");
            //}


            var role = new Role(request.Name);



            foreach (var p in request.Permissons)
            {
                role.AddPermission(p.Id);
            }



            await _roleRepository.AddAsync(role);
            await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            var rolePermissions = await _context.RolePermissons.Where(p => p.RoleId == role.Id).ToListAsync();

            foreach(var rolePermission in rolePermissions)
            {
                var userPermission = request.Permissons.FirstOrDefault(p => p.Id == rolePermission.PermissionId);
                if(userPermission.ScopeId != null && userPermission.Value != null)
                {
                    var parametrValue = new RolePermissionParameterValue();

                    parametrValue.AddToInfo(userPermission.ScopeId.Value, rolePermission.Id, userPermission.Value);

                    await _context.RolePermissonParametrValues.AddAsync(parametrValue);
                }
               
            }
            await _context.SaveEntitiesAsync();

            return true;
        }

        public class CreateRoleIdentifiedCommandHandler : IdentifiedCommandHandler<CreateRoleCommand, bool>
        {
            public CreateRoleIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
            {
            }

            protected override bool CreateResultForDuplicateRequest()
            {
                return true; // Ignore duplicate requests
            }
        }
    }
}
