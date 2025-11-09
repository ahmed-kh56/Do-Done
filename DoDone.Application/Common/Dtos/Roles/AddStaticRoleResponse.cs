using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Application.Common.Dtos.Roles
{
    public class AddStaticRoleResponse
    {
        public string RoleName { get; internal set; }
        public int Id { get; internal set; }
        public Guid UserId { get; internal set; }
        public DateTime AssignedAt { get; internal set; }
    }
}
