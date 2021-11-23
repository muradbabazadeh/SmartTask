using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Identity.Queries;
using SmartTask.Infrastructure.Commands;
using SmartTask.Infrastructure.Idempotency;
using SmartTask.SharedKernel.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartTask.User.Commands
{
    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserQueries _userQueries;

        public UpdateUserPasswordCommandHandler(IUserRepository userRepository, IUserQueries userQueries)
        {
            _userRepository = userRepository;
            _userQueries = userQueries;
        }

        public async Task<bool> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userQueries.GetUserEntityAsync(request.Id);


            user.SetPasswordHash(user.PasswordHash, PasswordHasher.HashPassword(request.Password));
            _userRepository.UpdateAsync(user);
            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        public class UpdateUserPasswordCommandIdentitfyHandler : IdentifiedCommandHandler<UpdateUserPasswordCommand, bool>
        {
            public UpdateUserPasswordCommandIdentitfyHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
            {
            }

            protected override bool CreateResultForDuplicateRequest()
            {
                return true; // Ignore duplicate requests
            }
        }
    }
}
