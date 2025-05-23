﻿using EmocineSveikataServer.Dto.RoomDtos;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Repositories.ProfileRepository;
using EmocineSveikataServer.Repositories.UserRepository;

namespace EmocineSveikataServer.Services.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly ISpecialistTimeSlotRepository _specialistTimeSlotRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISpecialistProfileRepository _specialistProfileRepository;

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
    }
}
