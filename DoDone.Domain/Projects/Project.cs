using DoDone.Domain.Features;

namespace DoDone.Domain.Projects
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsStarted { get; set; }
        public DateTime? StartDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<ProjectUserRole> ProjectUserRoles { get; set; } = new List<ProjectUserRole>();
        public ICollection<Feature> Features { get; set; }= new List<Feature>();
        public Project() { }
        public Project(string name,bool isStarted,DateTime? startDate)
        {
            Id = Guid.NewGuid();
            Name = name;
            StartDate = startDate;
            IsStarted= isStarted;
            EndDate = null;
            CreatedAt = DateTime.Now;
            IsCompleted=false;

        }
        public bool IsEnded => !(EndDate is null);
        public void End()
        {
            if (IsEnded)
            {
                throw new InvalidOperationException("Project is already ended.");
            }
            EndDate = DateTime.Now;
            IsCompleted = true;
        }

    }

     
}
