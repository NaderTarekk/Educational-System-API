using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.AttendanceFeature.Commands.UpdateAttendance
{
    public record UpdateAttendanceCommand(CreateAttendanceDto Attendance) : IRequest<ResponseMessage>;
}
