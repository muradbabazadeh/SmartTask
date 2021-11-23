using FluentValidation;
namespace SmartTask.User.Commands
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator() : base()
        {
            RuleFor(command => command.Password).NotNull();
            RuleFor(command => command.Email).NotNull();
            //.NotEmpty().EmailAddress().WithMessage("A valid email address is required.");
            RuleFor(command => command.FirstName).NotNull();
        }
    }
}
