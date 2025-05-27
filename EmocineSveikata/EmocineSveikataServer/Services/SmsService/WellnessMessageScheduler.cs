using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using EmocineSveikataServer.Configuration;

namespace EmocineSveikataServer.Services.SmsService
{
    public class WellnessMessageScheduler : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WellnessMessageScheduler> _logger;
        private readonly SmsSettings _smsSettings;
        
        private TimeSpan _sendTime = TimeSpan.FromHours(9);

        public WellnessMessageScheduler(
            IServiceProvider serviceProvider,
            IOptions<SmsSettings> smsSettings,
            ILogger<WellnessMessageScheduler> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _smsSettings = smsSettings.Value;
            
            if (_smsSettings.DailySendHour >= 0 && _smsSettings.DailySendHour < 24)
            {
                _sendTime = TimeSpan.FromHours(_smsSettings.DailySendHour);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Wellness Message Scheduler started. Will send messages daily at {time}", 
                _sendTime.ToString(@"hh\:mm"));
                
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRunTime = CalculateNextRunTime(now);
                var delay = nextRunTime - now;
                
                _logger.LogInformation("Next wellness messages will be sent at {time}", nextRunTime.ToString("yyyy-MM-dd HH:mm:ss"));
                
                await Task.Delay(delay, stoppingToken);
                
                await SendWellnessMessagesAsync();
                
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        
        private DateTime CalculateNextRunTime(DateTime currentTime)
        {
            var scheduledTime = currentTime.Date.Add(_sendTime);
            
            if (currentTime > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }
            
            return scheduledTime;
        }
        
        private async Task SendWellnessMessagesAsync()
        {
            try
            {
                if (!_smsSettings.Enabled)
                {
                    _logger.LogInformation("SMS notifications are disabled in settings. Skipping wellness messages.");
                    return;
                }
                
                using (var scope = _serviceProvider.CreateScope())
                {
                    var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
                    
                    _logger.LogInformation("Starting to send daily wellness messages...");
                    int sentCount = await notificationService.SendWellnessMessagesToAllEligibleUsersAsync();
                    _logger.LogInformation("Completed sending wellness messages to {count} users", sentCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending wellness messages");
            }
        }
    }
}
