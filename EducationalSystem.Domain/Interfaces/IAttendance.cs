using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;

namespace EducationalSystem.Domain.Interfaces
{
    // IAttendance.cs
    namespace EducationalSystem.Domain.Interfaces
    {
        public interface IAttendance
        {
            Task<ResponseMessage> CreateBulkAttendanceAsync(BulkAttendanceDto dto);
            Task<ResponseMessage> UpdateAttendanceAsync(CreateAttendanceDto attendance);
            Task<GetByIdResponseDto<List<Attendance>>> GetAttendancesByGroupAndDateAsync(Guid groupId, DateTime date);
            Task<GetByIdResponseDto<List<Attendance>>> GetAttendancesByStudentIdAsync(string studentId, Guid? groupId, DateTime? startDate, DateTime? endDate);
            Task<GetByIdResponseDto<object>> GetAttendanceStatsAsync(AttendanceReportDto report);
        }
    }
}
