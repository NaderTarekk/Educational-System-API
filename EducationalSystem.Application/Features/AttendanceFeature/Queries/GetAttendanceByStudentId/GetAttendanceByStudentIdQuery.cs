using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.AttendanceFeature.Queries.GetAttendanceByStudentId
{
    public record GetAttendanceByStudentIdQuery(
        string StudentId,
        Guid? GroupId = null,
        DateTime? StartDate = null,
        DateTime? EndDate = null
    ) : IRequest<GetByIdResponseDto<List<Attendance>>>;
}
