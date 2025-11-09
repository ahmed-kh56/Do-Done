using DoDone.Application.Commands.ManagerActions.AddRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Application.Common.Dtos.Roles
{
    public record AddRoleToEmployeeRequest
    {
        public Guid UserId { get; set; }
        public string RoleName { get; set; }

        public AddStaticRoleToUserCommand ToCommand()
        {
            return new AddStaticRoleToUserCommand(RoleName, UserId);
        }
    }
}
