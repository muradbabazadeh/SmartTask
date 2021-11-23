using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Identity.Auth;
using SmartTask.Identity.Queries;
using SmartTask.Infrastructure.Commands;
using SmartTask.Infrastructure.Idempotency;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartTask.User.Commands
{
   public class ChangeUserStatusCommandHandler : IRequestHandler<ChangeUserStatusCommand,bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserQueries _userQueries;
        private readonly IUserManager _userManager;

        public ChangeUserStatusCommandHandler(IUserRepository userRepository, IUserQueries userQueries, IUserManager userManager)
        {
            _userRepository = userRepository;
            _userQueries = userQueries;
            _userManager = userManager;
        }


        public async Task<bool> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
        {
            //PermissionDTO userPermission = await _userManager.GetPermissionAsync(PermissionName.User_Delete);

            //if (userPermission == null)
            //{
            //    throw new UnauthorizedAccessException($"You don't have {PermissionName.User_Delete} permission");
            //}

            
                var user = await _userQueries.FindAsync(request.Id);
                if (user != null)
                {
                    user.LockedUser(request.Status);

                    _userRepository.UpdateAsync(user);
                }

            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return true;
        }

        public class UserIdentifiedCommandHandler :
     IdentifiedCommandHandler<ChangeUserStatusCommand, bool>
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
