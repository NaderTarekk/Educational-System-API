using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces.EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.AttendanceFeature.Commands.CreateAttendance
{
    public class CreateAttendanceCommandHandler : IRequestHandler<CreateAttendanceCommand, ResponseMessage>
    {
        private readonly IAttendance _attendance;

        public CreateAttendanceCommandHandler(IAttendance attendance)
        {
            _attendance = attendance;
        }

        public async Task<ResponseMessage> Handle(CreateAttendanceCommand request, CancellationToken cancellationToken)
        {
            return await _attendance.CreateBulkAttendanceAsync(request.Attendance);
        }
    }
}
