using EmocineSveikataServer.Dto;
using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Services.NotificationService
{
	public interface INotificationService
	{
		Task CreateNotificationAsync(string message, int userId, string link = "");
		Task<List<NotificationDto>?> GetNotificationsAsync(int userId);
		Task MarkAllAsReadAsync(int userId);
	}
}