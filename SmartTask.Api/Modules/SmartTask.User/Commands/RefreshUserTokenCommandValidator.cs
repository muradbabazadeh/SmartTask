using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.UserDetails.Commands
{
   public class RefreshUserTokenCommandValidator : AbstractValidator<RefreshUserTokenCommand>
    {
        public RefreshUserTokenCommandValidator() : base()
        {
            RuleFor(command => command.RefreshToken).NotNull();
        }
    }
}
