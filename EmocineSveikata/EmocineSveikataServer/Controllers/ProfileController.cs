using Microsoft.AspNetCore.Mvc;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Repositories.ProfileRepository;
using EmocineSveikataServer.Dto.ProfileDtos;
using EmocineSveikataServer.Enums;
using EmocineSveikataServer.Services;
using System.Text.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmocineSveikataServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ISpecialistProfileRepository _specialistProfileRepository;
        private readonly ISpecialistTimeSlotRepository _timeSlotRepository;
        private readonly ITwilioService _twilioService;

        public ProfileController(
            IUserProfileRepository userProfileRepository,
            ISpecialistProfileRepository specialistProfileRepository,
            ISpecialistTimeSlotRepository timeSlotRepository,
            ITwilioService twilioService)
        {
            _userProfileRepository = userProfileRepository;
            _specialistProfileRepository = specialistProfileRepository;
            _timeSlotRepository = timeSlotRepository;
            _twilioService = twilioService;
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
                    : new List<string>(),
                SmsNotificationsEnabled = profile.SmsNotificationsEnabled,
                PhoneNumber = profile.PhoneNumber,
                SmsReminderTopic = profile.SmsReminderTopic,
                LastSmsReminder = profile.LastSmsReminder
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

                var validEnumValues = profileDto.SelectedTopics
                    .Where(topic => Enum.TryParse<DiscussionTagEnum>(topic, out _))
                    .ToList();

                profile.SelectedTopics = JsonSerializer.Serialize(validEnumValues);
                profile.SmsNotificationsEnabled = profileDto.SmsNotificationsEnabled;
                profile.PhoneNumber = profileDto.PhoneNumber;
                profile.SmsReminderTopic = profileDto.SmsReminderTopic;
                
                if (profile.SmsNotificationsEnabled && !string.IsNullOrEmpty(profile.PhoneNumber))
                {
                    string welcomeMessage = "Sveiki! Jūs sėkmingai užsiprenumeravote Emocinės Sveikatos priminimus. Jūs gausite reguliarius priminimus apie psichinę sveikatą.";
                    await _twilioService.SendSmsAsync(profile.PhoneNumber, welcomeMessage);
                }
                
                profile = await _userProfileRepository.UpdateUserProfile(profile);
            }
            else
            {
                var validEnumValues = profileDto.SelectedTopics
                    .Where(topic => Enum.TryParse<DiscussionTagEnum>(topic, out _))
                    .ToList();

                profile = new UserProfile
                {
                    UserId = profileDto.UserId,
                    ProfilePicture = profileDto.ProfilePicture,
                    SelectedTopics = JsonSerializer.Serialize(validEnumValues),
                    SmsNotificationsEnabled = profileDto.SmsNotificationsEnabled,
                    PhoneNumber = profileDto.PhoneNumber,
                    SmsReminderTopic = profileDto.SmsReminderTopic,
                    UpdatedAt = DateTime.UtcNow
                };
                
                if (profile.SmsNotificationsEnabled && !string.IsNullOrEmpty(profile.PhoneNumber))
                {
                    string welcomeMessage = "Sveiki! Jūs sėkmingai užsiprenumeravote Emocinės Sveikatos priminimus. Jūs gausite reguliarius priminimus apie psichinę sveikatą.";
                    await _twilioService.SendSmsAsync(profile.PhoneNumber, welcomeMessage);
                }
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

                var validEnumValues = profileDto.SelectedTopics
                    .Where(topic => Enum.TryParse<DiscussionTagEnum>(topic, out _))
                    .ToList();

                profile.SelectedTopics = JsonSerializer.Serialize(validEnumValues);
                profile = await _specialistProfileRepository.UpdateSpecialistProfile(profile);
            }
            else
            {
                var validEnumValues = profileDto.SelectedTopics
                    .Where(topic => Enum.TryParse<DiscussionTagEnum>(topic, out _))
                    .ToList();

                profile = new SpecialistProfile
                {
                    UserId = profileDto.UserId,
                    ProfilePicture = profileDto.ProfilePicture,
                    Bio = profileDto.Bio,
                    SelectedTopics = JsonSerializer.Serialize(validEnumValues),
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
                BookedByUserId = ts.BookedByUserId,
                MeetLink = ts.MeetLink
            }).ToList();

            return Ok(timeSlotDtos);
        }

        [HttpPost("specialist/timeslot")]
        public async Task<ActionResult<SpecialistTimeSlotDto>> CreateTimeSlot(SpecialistTimeSlotDto timeSlotDto)
        {
            var specialistProfileExists = await _specialistProfileRepository.SpecialistProfileExists(timeSlotDto.UserId);
            if (!specialistProfileExists)
            {
                return BadRequest(new { message = "Specialist profile not found. Please complete your profile before adding time slots." });
            }

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
            var existingTimeSlot = await _timeSlotRepository.GetTimeSlotById(id);
            if (existingTimeSlot == null)
            {
                return NotFound(new { message = "Time slot not found" });
            }

            await _timeSlotRepository.DeleteTimeSlot(id);
            return Ok(new { message = "Time slot deleted successfully" });
        }
        
        [HttpPost("user/test-sms")]
        public async Task<ActionResult> SendTestSms([FromBody] TestSmsDto testSmsDto)
        {
            if (string.IsNullOrEmpty(testSmsDto.PhoneNumber))
            {
                return BadRequest(new { message = "Phone number is required" });
            }
            
            string testMessage = "Tai yra bandomoji žinutė iš Emocinės Sveikatos programėlės. Jūsų SMS pranešimai veikia tinkamai!";
            
            bool success = await _twilioService.SendSmsAsync(testSmsDto.PhoneNumber, testMessage);
            
            if (success)
            {
                return Ok(new { message = "Test SMS sent successfully" });
            }
            else
            {
                return StatusCode(500, new { message = "Failed to send test SMS. Please check Twilio configuration and phone number format." });
            }
        }
    }
}
