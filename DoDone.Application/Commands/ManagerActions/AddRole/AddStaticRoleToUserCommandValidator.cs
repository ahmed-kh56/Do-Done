using FluentValidation;

namespace DoDone.Application.Commands.ManagerActions.AddRole;

public class AddStaticRoleToUserCommandValidator:AbstractValidator<AddStaticRoleToUserCommand>
{
    public AddStaticRoleToUserCommandValidator()
    {
        RuleFor(x => x.RoleName).NotEmpty().WithMessage("Role name is required.");
        RuleFor(x => x.UserId).NotEmpty().NotEqual(Guid.Empty).WithMessage("User ID is required.");
    }
}