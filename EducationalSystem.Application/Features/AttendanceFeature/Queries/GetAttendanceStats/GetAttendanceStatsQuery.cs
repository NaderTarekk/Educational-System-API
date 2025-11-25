using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.AttendanceFeature.Queries.GetAttendanceStats
{
    public record GetAttendanceStatsQuery(
    Guid? GroupId = null,
    string? StudentId = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null
) : IRequest<GetByIdResponseDto<object>>;

}
