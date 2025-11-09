using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Application.Common.Dtos.Features
{
    public class FeatureDto
    {
        public Guid Id { get; set; }
        public Guid AssignedProjectUserRoleId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string AssigneeName { get; set; } = null!;
    }
}
