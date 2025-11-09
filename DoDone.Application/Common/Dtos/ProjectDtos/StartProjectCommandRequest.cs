using DoDone.Application.Commands.Projects.StartProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Application.Common.Dtos.ProjectDtos
{
    public record StartProjectCommandRequest
    {
        public string Name { get; set; } = string.Empty;
        public bool IsStarted { get; set; }
        public DateTime? StartDate { get; set; }
        public StartProjectCommand ToCommand() => new StartProjectCommand(Name, IsStarted, StartDate);
    }
}
