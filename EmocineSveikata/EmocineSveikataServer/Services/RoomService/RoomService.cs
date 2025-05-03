using EmocineSveikataServer.Dto.RoomDtos;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Repositories.ProfileRepository;
using EmocineSveikataServer.Repositories.UserRepository;

namespace EmocineSveikataServer.Services.RoomService
{
    public class RoomService : IRoomService
    {
        ISpecialistTimeSlotRepository _specialistTimeSlotRepository;
        IUserRepository _userRepository;
        ISpecialistProfileRepository _specialistProfileRepository;

        public RoomService(ISpecialistTimeSlotRepository specialistTimeSlotRepository, IUserRepository userRepository, ISpecialistProfileRepository specialistProfileRepository)
        {
            _specialistTimeSlotRepository = specialistTimeSlotRepository;
            _userRepository = userRepository;
            _specialistProfileRepository = specialistProfileRepository;
        }

        public async Task<List<RoomDto>> GetAllCurrentRooms()
        {
            List<SpecialistTimeSlot> specialistTimeSlots = await _specialistTimeSlotRepository.GetCurrentlyAvailableTimeSlots();
            List<User> users = await _userRepository.GetUsersByIds(specialistTimeSlots.Select(sts => sts.UserId).ToList());
            List<SpecialistProfile> specialistProfiles = await _specialistProfileRepository.GetSpecialistProfilesByUserIds(users.Select(s => s.Id).ToList());

            List<RoomDto> roomList = [];

            try
            {
                for(int i = 0; i < users.Count; ++i)
                {
                    RoomDto roomDto = new()
                    {
                        Id = specialistTimeSlots[i].Id,
                        SpecialistName = users[i].Username,
                        Bio = specialistProfiles[i].Bio,
                        ProfilePicture = specialistProfiles[i].ProfilePicture,
                        Date = specialistTimeSlots[i].Date,
                        StartTime = specialistTimeSlots[i].StartTime,
                        EndTime = specialistTimeSlots[i].EndTime
                    };

                    roomList.Add(roomDto);
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);

                RoomDto errorRoomDto = new()
                {
                    Id = 0,
                    SpecialistName = "SpecialistProfile bug",
                    Bio = "Bug due to SpecialistProfile not existing for a SpecialistTimeSlot, click `Išsaugoti` in the user settings (`SpecialistProfile.jsx`, line 42)"
                };

                return [errorRoomDto];
            }

            return roomList;
        }
    }
}
