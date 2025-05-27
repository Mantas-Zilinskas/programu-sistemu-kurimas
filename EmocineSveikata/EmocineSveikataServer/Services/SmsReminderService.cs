using EmocineSveikataServer.Repositories.ProfileRepository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EmocineSveikataServer.Services
{
    public class SmsReminderService : BackgroundService
    {
        private readonly ILogger<SmsReminderService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly Random _random = new Random();
        
        private static readonly TimeSpan _reminderInterval = TimeSpan.FromDays(1);

        public SmsReminderService(
            ILogger<SmsReminderService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SMS Reminder Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await SendReminders();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while sending SMS reminders");
                }

                int jitterMinutes = _random.Next(0, 60);
                await Task.Delay(TimeSpan.FromMinutes(jitterMinutes), stoppingToken);
                await Task.Delay(_reminderInterval, stoppingToken);
            }

            _logger.LogInformation("SMS Reminder Service is stopping.");
        }

        private async Task SendReminders()
        {
            _logger.LogInformation("Starting to send SMS reminders at: {time}", DateTime.UtcNow);

            using (var scope = _serviceProvider.CreateScope())
            {
                var userProfileRepository = scope.ServiceProvider.GetRequiredService<IUserProfileRepository>();
                var twilioService = scope.ServiceProvider.GetRequiredService<ITwilioService>();
                var usersWithSmsEnabled = await userProfileRepository.GetUsersWithSmsNotificationsEnabled();
                var reminderMessages = twilioService.GetReminderMessagesByTopic();

                foreach (var userProfile in usersWithSmsEnabled)
                {
                    if (string.IsNullOrEmpty(userProfile.PhoneNumber) || 
                        (userProfile.LastSmsReminder.HasValue && 
                         userProfile.LastSmsReminder.Value.Date >= DateTime.UtcNow.Date))
                    {
                        continue;
                    }

                    string reminderTopic = userProfile.SmsReminderTopic ?? string.Empty;
                    
                    if (string.IsNullOrEmpty(reminderTopic) && !string.IsNullOrEmpty(userProfile.SelectedTopics))
                    {
                        try
                        {
                            var selectedTopics = JsonSerializer.Deserialize<List<string>>(userProfile.SelectedTopics);
                            reminderTopic = selectedTopics?.FirstOrDefault() ?? string.Empty;
                            reminderTopic = reminderTopic.Replace(" ", "");
                        }
                        catch
                        {
                        }
                    }

                    if (string.IsNullOrEmpty(reminderTopic) || 
                        !reminderMessages.ContainsKey(reminderTopic) || 
                        !reminderMessages[reminderTopic].Any())
                    {
                        continue;
                    }

                    int messageIndex = _random.Next(reminderMessages[reminderTopic].Count);
                    string message = reminderMessages[reminderTopic][messageIndex];

                    bool success = await twilioService.SendSmsAsync(userProfile.PhoneNumber, message);

                    if (success)
                    {
                        userProfile.LastSmsReminder = DateTime.UtcNow;
                        await userProfileRepository.UpdateUserProfile(userProfile);
                        
                        _logger.LogInformation("SMS reminder sent to user {UserId} on topic {Topic}", 
                            userProfile.UserId, reminderTopic);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to send SMS reminder to user {UserId}", userProfile.UserId);
                    }
                }
            }

            _logger.LogInformation("Finished sending SMS reminders at: {time}", DateTime.UtcNow);
        }
    }
}
