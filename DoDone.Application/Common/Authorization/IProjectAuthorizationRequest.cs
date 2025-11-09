using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Application.Common.Authorization
{
    public interface IProjectAuthorizationRequest
    {
        Guid ProjectId { get; }
        Guid? FeatureId { get; }

        IEnumerable<DynamicRoleRequirement> RequiredDynamicRoles { get; }
    }

    public class DynamicRoleRequirement
    {
        public string Role { get; set; } = default!;
        public bool RequiresProject { get; set; }
        public bool RequiresFeature { get; set; }
    }


}
