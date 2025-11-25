using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces.EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.AttendanceFeature.Commands.UpdateAttendance
{
    public class UpdateAttendanceCommandHandler : IRequestHandler<UpdateAttendanceCommand, ResponseMessage>
    {
        private readonly IAttendance _attendance;

        public UpdateAttendanceCommandHandler(IAttendance attendance)
        {
            _attendance = attendance;
        }

        public async Task<ResponseMessage> Handle(UpdateAttendanceCommand request, CancellationToken cancellationToken)
        {
            return await _attendance.UpdateAttendanceAsync(request.Attendance);
        }
    }
}
