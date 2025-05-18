using Microsoft.AspNetCore.Mvc;
using EmocineSveikataServer.Services.PositiveMessageService;
using EmocineSveikataServer.Repositories.ProfileRepository;
using EmocineSveikataServer.Dto.PositiveMessageDtos;

namespace EmocineSveikataServer.Controllers
{
    [Route("api/positiveMessages")]
    [ApiController]
    public class PositiveMessageController : ControllerBase
    {
        private readonly IPositiveMessageService _positiveMessageService;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ISpecialistProfileRepository _specialistProfileRepository;

        public PositiveMessageController(IPositiveMessageService positiveMessageService, IUserProfileRepository userProfileRepository, ISpecialistProfileRepository specialistProfileRepository)
        {
            _positiveMessageService = positiveMessageService;
            _userProfileRepository = userProfileRepository;
            _specialistProfileRepository = specialistProfileRepository;
        }

        [HttpGet("random")]
        public async Task<ActionResult<PositiveMessageDto>> GetRandomPositiveMessage()
        {
            var positiveMessageDto = await _positiveMessageService.GetRandomMessage();

            return Ok(positiveMessageDto);
        }

        [HttpGet("{userId}/random")]
        public async Task<ActionResult<PositiveMessageDto>> GetPreferredRandomMessage(int userId)
        {
            var userProfile = await _userProfileRepository.GetUserProfileByUserId(userId);
            if(userProfile == null)
            {
                var specialistProfile = await _specialistProfileRepository.GetSpecialistProfileByUserId(userId);

                if(specialistProfile == null)
                {
                    return NotFound(new { message = "Profile not found" });
                }

                var positiveMessageDto = await _positiveMessageService.GetPreferredRandomMessage(specialistProfile.SelectedTopics);
                return Ok(positiveMessageDto);
            }
            else
            {
                var positiveMessageDto = await _positiveMessageService.GetPreferredRandomMessage(userProfile.SelectedTopics);
                return Ok(positiveMessageDto);
            }
        }
    }
}
