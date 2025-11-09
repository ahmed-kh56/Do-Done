using DoDone.Application.Common.Dtos.Features;

namespace DoDone.Application.Common.Dtos.UserDtos
{
    public class ProjectEmployeeProfileResult
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePhotoLink { get; set; }
        public string EmployeeShowName { get; set; }
        public bool IsLeft { get; set; }
        public bool IsScrumMaster { get; set; }
        public IEnumerable<FeatureSummaryDto> features { get; set; } = new List<FeatureSummaryDto>();
    }

}
