using MediatR;
using Microsoft.Extensions.Options;
using SmartTask.Identity.Auth;
using SmartTask.Identity.Queries;
using SmartTask.Infrastructure;
using SmartTask.Infrastructure.Commands;
using SmartTask.Infrastructure.Idempotency;
using SmartTask.User.Commands.Models;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartTask.User.Commands
{
    public class CheckTokenVerifyInputCommandHandler : IRequestHandler<CheckTokenVerifyInputCommand, bool>
    {
        private readonly IUserManager _userManager;
        private readonly IUserQueries _userQueries;
        private readonly SmartTaskSettings _saffarmaSettings;


        public CheckTokenVerifyInputCommandHandler(IUserManager userManager, IUserQueries userQueries, IOptions<SmartTaskSettings> saffarmaSettings)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _userQueries = userQueries ?? throw new ArgumentNullException(nameof(userQueries));
            _saffarmaSettings = saffarmaSettings.Value ?? throw new ArgumentNullException(nameof(saffarmaSettings));
        }

        public async Task<bool> Handle(CheckTokenVerifyInputCommand request, CancellationToken cancellationToken)
        {
            var userName = request.UserName.ToLower();
            var user = await _userQueries.FindByNameAsync(userName);
            var userId = user.Id.ToString();  //because of validateTokenUserId , it accepts only String value

            if (user == null)
                throw new AuthenticationException("Invalid credentials.");

            var validateTokenUserId = TokenManager.ValidateToken(_saffarmaSettings, request.Token);

            if (validateTokenUserId == null)
            {
                throw new AuthenticationException("Token is null");
            }


            if (!userId.Equals(validateTokenUserId))
            {
                throw new AuthenticationException("Token is invalid for this user");
            }

            return true;
        }

        public class CheckTokenInputCommandHandler : IdentifiedCommandHandler<CheckTokenVerifyInputCommand, bool>
        {
            public CheckTokenInputCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
            {
            }

            protected override bool CreateResultForDuplicateRequest()
            {
                return true;
            }
        }
    }
}
