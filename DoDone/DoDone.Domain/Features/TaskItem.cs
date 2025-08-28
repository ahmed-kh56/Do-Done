
namespace DoDone.Domain.Features
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public TaskItemStatus TaskStatus { get; set; }= TaskItemStatus.ToDo;
        public DateTime CreatedAt { get; set; }
        public Guid FeatureId { get; set; }
        public Feature Feature { get; set; }


    }

}
