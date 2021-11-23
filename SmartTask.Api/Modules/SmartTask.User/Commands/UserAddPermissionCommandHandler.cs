using SmartTask.Domain.AggregatesModel.PermissionAggregate;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Identity.Auth;
using SmartTask.Identity.ViewModels;
using SmartTask.Infrastructure.Commands;
using SmartTask.Infrastructure.Database;
using SmartTask.Infrastructure.Idempotency;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartTask.User.Commands
{
    public class UserAddPermissionCommandHandler : IRequestHandler<UserAddPermissionCommand, int>
    {

        private readonly IUserManager _userManager;
        private readonly SmartTaskDbContext _context;

        public UserAddPermissionCommandHandler(IUserManager userManager, SmartTaskDbContext context)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<int> Handle(UserAddPermissionCommand request, CancellationToken cancellationToken)
        {

            PermissionDTO userpermission = await _userManager.GetPermissionAsync(PermissionName.Employee_Add);

            if (userpermission == null)
            {
                throw new UnauthorizedAccessException($"You don't have {PermissionName.Employee_Add} permission");
            }

            var user = await _context.Users.Include(p => p.Permissions).FirstOrDefaultAsync(p => p.Id == request.UserId);

            foreach (var permission in request.Permissions)
            {
                var userPermission = new UserPermission();

                userPermission.AddToInfo(user.Id, permission.PermissionId);

                await _context.UserPermisson.AddAsync(userPermission);
                await _context.SaveEntitiesAsync();

                var userParametr = new UserPermissionParameterValue();

                userParametr.AddToInfo(permission.ScopeId, permission.Value, userPermission.Id);

                await _context.UserPermissonParametrValues.AddAsync(userParametr);
            }

            return  await _context.SaveChangesAsync();
        }

        public class UserIdentifiedCommandHandler : IdentifiedCommandHandler<UserAddPermissionCommand, int>
        {
            public UserIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
            {
            }

            protected override int CreateResultForDuplicateRequest()
            {
                return 1; // Ignore duplicate requests
            }
        }
    }
}
