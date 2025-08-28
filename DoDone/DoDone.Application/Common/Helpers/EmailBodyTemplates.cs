using DoDone.Domain.Users;

namespace DoDone.Application.Common.Helpers
{
    public static class EmailBodyTemplates
    {



        public static string GenerateTemplate(string name, string token, TokenTypes type)
        {
            return type switch
            {
                var t when t == TokenTypes.EmailConfirmation => EmailConfirmationTemplate(name, token),
                var t when t == TokenTypes.PasswordReset => PasswordResetTemplate(name, token),
                var t when t == TokenTypes.TwoFactorAuthentication => TwoFactorTemplate(name, token),
                // Add more templates as needed
                _ => throw new ArgumentException("Unsupported token type", nameof(type))
            };
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

    }

}
