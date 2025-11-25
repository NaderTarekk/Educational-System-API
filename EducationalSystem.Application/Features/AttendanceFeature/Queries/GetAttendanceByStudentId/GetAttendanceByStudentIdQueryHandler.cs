using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces.EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.AttendanceFeature.Queries.GetAttendanceByStudentId
{
    public class GetAttendanceByStudentIdQueryHandler
       : IRequestHandler<GetAttendanceByStudentIdQuery, GetByIdResponseDto<List<Attendance>>>
    {
        private readonly IAttendance _attendance;

        public GetAttendanceByStudentIdQueryHandler(IAttendance attendance)
        {
            _attendance = attendance;
        }

        public async Task<GetByIdResponseDto<List<Attendance>>> Handle(
            GetAttendanceByStudentIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _attendance.GetAttendancesByStudentIdAsync(
                request.StudentId,
                request.GroupId,
                request.StartDate,
                request.EndDate
            );
        }
    }
}
