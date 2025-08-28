using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Application.Queries.Authentication
{
    public class UserProjectRoleDto
    {
        public Guid UserId { get; set; }

        public Guid? ProjectId { get; set; }

        public string UserRole { get; set; } = string.Empty;

        public DateTime StartedAt { get; set; }

        public DateTime? LeftAt { get; set; }

        public bool IsLeft { get; set; }
        override public string ToString()
        {
            return $"{UserRole}:{ProjectId}";
        }
    }

}
