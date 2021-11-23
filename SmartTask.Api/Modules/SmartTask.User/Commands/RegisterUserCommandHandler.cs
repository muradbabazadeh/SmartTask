using MediatR;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Domain.Exceptions;
using SmartTask.Identity.Queries;
using SmartTask.Infrastructure.Commands;
using SmartTask.Infrastructure.Idempotency;
using SmartTask.SharedKernel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SmartTask.Infrastructure.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SmartTask.User.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserQueries _userQueries;
        private readonly SmartTaskDbContext _context;

        public RegisterUserCommandHandler(IUserRepository userRepository, IUserQueries userQueries, SmartTaskDbContext context)
        {
            _userRepository = userRepository;
            _userQueries = userQueries;
            _context = context;
        }

        public async Task<bool> Handle(RegisterUserCommand request, 
            CancellationToken cancellationToken)
        {
            var email = request.Email.ToLower();
            var existingUser = await _userQueries.FindByEmailAsync(email);

            if (existingUser != null)
            {
                throw new DomainException($"User name '{request.Email}' already taken, please choose another name.");
            }

            var user = new Domain.AggregatesModel.UserAggregate.User(request.Email,PasswordHasher.HashPassword(request.Password), false, request.FirstName, request.LastName);

            user.AddToRole(request.RoleId);

            var rolePermissions = await _context.RolePermissons.Where(p => p.RoleId == request.RoleId).ToListAsync();

            foreach (var rolePermission in rolePermissions)
            {
                user.AddPermission(rolePermission.PermissionId.Value);
            }

            await _userRepository.AddAsync(user);
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
            return true;
        }
    }

    public class RegisterUserIdentifiedCommandHandler :
        IdentifiedCommandHandler<RegisterUserCommand, bool>
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
