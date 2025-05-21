using EmocineSveikataServer.Data;
using EmocineSveikataServer.Models;
using Microsoft.EntityFrameworkCore;

namespace EmocineSveikataServer.Repositories.NotificationRepository
{
	public class NotificationRepository : INotificationRepository
	{
		public DataContext _context;

		public NotificationRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<List<Notification>> GetNotificationsAsync(int userId)
		{
			var notifications = _context.Notifications
				.Where(n => n.UserId == userId)
				.OrderByDescending(n => n.CreatedAt);

			return await notifications.ToListAsync();
		}

		public async Task AddNotificationAsync(Notification notification)
		{
			_context.Notifications.Add(notification);
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}