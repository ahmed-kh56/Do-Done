using DoDone.Application.Common.Authorization;
using DoDone.Application.Common.Dtos.Roles;
using DoDone.Application.Common.Dtos.UserDtos;
using DoDone.Domain.Roles;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Application.Commands.ManagerActions.AddRole;
[Authorize(Role ="Manager")]
public record AddStaticRoleToUserCommand(string RoleName,Guid UserId):IRequest<ErrorOr<AddStaticRoleResponse>>;
