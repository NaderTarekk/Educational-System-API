using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces.EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.AttendanceFeature.Queries.GetAttendancesByGroupId
{
    public class GetAttendancesByGroupIdQueryHandler
       : IRequestHandler<GetAttendancesByGroupIdQuery, GetByIdResponseDto<List<Attendance>>>
    {
        private readonly IAttendance _attendance;

        public GetAttendancesByGroupIdQueryHandler(IAttendance attendance)
        {
            _attendance = attendance;
        }

        public async Task<GetByIdResponseDto<List<Attendance>>> Handle(
            GetAttendancesByGroupIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _attendance.GetAttendancesByGroupAndDateAsync(request.GroupId, request.Date);
        }
    }
}
