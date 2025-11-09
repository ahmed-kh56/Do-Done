using DoDone.Domain.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Domain.Features
{
    public class Feature
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid AssignedProjectUserRoleId { get; set; }
        public bool IsCompleted { get; set; }

        public ProjectUserRole UserRole { get; set; }
        public ICollection<TaskItem> FeatureTasks { get; set; } = new List<TaskItem>();
    }
}
