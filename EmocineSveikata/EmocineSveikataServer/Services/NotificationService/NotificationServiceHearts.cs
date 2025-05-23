using EmocineSveikataServer.Dto;

namespace EmocineSveikataServer.Services.NotificationService
{
    public class NotificationServiceHearts : INotificationService
    {
        private readonly NotificationService _originalNotificationService;

        public NotificationServiceHearts(NotificationService originalNotificationService)
        {
            _originalNotificationService = originalNotificationService;
        }

        public async Task CreateNotificationAsync(string message, int userId, string link = "")
        {
            await _originalNotificationService.CreateNotificationAsync(message, userId, link);
        }

        public async Task<List<NotificationDto>?> GetNotificationsAsync(int userId)
        {
            var notificationDtos = await _originalNotificationService.GetNotificationsAsync(userId);

            if(notificationDtos != null)
            {
                foreach(var notification in notificationDtos)
                {
                    notification.Message = $"❤️{notification.Message}❤️";
                }
            }

            return notificationDtos;
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            await _originalNotificationService.MarkAllAsReadAsync(userId);
        }
    }
}
