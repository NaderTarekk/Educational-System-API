using EducationalSystem.Application.Features.User.Commands.AuthCommands.Login;
using EducationalSystem.Application.Features.User.Commands.AuthCommands.Register;
using EducationalSystem.Application.Features.User.Queries.Auth.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                // ✅ Extract user ID from JWT claims
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst("sub")?.Value
                          ?? User.FindFirst("id")?.Value
                          ?? User.FindFirst("userId")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "User ID not found in token"
                    });
                }

                // Optional: Extract email for logging
                var email = User.FindFirst(ClaimTypes.Email)?.Value
                         ?? User.FindFirst("Email")?.Value;

                Console.WriteLine($"🔄 Refreshing token for user: {userId}, Email: {email}");

                // ✅ Call MediatR handler
                var result = await _mediator.Send(new RefreshTokenQuery(userId));

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error refreshing token: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while refreshing token",
                    error = ex.Message
                });
            }
        }
    }
}
