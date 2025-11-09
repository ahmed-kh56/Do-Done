using DoDone.Application.Common.Dtos.Roles;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Interfaces.Services;
using DoDone.Domain.Outbox;
using DoDone.Domain.Roles;
using DoDone.Domain.Users;
using ErrorOr;
using MediatR;

namespace DoDone.Application.Commands.ManagerActions.AddRole
{

    public class AddStaticRoleToUserCommandHandler(
        IRolesRepository _rolesRepository,
        IUnitOfWork _unitOfWork,
        IUserRolesRepository _userRolesRepository,
        IEmailOutboxRepository _emailRepository,
        IUsersRepository _usersRepository) : IRequestHandler<AddStaticRoleToUserCommand, ErrorOr<AddStaticRoleResponse>>
    {
        public async Task<ErrorOr<AddStaticRoleResponse>> Handle(AddStaticRoleToUserCommand request, CancellationToken cancellationToken)
        {

            var roleId = await _rolesRepository.GetRoleIdByNameAsync(request.RoleName);
            if (roleId == 0)
            {
                return Error.NotFound(description: "Role not found");
            }

            var roles = await _rolesRepository.GetUserRolesAsync(request.UserId, onlyStatic: true);

            if (roles.Any(r => r.RoleId == roleId))
            {
                return Error.Conflict(description: "User already has this role");
            }
            var user = await _usersRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return Error.NotFound(description: "User not found");
            }

            var userRole = new UserRoles(roleId, request.UserId);
            var email = new EmailOutbox
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                UserName = user.ShowName,
                UserEmail = user.Email,
                EmailType = EmailTemplateType.AddStaticRole.ToString(),
                Token = request.RoleName

            };

            await _unitOfWork.ExecuteInTransactionAsync(async (conn, tran) =>
            {
                userRole.Id = await _userRolesRepository.AddAsync(userRole, conn, tran);
                await _emailRepository.AddAsync(email,conn,tran);

            });


            return userRole.ToStaticUserRoleResponse(request);

        }
    }
    public static class UserRoleExtensions
    {
        public static AddStaticRoleResponse ToStaticUserRoleResponse(this UserRoles userRole, AddStaticRoleToUserCommand request)
        {
            return new AddStaticRoleResponse
            {
                Id = userRole.Id,
                RoleName= request.RoleName,
                UserId = userRole.UserId,
                AssignedAt = userRole.StartedAt
            };
        }
    }
}