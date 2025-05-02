using EmocineSveikataServer.Dto.PositiveMessageDtos;

namespace EmocineSveikataServer.Services.PositiveMessageService
{
    public interface IPositiveMessageService
    {
        PositiveMessageDto GetRandomMessage();
    }
}
