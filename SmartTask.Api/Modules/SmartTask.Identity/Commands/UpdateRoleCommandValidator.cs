using FluentValidation;
namespace SmartTask.Identity.Commands
{
    class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator() : base()
        {
            RuleFor(command => command.Name).NotNull();
            RuleFor(command => command.Id).NotNull();
        }
    }
}
