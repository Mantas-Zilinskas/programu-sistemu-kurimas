using AutoMapper;
using EmocineSveikataServer.Dto;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Repositories.NotificationRepository;
using Microsoft.EntityFrameworkCore;

namespace EmocineSveikataServer.Services.NotificationService
{
	public class NotificationService : INotificationService
	{
		private INotificationRepository _repository;
		private IMapper _mapper;

		public NotificationService(INotificationRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task CreateNotificationAsync(string message, int userId, string link = "")
		{
			var notification = new Notification
			{
				UserId = userId,
				Message = message,
				Link = link,
				CreatedAt = DateTime.Now
			};

			await _repository.AddNotificationAsync(notification);
		}

		public async Task<List<NotificationDto>?> GetNotificationsAsync(int userId)
		{
			var notifications = await _repository.GetNotificationsAsync(userId);
			return _mapper.Map<List<NotificationDto>>(notifications);
		}

		public async Task MarkAllAsReadAsync(int userId)
		{
			var notifications = await _repository.GetNotificationsAsync(userId);
			notifications.ForEach(n => n.IsRead = true);
			await _repository.SaveChangesAsync();
		}
	}
}