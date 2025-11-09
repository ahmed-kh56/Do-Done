using DoDone.Application.Common.Authorization;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Application.Commands.Projects.AssigneeEmployeeToProject
{
    [Authorize(Role ="Manager")]
    public record AssigneeEmployeeToProjectCommand(Guid UserId,Guid ProjectId,string RoleName):IRequest<ErrorOr<Guid>>;

}
