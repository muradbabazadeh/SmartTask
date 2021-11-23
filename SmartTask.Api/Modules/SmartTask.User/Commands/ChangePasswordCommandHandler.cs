using MediatR;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Identity.Auth;
using SmartTask.Infrastructure.Commands;
using SmartTask.Infrastructure.Idempotency;
using SmartTask.SharedKernel.Infrastructure;
using SmartTask.User.Commands.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SmartTask.User.Commands
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, JwtTokenDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserManager _userManager;

        public ChangePasswordCommandHandler(IUserRepository userRepository, IUserManager userManager)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<JwtTokenDTO> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetCurrentUser();
            var oldPasswordHash = PasswordHasher.HashPassword(request.OldPassword);
            var newPasswordHash = PasswordHasher.HashPassword(request.NewPassword);
            user.SetPasswordHash(oldPasswordHash, newPasswordHash);

            var randomNumber = GenerateRandomNumber();
            var refreshToken = $"{randomNumber}_{user.Id}_{DateTime.Now.AddDays(30)}";

            user.UpdateRefreshToken(refreshToken);

            _userRepository.UpdateAsync(user);

            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            (string token, DateTime expiresAt) = _userManager.GenerateJwtToken(user);
            return new JwtTokenDTO
            {
                Token = token,
                RefreshToken=refreshToken,
                ExpiresAt = expiresAt
            };
        }

        private string GenerateRandomNumber()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }

    public class ChangePasswordIdentifiedCommandHandler : IdentifiedCommandHandler<ChangePasswordCommand, JwtTokenDTO>
    {
        public ChangePasswordIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
        {
        }

        protected override JwtTokenDTO CreateResultForDuplicateRequest() => null;
    }

   
}
