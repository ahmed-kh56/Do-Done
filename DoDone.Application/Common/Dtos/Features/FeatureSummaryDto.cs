namespace DoDone.Application.Common.Dtos.Features
{
    public class FeatureSummaryDto
    {
        public Guid FeatureId { get; set; }
        public Guid ProjectId { get; set; }

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid AssinedPURId { get; set; }
        public Guid AssignedUserId { get; set; }
        public string AssignedUserName { get; set; } = null!;
        public bool IsCompleted { get; set; }

        public int TotalTasks { get; set; }

    }

}
