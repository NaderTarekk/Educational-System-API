using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.AttendanceFeature.Queries.GetAttendancesByGroupId
{
    public record GetAttendancesByGroupIdQuery(
         Guid GroupId,
         DateTime Date
     ) : IRequest<GetByIdResponseDto<List<Attendance>>>;
}
