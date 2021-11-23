using FluentValidation;
namespace SmartTask.Identity.Commands
{
    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator() : base()
        {
            RuleFor(command => command.Id).NotNull();
        }
    }
}
