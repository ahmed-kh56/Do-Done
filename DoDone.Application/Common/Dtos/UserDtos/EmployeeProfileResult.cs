namespace DoDone.Application.Common.Dtos.UserDtos
{
    public  class EmployeeProfileResult
    {
        public Guid EmployeeId { get; set; }
        public string FullName { get; set; } = null!;
        public string ShowName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ProfilePhotoLink = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int ProjectCount { get; set; }
        public string HighestRoleName { get; set; }


    }
}
