using EmocineSveikataServer.Dto.RoomDtos;

namespace EmocineSveikataServer.Services.RoomService
{
    public interface IRoomService
    {
        Task<List<RoomDto>> GetAllCurrentRooms();
    }
}
