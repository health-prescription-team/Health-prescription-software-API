namespace Health_prescription_software_API.Controllers
{
    using Health_prescription_software_API.Contracts;
    using Health_prescription_software_API.Contracts.Validations;
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
        private readonly IChatValidation chatValidation;

        private readonly IOptions<ApiBehaviorOptions> apiBehaviorOptions;

        public ChatController(IChatService chatService,
            IChatValidation chatValidation,
            IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            this.chatService = chatService;
            this.chatValidation = chatValidation;
            this.apiBehaviorOptions = apiBehaviorOptions;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(string targetUserEgn)
        {
            try
            {
                if (!await chatValidation.IsEngValid(targetUserEgn))
                {
                    foreach (var error in chatValidation.ModelErrors)
                    {
                        ModelState.AddModelError(error.ErrorPropName!, error.ErrorMessage!);
                    }

                    return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var targetUserId = await chatService.GetUserIdByEgn(targetUserEgn);
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

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
                if (!await chatValidation.IsEngValid(egn))
                {
                    foreach (var error in chatValidation.ModelErrors)
                    {
                        ModelState.AddModelError(error.ErrorPropName!, error.ErrorMessage!);
                    }

                    return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

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