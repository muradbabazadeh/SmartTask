using FluentValidation;
namespace SmartTask.User.Commands
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator() : base()
        {
            RuleFor(command => command.Id).NotNull();
            RuleFor(command => command.Email).NotNull();
            RuleFor(command => command.FirstName).NotNull();
            RuleFor(command => command.LastName).NotNull();
        }
    }
}
