using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Domain.Exceptions;
using SmartTask.Identity.Auth;
using SmartTask.Identity.Queries;
using SmartTask.Infrastructure.Commands;
using SmartTask.Infrastructure.Database;
using SmartTask.Infrastructure.Idempotency;
using SmartTask.User.Commands.Models;

namespace SmartTask.UserDetails.Commands
{
    public class RefreshUserTokenCommandHandler : IRequestHandler<RefreshUserTokenCommand, JwtTokenDTO>
    {
        private readonly IUserManager _userManager;
        private readonly IUserQueries _userQueries;
        private readonly IUserRepository _userRepository;
        private readonly SmartTaskDbContext _context;

        public RefreshUserTokenCommandHandler(IUserManager userManager, IUserQueries userQueries,
            IUserRepository userRepository, SmartTaskDbContext context)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _userQueries = userQueries ?? throw new ArgumentNullException(nameof(userQueries));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<JwtTokenDTO> Handle(RefreshUserTokenCommand request, CancellationToken cancellationToken)
        {
            var splitToken = request.RefreshToken.Split("_");

            var user = await _context.Users.FirstOrDefaultAsync(p => p.RefreshToken == request.RefreshToken);

            if (user == null)
                throw new AuthenticationException("Invalid token.");

            if (!user.Locked || user.IsDeleted)
                throw new DomainException("You are locked.");

            if (Convert.ToDateTime(splitToken[2]) < DateTime.Now)
                throw new AuthenticationException("Token is expired.");

            (string token, DateTime expiresAt) = _userManager.GenerateJwtToken(user);

            return new JwtTokenDTO
            {
                Token = token,
                RefreshToken = request.RefreshToken,
                ExpiresAt = expiresAt,
            };
        }

    }
}
