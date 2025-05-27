using System.Threading.Tasks;

namespace EmocineSveikataServer.Services.SmsService
{
    public interface ISmsService
    {
        Task<bool> SendDailyWellnessMessageAsync(string phoneNumber, string[]? topics = null);
    }
}
