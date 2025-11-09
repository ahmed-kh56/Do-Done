using DoDone.Application.Common.Dtos.Features;
using DoDone.Application.Common.Dtos.UserDtos;

namespace DoDone.Application.Common.Dtos.ProjectDtos
{
    public class ProjectSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsStarted { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // KPIs

        public int EmployeeCount { get; set; }
        public IEnumerable<ProjectEmployeeProfileResult> Employees { get; set; } = new List<ProjectEmployeeProfileResult>();
        public int FeaturesCount { get; set; }
        public int TotalTasks { get; set; }

        public IEnumerable<FeatureSummaryDto> UnAssignedFeatures { get; set; } = new List<FeatureSummaryDto>();
    }

}
