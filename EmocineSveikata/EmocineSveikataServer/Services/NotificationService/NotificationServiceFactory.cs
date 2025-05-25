namespace EmocineSveikataServer.Services.NotificationService
{
    public class NotificationServiceFactory : INotificationServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public NotificationServiceFactory(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public INotificationService Create()
        {
            var type = _configuration["NotificationSettings:Type"];
            return type switch
            {
                "Regular" => _serviceProvider.GetRequiredService<NotificationService>(),
                "Hearts" => _serviceProvider.GetRequiredService<NotificationServiceHearts>(),
                _ => throw new InvalidOperationException("Unrecognized notification type in \"appsettings.json\".NotificationSettings")
            };
        }
    }
}
