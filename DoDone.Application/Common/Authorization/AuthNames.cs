
namespace DoDone.Application.Common.Authorization
{
    public static class AuthNames
    {

        public readonly static string ScrumMaster = "ScrumMaster";
        public readonly static string NormalizedScrumMaster = "scrummaster";
        public readonly static string Employee = "Employee";
        public readonly static string NormalizedEmployee = "employee";
        public readonly static string None = "";

        public readonly static DynamicRoleRequirement ScrumMasterRequirement = new DynamicRoleRequirement 
        {
            Role = ScrumMaster,
            RequiresProject = true,
            RequiresFeature = false
        };
        public readonly static DynamicRoleRequirement ProjectEmployeeRequirement = new DynamicRoleRequirement 
        {
            Role = Employee,
            RequiresProject = true,
            RequiresFeature = false
        };
        public readonly static DynamicRoleRequirement EmployeeFeatureRequirement = new DynamicRoleRequirement 
        {
            Role = Employee,
            RequiresProject = true,
            RequiresFeature = true
        };

        public static Guid DefaultEmployeeId { get; set; } = Guid.Empty;
    }
}
