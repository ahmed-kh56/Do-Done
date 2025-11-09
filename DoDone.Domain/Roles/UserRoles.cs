namespace DoDone.Domain.Roles
{
    public class UserRoles
    {
        public UserRoles(byte roleId, Guid userId, DateTime? startedAt=null)
        {
            RoleId = roleId;
            UserId = userId;
            StartedAt = startedAt??DateTime.UtcNow;
        }

        public int Id { get; set; }
        public byte RoleId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? LeftAt { get; set; }
    }
}
