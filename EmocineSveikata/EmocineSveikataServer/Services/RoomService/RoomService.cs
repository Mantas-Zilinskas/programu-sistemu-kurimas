using EmocineSveikataServer.Dto.RoomDtos;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Repositories.ProfileRepository;
using EmocineSveikataServer.Repositories.UserRepository;
using EmocineSveikataServer.Services.Meets;

namespace EmocineSveikataServer.Services.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly ISpecialistTimeSlotRepository _specialistTimeSlotRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISpecialistProfileRepository _specialistProfileRepository;
        private readonly GoogleMeetService _googleMeetService;

        public RoomService(ISpecialistTimeSlotRepository specialistTimeSlotRepository, IUserRepository userRepository, 
            ISpecialistProfileRepository specialistProfileRepository, GoogleMeetService meetService)
        {
            _specialistTimeSlotRepository = specialistTimeSlotRepository;
            _userRepository = userRepository;
            _specialistProfileRepository = specialistProfileRepository;
            _googleMeetService = meetService;
        }

        public async Task<List<RoomDto>> GetAllCurrentRooms()
        {
            List<SpecialistTimeSlot> specialistTimeSlots = await _specialistTimeSlotRepository.GetCurrentlyAvailableTimeSlots();
            List<User> users = await _userRepository.GetUsersByIds(specialistTimeSlots.Select(sts => sts.UserId).ToList());
            List<SpecialistProfile> specialistProfiles = await _specialistProfileRepository.GetSpecialistProfilesByUserIds(users.Select(s => s.Id).ToList());

            List<RoomDto> roomList = [];

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

            return roomList;
        }

		public async Task<List<BookedRoomDto>> GetUserBookedRoomsAsync(int userId)
		{
			var bookedTimeSlots = await _specialistTimeSlotRepository.GetBookedTimeSlotsByUserId(userId);

			var specialistUserIds = bookedTimeSlots.Select(ts => ts.UserId).ToList();
			var specialistUsers = await _userRepository.GetUsersByIds(specialistUserIds);

			var specialistProfiles = await _specialistProfileRepository.GetSpecialistProfilesByUserIds(specialistUserIds);

			List<BookedRoomDto> bookedRooms = [];

			for (int i = 0; i < bookedTimeSlots.Count; i++)
			{
				var timeSlot = bookedTimeSlots[i];
				var specialist = specialistUsers.FirstOrDefault(u => u.Id == timeSlot.UserId);
				var profile = specialistProfiles.FirstOrDefault(p => p.UserId == timeSlot.UserId);

				var bookedRoom = new BookedRoomDto
				{
					Id = timeSlot.Id,
					SpecialistName = specialist?.Username ?? "Unknown",
					Bio = profile?.Bio ?? "",
					ProfilePicture = profile?.ProfilePicture,
					Date = timeSlot.Date,
					StartTime = timeSlot.StartTime,
					EndTime = timeSlot.EndTime,
					BookedAt = timeSlot.CreatedAt,
					MeetLink = timeSlot.MeetLink
				};

				bookedRooms.Add(bookedRoom);
			}

			return bookedRooms.OrderBy(br => br.Date).ThenBy(br => br.StartTime).ToList();
		}

		public async Task<string> BookRoomAsync(int roomId, int userId)
		{
			var timeSlot = await _specialistTimeSlotRepository.GetTimeSlotById(roomId);
			if (timeSlot == null)
			{
				throw new ArgumentException("Kambarys nerastas.");
			}

			if (timeSlot.IsBooked)
			{
				throw new InvalidOperationException("Kambarys jau yra rezervuotas.");
			}

			var slotDateTime = timeSlot.Date.Add(timeSlot.StartTime);
			if (slotDateTime <= DateTime.Now)
			{
				throw new InvalidOperationException("Negalima rezervuoti pasibaigusio kambario.");
			}

			var user = await _userRepository.GetUserById(userId);
			if (user == null)
			{
				throw new ArgumentException("Vartotojas nerastas.");
			}

			if (timeSlot.UserId == userId)
			{
				throw new InvalidOperationException("Negalima rezervuoti savo kambario.");
			}

			try
			{
				var startDateTime = timeSlot.Date.Add(timeSlot.StartTime);
				var endDateTime = timeSlot.Date.Add(timeSlot.EndTime);
				var meetLink = await _googleMeetService.CreateMeetAsync(startDateTime, endDateTime);

				timeSlot.BookedByUserId = userId;
				timeSlot.BookedByUser = user;
				timeSlot.IsBooked = true;
				timeSlot.MeetLink = meetLink;

				await _specialistTimeSlotRepository.UpdateTimeSlot(timeSlot);

				return meetLink;
			}
			catch (Exception ex)
			{
				throw new Exception($"Nepavyko rezervuoti: {ex.Message}", ex);
			}
		}
	}
}
