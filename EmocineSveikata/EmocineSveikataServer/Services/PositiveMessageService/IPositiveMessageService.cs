using EmocineSveikataServer.Dto.PositiveMessageDtos;

namespace EmocineSveikataServer.Services.PositiveMessageService
{
    public interface IPositiveMessageService
    {
        Task<PositiveMessageDto> GetRandomMessage();
        Task<PositiveMessageDto> GetPreferredRandomMessage(string? selectedTopics);
    }
}
