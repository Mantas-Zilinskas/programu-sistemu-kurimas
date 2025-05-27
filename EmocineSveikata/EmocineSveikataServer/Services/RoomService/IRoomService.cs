using EmocineSveikataServer.Dto.RoomDtos;

namespace EmocineSveikataServer.Services.RoomService
{
    public interface IRoomService
    {
        Task<List<RoomDto>> GetAllCurrentRooms();
		Task<List<BookedRoomDto>> GetUserBookedRoomsAsync(int userId);
		Task<string> BookRoomAsync(int roomId, int userId);
    }
}
