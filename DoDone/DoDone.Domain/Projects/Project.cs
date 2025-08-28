using DoDone.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }


}
