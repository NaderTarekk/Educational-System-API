using EducationalSystem.Application.Features.User.Commands.GroupCommands.AddStudentsToGroup;
using EducationalSystem.Application.Features.User.Commands.GroupCommands.CreateNewGroup;
using EducationalSystem.Application.Features.User.Commands.GroupCommands.DeleteGroup;
using EducationalSystem.Application.Features.User.Commands.GroupCommands.DeleteStudentFromGroup;
using EducationalSystem.Application.Features.User.Commands.GroupCommands.UpdateGroup;
using EducationalSystem.Application.Features.User.Queries.GroupQueries.GetAllGroups;
using EducationalSystem.Application.Features.User.Queries.GroupQueries.GetGroupById;
using EducationalSystem.Application.Features.User.Queries.GroupQueries.GetStudentsByGroupId;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalSystem.API.Controllers
{
    [Authorize(Roles = "Admin,Assistant")]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateNewGroup")]
        public async Task<IActionResult> CreateNewGroup([FromBody] CreateNewGroupCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("AddStudentsToGroup/{groupId}")]
        public async Task<IActionResult> AddStudentsToGroup(string groupId, [FromBody] List<string> studentIds)
        {
            if (string.IsNullOrEmpty(groupId) || studentIds == null || !studentIds.Any())
                return BadRequest(new { message = "بيانات غير صحيحة" });

            var command = new AddStudentsToGroupCommand(groupId, studentIds);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("DeleteGroup/{id}")]
        public async Task<IActionResult> DeleteGroup(string id)
        {
            var command = new DeleteGroupCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("DeleteStudentFromGroup/{groupId}/{studentId}")]
        public async Task<IActionResult> RemoveStudentFromGroup(string groupId, string studentId)
        {
            if (string.IsNullOrEmpty(groupId) || string.IsNullOrEmpty(studentId))
                return BadRequest(new { message = "بيانات غير صحيحة" });

            var command = new DeleteStudentFromGroupCommand(groupId, studentId);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("GetAllGroups")]
        public async Task<IActionResult> GetAllGroups()
        {
            var result = await _mediator.Send(new GetAllGroupQuery());
            return Ok(result);
        }

        [HttpPut("UpdateGroup")]
        public async Task<IActionResult> UpdateGroup(UpdateGroupDto group)
        {
            if (group == null)
                return BadRequest(new { message = "بيانات غير صحيحة" });
            var command = new UpdateGroupCommand(group);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("GetGroupById/{id}")]
        public async Task<IActionResult> GetGroupById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { message = "بيانات غير صحيحة" });
            var command = new GetGroupByIdQuery(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("GetStudentsByGroupId/{id}")]
        public async Task<IActionResult> GetStudentsByGroupId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { message = "بيانات غير صحيحة" });
            var command = new GetStudentsByGroupIdQuery(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
