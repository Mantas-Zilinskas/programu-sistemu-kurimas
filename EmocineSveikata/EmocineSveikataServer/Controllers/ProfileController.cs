using Microsoft.AspNetCore.Mvc;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Repositories.ProfileRepository;
using EmocineSveikataServer.Dto.ProfileDtos;
using System.Text.Json;

namespace EmocineSveikataServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ISpecialistProfileRepository _specialistProfileRepository;
        private readonly ISpecialistTimeSlotRepository _timeSlotRepository;

        public ProfileController(
            IUserProfileRepository userProfileRepository,
            ISpecialistProfileRepository specialistProfileRepository,
            ISpecialistTimeSlotRepository timeSlotRepository)
        {
            _userProfileRepository = userProfileRepository;
            _specialistProfileRepository = specialistProfileRepository;
            _timeSlotRepository = timeSlotRepository;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(int userId)
        {
            var profile = await _userProfileRepository.GetUserProfileByUserId(userId);
            if (profile == null)
            {
                return NotFound(new { message = "User profile not found" });
            }

            var profileDto = new UserProfileDto
            {
                UserId = profile.UserId,
                ProfilePicture = profile.ProfilePicture,
                SelectedTopics = profile.SelectedTopics != null 
                    ? JsonSerializer.Deserialize<List<string>>(profile.SelectedTopics) 
                    : new List<string>()
            };

            return Ok(profileDto);
        }

        [HttpPost("user")]
        public async Task<ActionResult<UserProfileDto>> CreateOrUpdateUserProfile(UserProfileDto profileDto)
        {
            var exists = await _userProfileRepository.UserProfileExists(profileDto.UserId);
            UserProfile profile;

            if (exists)
            {
                profile = await _userProfileRepository.GetUserProfileByUserId(profileDto.UserId);
                profile.ProfilePicture = profileDto.ProfilePicture;
                profile.SelectedTopics = JsonSerializer.Serialize(profileDto.SelectedTopics);
                profile = await _userProfileRepository.UpdateUserProfile(profile);
            }
            else
            {
                profile = new UserProfile
                {
                    UserId = profileDto.UserId,
                    ProfilePicture = profileDto.ProfilePicture,
                    SelectedTopics = JsonSerializer.Serialize(profileDto.SelectedTopics),
                    UpdatedAt = DateTime.UtcNow
                };
                profile = await _userProfileRepository.CreateUserProfile(profile);
            }

            return Ok(profileDto);
        }

        [HttpGet("specialist/{userId}")]
        public async Task<ActionResult<SpecialistProfileDto>> GetSpecialistProfile(int userId)
        {
            var profile = await _specialistProfileRepository.GetSpecialistProfileByUserId(userId);
            if (profile == null)
            {
                return NotFound(new { message = "Specialist profile not found" });
            }

            var profileDto = new SpecialistProfileDto
            {
                UserId = profile.UserId,
                ProfilePicture = profile.ProfilePicture,
                Bio = profile.Bio,
                SelectedTopics = profile.SelectedTopics != null 
                    ? JsonSerializer.Deserialize<List<string>>(profile.SelectedTopics) 
                    : new List<string>()
            };

            return Ok(profileDto);
        }

        [HttpPost("specialist")]
        public async Task<ActionResult<SpecialistProfileDto>> CreateOrUpdateSpecialistProfile(SpecialistProfileDto profileDto)
        {
            var exists = await _specialistProfileRepository.SpecialistProfileExists(profileDto.UserId);
            SpecialistProfile profile;

            if (exists)
            {
                profile = await _specialistProfileRepository.GetSpecialistProfileByUserId(profileDto.UserId);
                profile.ProfilePicture = profileDto.ProfilePicture;
                profile.Bio = profileDto.Bio;
                profile.SelectedTopics = JsonSerializer.Serialize(profileDto.SelectedTopics);
                profile = await _specialistProfileRepository.UpdateSpecialistProfile(profile);
            }
            else
            {
                profile = new SpecialistProfile
                {
                    UserId = profileDto.UserId,
                    ProfilePicture = profileDto.ProfilePicture,
                    Bio = profileDto.Bio,
                    SelectedTopics = JsonSerializer.Serialize(profileDto.SelectedTopics),
                    UpdatedAt = DateTime.UtcNow
                };
                profile = await _specialistProfileRepository.CreateSpecialistProfile(profile);
            }

            return Ok(profileDto);
        }

        [HttpGet("specialist/{userId}/timeslots")]
        public async Task<ActionResult<List<SpecialistTimeSlotDto>>> GetSpecialistTimeSlots(int userId)
        {
            var timeSlots = await _timeSlotRepository.GetTimeSlotsBySpecialistId(userId);
            var timeSlotDtos = timeSlots.Select(ts => new SpecialistTimeSlotDto
            {
                Id = ts.Id,
                UserId = ts.UserId,
                Date = ts.Date,
                StartTime = ts.StartTime,
                EndTime = ts.EndTime,
                IsBooked = ts.IsBooked,
                BookedByUserId = ts.BookedByUserId
            }).ToList();

            return Ok(timeSlotDtos);
        }

        [HttpPost("specialist/timeslot")]
        public async Task<ActionResult<SpecialistTimeSlotDto>> CreateTimeSlot(SpecialistTimeSlotDto timeSlotDto)
        {
            var timeSlot = new SpecialistTimeSlot
            {
                UserId = timeSlotDto.UserId,
                Date = timeSlotDto.Date,
                StartTime = timeSlotDto.StartTime,
                EndTime = timeSlotDto.EndTime,
                IsBooked = timeSlotDto.IsBooked,
                BookedByUserId = timeSlotDto.BookedByUserId,
                CreatedAt = DateTime.UtcNow
            };

            var createdTimeSlot = await _timeSlotRepository.CreateTimeSlot(timeSlot);
            timeSlotDto.Id = createdTimeSlot.Id;

            return Ok(timeSlotDto);
        }

        [HttpPut("specialist/timeslot/{id}")]
        public async Task<ActionResult<SpecialistTimeSlotDto>> UpdateTimeSlot(int id, SpecialistTimeSlotDto timeSlotDto)
        {
            var existingTimeSlot = await _timeSlotRepository.GetTimeSlotById(id);
            if (existingTimeSlot == null)
            {
                return NotFound(new { message = "Time slot not found" });
            }

            existingTimeSlot.Date = timeSlotDto.Date;
            existingTimeSlot.StartTime = timeSlotDto.StartTime;
            existingTimeSlot.EndTime = timeSlotDto.EndTime;
            existingTimeSlot.IsBooked = timeSlotDto.IsBooked;
            existingTimeSlot.BookedByUserId = timeSlotDto.BookedByUserId;

            var updatedTimeSlot = await _timeSlotRepository.UpdateTimeSlot(existingTimeSlot);
            
            return Ok(timeSlotDto);
        }

        [HttpDelete("specialist/timeslot/{id}")]
        public async Task<ActionResult> DeleteTimeSlot(int id)
        {
            var success = await _timeSlotRepository.DeleteTimeSlot(id);
            if (!success)
            {
                return NotFound(new { message = "Time slot not found" });
            }

            return Ok(new { message = "Time slot deleted successfully" });
        }
    }
}
