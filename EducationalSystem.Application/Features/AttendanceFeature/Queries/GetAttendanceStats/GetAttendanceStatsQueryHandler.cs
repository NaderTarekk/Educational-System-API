using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces.EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.AttendanceFeature.Queries.GetAttendanceStats
{
    public class GetAttendanceStatsQueryHandler
         : IRequestHandler<GetAttendanceStatsQuery, GetByIdResponseDto<object>>
    {
        private readonly IAttendance _attendance;

        public GetAttendanceStatsQueryHandler(IAttendance attendance)
        {
            _attendance = attendance;
        }

        public async Task<GetByIdResponseDto<object>> Handle(
            GetAttendanceStatsQuery request,
            CancellationToken cancellationToken)
        {
            var reportDto = new AttendanceReportDto
            {
                GroupId = request.GroupId,
                StudentId = request.StudentId,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            return await _attendance.GetAttendanceStatsAsync(reportDto);
        }
    }
}
