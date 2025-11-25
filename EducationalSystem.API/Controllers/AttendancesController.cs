using EducationalSystem.Application.Features.AttendanceFeature.Commands.CreateAttendance;
using EducationalSystem.Application.Features.AttendanceFeature.Commands.UpdateAttendance;
using EducationalSystem.Application.Features.AttendanceFeature.Queries.GetAttendanceByStudentId;
using EducationalSystem.Application.Features.AttendanceFeature.Queries.GetAttendancesByGroupId;
using EducationalSystem.Application.Features.AttendanceFeature.Queries.GetAttendanceStats;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalSystem.API.Controllers
{
    [Authorize(Roles = "Admin,Assistant")]
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttendancesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/attendance/bulk
        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBulkAttendance([FromBody] BulkAttendanceDto dto)
        {
            var command = new CreateAttendanceCommand(dto);
            var result = await _mediator.Send(command);
            return result.success ? Ok(result) : BadRequest(result);
        }

        // PUT: api/attendance
        [HttpPut]
        public async Task<IActionResult> UpdateAttendance([FromBody] CreateAttendanceDto dto)
        {
            var command = new UpdateAttendanceCommand(dto);
            var result = await _mediator.Send(command);
            return result.success ? Ok(result) : BadRequest(result);
        }

        // GET: api/attendance/group/{groupId}/date/{date}
        [HttpGet("group/{groupId}/date/{date}")]
        public async Task<IActionResult> GetByGroupAndDate(Guid groupId, DateTime date)
        {
            var query = new GetAttendancesByGroupIdQuery(groupId, date);
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // GET: api/attendance/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudent(
            string studentId,
            [FromQuery] Guid? groupId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var query = new GetAttendanceByStudentIdQuery(studentId, groupId, startDate, endDate);
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // GET: api/attendance/stats
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats(
            [FromQuery] Guid? groupId = null,
            [FromQuery] string? studentId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var query = new GetAttendanceStatsQuery(groupId, studentId, startDate, endDate);
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
