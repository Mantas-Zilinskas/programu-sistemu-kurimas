using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Services.SmsService
{
    public interface ISmsService
    {
        Task<bool> SendSms(string phoneNumber, string message);
        Task<bool> SendScheduledMessage(UserProfile userProfile);
        Task<bool> SendTestMessage(UserProfile userProfile);
        string GenerateMessageForTopics(List<string> topics);
    }
}
