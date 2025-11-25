using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.AttendanceFeature.Commands.CreateAttendance
{
    public record CreateAttendanceCommand(BulkAttendanceDto Attendance) : IRequest<ResponseMessage>;
}
