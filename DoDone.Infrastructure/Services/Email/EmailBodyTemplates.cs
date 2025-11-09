using DoDone.Domain.Users;

namespace DoDone.Infrastructure.Services.Email
{
    public static class EmailBodyTemplates
    {



        public static EmailContent GenerateTemplate(string name, string token, EmailTemplateType type)
        {
            return type switch
            {
                EmailTemplateType.emailConfirmation =>
                    new EmailContent(
                        "Confirm your email",
                        EmailConfirmationTemplate(name, token)
                    ),

                EmailTemplateType.passwordReset =>
                    new EmailContent(
                        "Reset your password",
                        PasswordResetTemplate(name, token)
                    ),

                EmailTemplateType.TwoFA =>
                    new EmailContent(
                        "Your Two-Factor Authentication Code",
                        TwoFactorTemplate(name, token)
                    ),

                EmailTemplateType.Welcome =>
                    new EmailContent(
                        "Welcome to DoDone 🎉",
                        WelcomeTemplate(name)
                    ),

                EmailTemplateType.FeatureAssigned =>
                    new EmailContent(
                        "New Feature Assigned to You",
                        FeatureAssignedTemplate(name, token)
                    ),

                EmailTemplateType.ProjectAssigned =>
                    new EmailContent(
                        "You’ve been assigned to a Project",
                        ProjectAssignedTemplate(name, token)
                    ),

                    EmailTemplateType.AddStaticRole =>
                    new EmailContent(
                        $"You’ve been been appointed as {token}",
                        StaticRoleAssignedTemplate(name, token)
                    ),
                    EmailTemplateType.RemoveFromeProject =>
                    new EmailContent(
                        "You’ve been removed from a Project",
                        ProjectRemovedTemplate(name, token)
                    ),
                    EmailTemplateType.RemoveStaticRole =>
                    new EmailContent(
                        $"Your role {token} has been removed",
                        StaticRoleRemovedTemplate(name, token)
                    ),

                _ => throw new ArgumentException("Unsupported token type", nameof(type))
            };
        }

        private static string WelcomeTemplate(string name)
        {
            return $@"
            <div style=""font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:20px;
                        border:1px solid #eee;border-radius:10px;"">
                <h2 style=""color:#2c3e50;"">Welcome to DoDone 🎉</h2>
        
                <p style=""font-size:16px;"">Hello <strong>{name}</strong>,</p>
        
                <p style=""font-size:15px;"">
                    We're excited to have you on board! 🎊
                </p>

                <p style=""font-size:15px;"">
                    Start exploring projects, features, and tasks with <strong>DoDone</strong> today.
                </p>

                <p style=""font-size:13px;color:#888;margin-top:30px;"">
                    If you have any questions, feel free to reach out to our support team.
                </p>
            </div>";
        }

        private static string ProjectAssignedTemplate(string name, string token)
        {
            var parts = token.Split('|', 2);
            var projectName = parts.Length > 0 ? parts[0] : "Unknown Project";
            var role = parts.Length > 1 ? parts[1] : "Unknown Role";

            return $@"
            <div style=""font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:20px;
                        border:1px solid #eee;border-radius:10px;"">
                <h2 style=""color:#2c3e50;"">Project Assignment 📌</h2>
        
                <p style=""font-size:16px;"">Hello <strong>{name}</strong>,</p>
        
                <p style=""font-size:15px;"">
                    You have been assigned to the project:
                </p>

                <div style=""margin:20px auto;text-align:center;font-size:18px;font-weight:bold;
                            padding:12px 18px;background:#f4f4f4;border-radius:8px;border:1px dashed #ccc;
                            width:fit-content;"">
                    {projectName}
                </div>

                <p style=""font-size:15px;"">
                    Your role in this project will be: <strong>{role}</strong>
                </p>

                <p style=""font-size:13px;color:#888;margin-top:30px;"">
                    Please check your project board for more information.
                </p>
            </div>";
        }

        private static string FeatureAssignedTemplate(string name, string token)
        {
            var parts = token.Split('|', 2);
            var projectName = parts.Length > 0 ? parts[0] : "Unknown Project";
            var featureName = parts.Length > 1 ? parts[1] : "Unknown Feature";

            return $@"
            <div style=""font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:20px;
                        border:1px solid #eee;border-radius:10px;"">
                <h2 style=""color:#2c3e50;"">New Feature Assigned 🚀</h2>
        
                <p style=""font-size:16px;"">Hello <strong>{name}</strong>,</p>
        
                <p style=""font-size:15px;"">
                    You have been assigned to the following feature in project:
                </p>

                <div style=""margin:20px auto;text-align:center;font-size:18px;font-weight:bold;
                            padding:12px 18px;background:#f4f4f4;border-radius:8px;border:1px dashed #ccc;
                            width:fit-content;"">
                    {featureName}
                </div>

                <p style=""font-size:15px;"">
                    Project: <strong>{projectName}</strong>
                </p>

                <p style=""font-size:13px;color:#888;margin-top:30px;"">
                    Please check your dashboard for more details.
                </p>
            </div>";
        }

