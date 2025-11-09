using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Application.Common.Dtos.UserDtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public Guid AssignedProjectUserRoleId { get; set; }
        public string FullName { get; set; } = null!;
        public string ShowName { get; set; } = null!;
        public string? ProfilePhotoLink { get; set; }
    }
}
