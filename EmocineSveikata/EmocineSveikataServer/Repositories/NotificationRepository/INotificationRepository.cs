using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Repositories.NotificationRepository
{
	public interface INotificationRepository
	{
		Task<List<Notification>> GetNotificationsAsync(int userId);
		Task AddNotificationAsync(Notification notification);
		Task SaveChangesAsync();
	}
}