        private static string EmailConfirmationTemplate(string name, string token)
        {
            return $@"
            <div style=""font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:20px;border:1px solid #eee;border-radius:10px;"">
                <h2 style=""color:#2c3e50;"">Welcome to DoDone 👋</h2>
                
                <p style=""font-size:16px;"">Hello <strong>{name}</strong>,</p>
                
                <p style=""font-size:15px;"">Thanks for registering at <strong>DoDone</strong>.</p>
                
                <p style=""font-size:15px;"">
                    Please confirm your email by entering the following code in the app:
                </p>

                <div style=""margin:20px auto;text-align:center;font-size:22px;font-weight:bold;padding:15px 20px;background:#f4f4f4;border-radius:8px;border:1px dashed #ccc;width:fit-content;"">
                    {token}
                </div>

                <p style=""font-size:13px;color:#888;margin-top:30px;"">
                    If you didn’t register, you can safely ignore this email.
                </p>
            </div>";
        }
        private static string PasswordResetTemplate(string name, string token)
        {
            return $@"
            <div style=""font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:20px;border:1px solid #eee;border-radius:10px;"">
                <h2 style=""color:#2c3e50;"">Password Reset Request</h2>
                
                <p style=""font-size:16px;"">Hello <strong>{name}</strong>,</p>
                
                <p style=""font-size:15px;"">We received a request to reset your password.</p>
                
                <p style=""font-size:15px;"">
                    Please use the following code to reset your password:
                </p>
                <div style=""margin:20px auto;text-align:center;font-size:22px;font-weight:bold;padding:15px 20px;background:#f4f4f4;border-radius:8px;border:1px dashed #ccc;width:fit-content;"">
                    {token}
                </div>
                <p style=""font-size:13px;color:#888;margin-top:30px;"">
                    If you didn’t request this, you can safely ignore this email.
                </p>
            </div>";
        }
        private static string TwoFactorTemplate(string name, string token)
        {
            return $@"
        <div style=""font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:20px;border:1px solid #eee;border-radius:10px;"">
            <h2 style=""color:#2c3e50;"">Two-Factor Authentication Code</h2>
            <p style=""font-size:16px;"">Hello <strong>{name}</strong>,</p>
            <p style=""font-size:15px;"">Use the following code to complete your login:</p>
            <div style=""margin:20px auto;text-align:center;font-size:22px;font-weight:bold;padding:15px 20px;background:#f4f4f4;border-radius:8px;border:1px dashed #ccc;width:fit-content;"">
                {token}
            </div>
            <p style=""font-size:13px;color:#888;margin-top:30px;"">If you didn't try to log in, please ignore this email.</p>
        </div>";
        }

        private static string StaticRoleAssignedTemplate(string name, string token)
        {
            var role = string.IsNullOrWhiteSpace(token) ? "Unknown Role" : token;

            return $@"
            <div style=""font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:20px;
                        border:1px solid #eee;border-radius:10px;"">
                <h2 style=""color:#2c3e50;"">Role Assignment 🎯</h2>

                <p style=""font-size:16px;"">Hello <strong>{name}</strong>,</p>

                <p style=""font-size:15px;"">
                    You have been assigned the following role:
                </p>

                <div style=""margin:20px auto;text-align:center;font-size:18px;font-weight:bold;
                            padding:12px 18px;background:#f4f4f4;border-radius:8px;border:1px dashed #ccc;
                            width:fit-content;"">
                    {role}
                </div>

                <p style=""font-size:13px;color:#888;margin-top:30px;"">
                    Please check your account permissions for more details.
                </p>
            </div>";
        }
        private static string ProjectRemovedTemplate(string name, string token)
        {
            var projectName = string.IsNullOrWhiteSpace(token) ? "Unknown Project" : token;

            return $@"
            <div style=""font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:20px;
                        border:1px solid #eee;border-radius:10px;"">
                <h2 style=""color:#c0392b;"">Project Removal ⚠️</h2>

                <p style=""font-size:16px;"">Hello <strong>{name}</strong>,</p>

                <p style=""font-size:15px;"">
                    You have been removed from the following project:
                </p>

                <div style=""margin:20px auto;text-align:center;font-size:18px;font-weight:bold;
                            padding:12px 18px;background:#f4f4f4;border-radius:8px;border:1px dashed #ccc;
                            width:fit-content;"">
                    {projectName}
                </div>

                <p style=""font-size:13px;color:#888;margin-top:30px;"">
                    If you think this is a mistake, please contact your project manager.
                </p>
            </div>";
        }


        private static string StaticRoleRemovedTemplate(string name, string token)
        {
            var role = string.IsNullOrWhiteSpace(token) ? "Unknown Role" : token;

            return $@"
            <div style=""font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:20px;
                        border:1px solid #eee;border-radius:10px;"">
                <h2 style=""color:#c0392b;"">Role Removal ⚠️</h2>

                <p style=""font-size:16px;"">Hello <strong>{name}</strong>,</p>

                <p style=""font-size:15px;"">
                    The following role has been removed from your account:
                </p>

                <div style=""margin:20px auto;text-align:center;font-size:18px;font-weight:bold;
                            padding:12px 18px;background:#f4f4f4;border-radius:8px;border:1px dashed #ccc;
                            width:fit-content;"">
                    {role}
                </div>

                <p style=""font-size:13px;color:#888;margin-top:30px;"">
                    If you think this is a mistake, please contact your administrator.
                </p>
            </div>";
        }


    }

}
