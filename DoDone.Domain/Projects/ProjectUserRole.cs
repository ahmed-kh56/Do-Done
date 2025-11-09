using DoDone.Domain.Features;
using DoDone.Domain.Roles;
using DoDone.Domain.Users;

namespace DoDone.Domain.Projects
{
    public class ProjectUserRole
    {
        public ProjectUserRole
            (Guid userId,
            Guid projectId,
            byte roleId,
            Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            UserId = userId;
            ProjectId = projectId;
            RoleId = roleId;
            StartedAt = DateTime.Now;
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public Project project { get; set; } = null!;
        public byte RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public DateTime StartedAt { get; set; } =DateTime.Now;
        public DateTime? LeftAt { get; set; }

        public ICollection<Feature> Features { get; set; } = new List<Feature>();
        public bool IsActive => LeftAt == null || LeftAt > DateTime.UtcNow;
        public void End() => LeftAt = DateTime.Now;

    }

}
