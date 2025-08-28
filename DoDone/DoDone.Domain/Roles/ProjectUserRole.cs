using DoDone.Domain.Features;
using DoDone.Domain.Projects;
using DoDone.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Domain.Roles
{
    public class ProjectUserRole
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid? ProjectId { get; set; } = null;
        public Project project { get; set; } = null!;
        public byte RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public DateTime StartedAt { get; set; }
        public DateTime? LeftAt { get; set; }

        public ICollection<Feature> Features { get; set; } = new List<Feature>();
        public bool IsActive() => LeftAt == null || LeftAt > DateTime.UtcNow;

    }

}
