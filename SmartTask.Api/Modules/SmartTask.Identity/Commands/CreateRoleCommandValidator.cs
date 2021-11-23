
using FluentValidation;

namespace SmartTask.Identity.Commands
{
    class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator() : base()
        {
            RuleFor(command => command.Name).NotNull();
        }
    }
}
