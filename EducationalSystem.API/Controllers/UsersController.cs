using EducationalSystem.Application.Features.User.Commands.UserCommands.CreateUser;
using EducationalSystem.Application.Features.User.Commands.UserCommands.DeleteUser;
using EducationalSystem.Application.Features.User.Commands.UserCommands.UpdateUser;
using EducationalSystem.Application.Features.User.Queries.UserQueries.GetAllUsers;
using EducationalSystem.Application.Features.User.Queries.UserQueries.GetUserById;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalSystem.API.Controllers
{
    [Authorize(Roles = "Admin,Assistant")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto user)
        {
            if (user == null)
                return BadRequest(new { message = "بيانات غير صحيحة" });

            var command = new CreateUserCommand(user);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var command = new DeleteUserCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllUsersQuery(pageNumber, pageSize));
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] CreateUserDto User)
        {
            if (User == null)
                return BadRequest(new { message = "بيانات غير صحيحة" });
            var command = new UpdateteUserCommand(User);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("profile/{id}")]
        public async Task<IActionResult> Profile(string id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));
            return Ok(result);
        }
    }
}
