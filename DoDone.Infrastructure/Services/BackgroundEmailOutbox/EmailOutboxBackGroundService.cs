using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Interfaces.Service;
using DoDone.Domain.Outbox;
using DoDone.Domain.Users;
using DoDone.Infrastructure.Services.Email;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DoDone.Infrastructure.Services.BackgroundEmailOutbox
{
    public class EmailOutboxBackGroundService : BackgroundService
    {
        private readonly TimeSpan _delay = TimeSpan.FromSeconds(30);
        private readonly IServiceProvider _serviceProvider;

        public EmailOutboxBackGroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var emailOutboxRepository = scope.ServiceProvider.GetRequiredService<IEmailOutboxRepository>();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    var pendingEmails = await emailOutboxRepository.GetPendingAsync(50);

                    foreach (var email in pendingEmails)
                    {
                        try
                        {
                            var content = EmailBodyTemplates.GenerateTemplate(
                                email.UserName,
                                email.Token ?? "",
                                (EmailTemplateType)Enum.Parse(typeof(EmailTemplateType), email.EmailType));

                            await emailService.SendEmailAsync(email.UserEmail, content.Subject, content.Body);

                            await emailOutboxRepository.UpdateAsync(EmailOutbox.MarkSent(email));
                        }
                        catch (Exception ex)
                        {
                            await emailOutboxRepository.UpdateAsync(EmailOutbox.MarkFailed(email, ex.Message));
                        }
                    }

                    await Task.Delay(_delay, stoppingToken);
                }
            }
        }
    }
}
