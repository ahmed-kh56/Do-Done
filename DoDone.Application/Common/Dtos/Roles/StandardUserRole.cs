namespace DoDone.Application.Common.Dtos.Roles
{
    public class StandardUserRole
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? ProjectId { get; set; }
        public byte RoleId { get; set; }
        public string? RoleName { get; set; } = string.Empty;
        public Guid? FeatureId { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime? LeftAt { get; set; }

        public bool IsLeft { get; set; }

        public string DynamicToString()
        {
            return $"{RoleName??""}:{ProjectId?.ToString("N")}:{FeatureId?.ToString()??""}";
        }
        public string StaticToString()
        {
            return $"{RoleName}";
        }
    }

}
