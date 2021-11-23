using MediatR;
using SmartTask.Identity.Auth;
using SmartTask.Identity.Queries;
using SmartTask.SharedKernel.Infrastructure;
using SmartTask.User.Commands.Models;
using System;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using SmartTask.Domain.AggregatesModel.UserAggregate;

namespace SmartTask.User.Commands
{
    public class GetAuthorizationTokenCommandHandler : IRequestHandler<GetAuthorizationTokenCommand, JwtTokenDTO>
    {
        private readonly IUserManager _userManager;
        private readonly IUserQueries _userQueries;
        private readonly IUserRepository _userRepository;

        public GetAuthorizationTokenCommandHandler(IUserManager userManager, IUserQueries userQueries, IUserRepository userRepository)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _userQueries = userQueries ?? throw new ArgumentNullException(nameof(userQueries));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<JwtTokenDTO> Handle(GetAuthorizationTokenCommand request, 
            CancellationToken cancellationToken)
        {
            var userName = request.UserName.ToLower();
            var user = await _userQueries.FindByNameAsync(userName);
            if (user == null || user.PasswordHash != PasswordHasher.HashPassword(request.Password) || user.Locked || user.IsDeleted)
                throw new AuthenticationException("Invalid credentials.");


            var randomNumber = GenerateRandomNumber();
            var refreshToken = $"{randomNumber}_{user.Id}_{DateTime.Now.AddDays(30)}";


            user.UpdateRefreshToken(refreshToken);

            (string token, DateTime expiresAt) = _userManager.GenerateJwtToken(user);

            _userRepository.UpdateAsync(user);

            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
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
}
