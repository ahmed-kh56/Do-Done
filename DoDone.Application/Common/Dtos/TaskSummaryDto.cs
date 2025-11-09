namespace DoDone.Application.Common.Dtos
{
    public class TaskSummaryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Status { get; set; } = null!; // ToDo, InProgress, Done
        public DateTime CreatedAt { get; set; }
        public Guid? FeatureId { get; set; }
        public string? FeatureName { get; set; }
    }

}
