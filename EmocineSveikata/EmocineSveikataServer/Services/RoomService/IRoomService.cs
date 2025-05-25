using EmocineSveikataServer.Dto.RoomDtos;

namespace EmocineSveikataServer.Services.RoomService
{
    public interface IRoomService
    {
        Task<List<RoomDto>> GetAllCurrentRooms();
        Task<string> BookRoomAsync(int roomId, int userId);
    }
}
