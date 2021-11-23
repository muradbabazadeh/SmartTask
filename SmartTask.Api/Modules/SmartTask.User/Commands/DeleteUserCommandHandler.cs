using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using SmartTask.Domain.AggregatesModel.PermissionAggregate;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Identity.Auth;
using SmartTask.Identity.Queries;
using SmartTask.Identity.ViewModels;
using SmartTask.Infrastructure.Commands;
using SmartTask.Infrastructure.Idempotency;

namespace SmartTask.UserDetails.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {

        private readonly IUserRepository _userRepository;
        private readonly IUserQueries _userQueries;
        private readonly IUserManager _userManager;

        public DeleteUserCommandHandler(IUserRepository userRepository, IUserQueries userQueries, IUserManager userManager)
        {
            _userRepository = userRepository;
            _userQueries = userQueries;
            _userManager = userManager;
        }


        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            //PermissionDTO userPermission = await _userManager.GetPermissionAsync(PermissionName.User_Delete);

            //if (userPermission == null)
            //{
            //    throw new UnauthorizedAccessException($"You don't have {PermissionName.User_Delete} permission");
            //}

            foreach (var id in request.Id)
            {
                var user = await _userQueries.FindAsync(id);
                if (user != null)
                {
                    user.Delete();

                    _userRepository.UpdateAsync(user);
                }
            }

            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return true;
        }

        public class UserIdentifiedCommandHandler :
     IdentifiedCommandHandler<DeleteUserCommand, bool>
        {
            public UserIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
            {

            }

            protected override bool CreateResultForDuplicateRequest()
            {
                return true; // Ignore duplicate requests
            }
        }
    }
}
