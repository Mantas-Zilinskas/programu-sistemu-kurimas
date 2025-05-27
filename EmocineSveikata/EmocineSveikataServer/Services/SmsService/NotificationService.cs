using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Repositories.UserRepository;
using Microsoft.Extensions.Logging;

namespace EmocineSveikataServer.Services.SmsService
{
    public class NotificationService
    {
        private readonly ISmsService _smsService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            ISmsService smsService, 
            IUserRepository userRepository,
            ILogger<NotificationService> logger)
        {
            _smsService = smsService;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Daily message
        /// </summary>
        public async Task<bool> SendDailyWellnessMessageAsync(int userId)
        {
            var userProfile = await _userRepository.GetUserProfileAsync(userId);
            if (userProfile == null)
            {
                _logger.LogWarning($"Cannot send wellness message: User profile not found for user {userId}");
                return false;
            }

            if (!userProfile.EnableSmsNotifications || string.IsNullOrEmpty(userProfile.PhoneNumber))
            {
                _logger.LogInformation($"SMS notifications disabled or no phone number for user {userId}");
                return false;
            }
            
            string[] topics = null;
            if (!string.IsNullOrEmpty(userProfile.SelectedTopics))
            {
                try
                {
                    topics = JsonSerializer.Deserialize<string[]>(userProfile.SelectedTopics);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error deserializing selected topics for user {userId}");
                }
            }
            
            return await _smsService.SendDailyWellnessMessageAsync(userProfile.PhoneNumber, topics);
        }
        
        /// <summary>
        /// Daily messagfe
        /// </summary>
        public async Task<int> SendWellnessMessagesToAllEligibleUsersAsync()
        {
            int sentCount = 0;
            var userProfiles = await _userRepository.GetAllUserProfilesWithSmsEnabled();
            
            foreach (var profile in userProfiles)
            {
                if (string.IsNullOrEmpty(profile.PhoneNumber))
                    continue;
                    

                string[] topics = null;
                if (!string.IsNullOrEmpty(profile.SelectedTopics))
                {
                    try
                    {
                        topics = JsonSerializer.Deserialize<string[]>(profile.SelectedTopics);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error deserializing selected topics for user {profile.UserId}");
                    }
                }
                
                var success = await _smsService.SendDailyWellnessMessageAsync(profile.PhoneNumber, topics);
                if (success)
                {
                    sentCount++;
                }
                
                await Task.Delay(100); 
            }
            
            _logger.LogInformation($"Sent wellness messages to {sentCount} users");
            return sentCount;
        }
    }
}
