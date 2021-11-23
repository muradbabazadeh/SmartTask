using FluentValidation;
namespace SmartTask.User.Commands
{
    class GetAuthorizationTokenCommandValidator : AbstractValidator<GetAuthorizationTokenCommand>
    {
        public GetAuthorizationTokenCommandValidator() : base()
        {
            RuleFor(command => command.UserName).NotNull();
            RuleFor(command => command.Password).NotNull();
        }
    }
}
