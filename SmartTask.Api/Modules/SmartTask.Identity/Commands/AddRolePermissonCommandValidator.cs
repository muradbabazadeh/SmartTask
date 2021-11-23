using FluentValidation;
namespace SmartTask.Identity.Commands
{
    class AddRolePermissonCommandValidator : AbstractValidator<AddRolePermissonCommand>
    {
        public AddRolePermissonCommandValidator() : base()
        {
            RuleFor(command => command.RoleId).NotNull();
            RuleFor(command => command.Permissons).NotNull();
        }
    }
}
