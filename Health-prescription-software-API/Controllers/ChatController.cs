namespace Health_prescription_software_API.Controllers
{
    using Health_prescription_software_API.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using System.Security.Claims;

    using static Common.Roles.RoleConstants;

    [Authorize(Roles = $"{GP}, {Patient}")]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly IChatService chatService;

        private readonly IOptions<ApiBehaviorOptions> apiBehaviorOptions;

        public ChatController(IChatService chatService,
            IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            this.chatService = chatService;
            this.apiBehaviorOptions = apiBehaviorOptions;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(string targetUserEgn)
        {
            try
            {
                var targetUserId = await chatService.GetUserIdByEgn(targetUserEgn);
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

                if (targetUserId is null)
                {
                    ModelState.AddModelError("TargetUserEgn", "User does not exist.");

                    return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var messages = await chatService.GetChatMessages(userId, targetUserId);

                return Ok(messages);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{egn}")]
        public async Task<IActionResult> GetUserDetails(string egn)
        {
            try
            {
                var userDetails = await chatService.GetUserDetailsByEgn(egn);

                return Ok(userDetails);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("CheckMessages")]
        public async Task<IActionResult> HasUnreadMessages()
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

                var hasUnreadMessages = await chatService.UserHasUnreadMessages(userId);

                return Ok(new { HasUnreadMessage = hasUnreadMessages});
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}