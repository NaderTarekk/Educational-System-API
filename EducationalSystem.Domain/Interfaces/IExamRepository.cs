using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;

namespace EducationalSystem.Domain.Interfaces
{
    public interface IExamRepository
    {
        // ===== READ OPERATIONS =====
        Task<GetByIdResponseDto<ExamDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<GetByIdResponseDto<Exam>> GetByIdWithQuestionsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<GetByIdResponseDto<Exam>> GetByIdWithQuestionsAndOptionsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<GetByIdResponseDto<List<ExamDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<GetByIdResponseDto<List<Exam>>> GetByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default);
        Task<GetByIdResponseDto<List<Exam>>> GetByGroupIdWithQuestionsAsync(Guid groupId, CancellationToken cancellationToken = default);
        Task<GetByIdResponseDto<List<Exam>>> GetActiveExamsAsync(CancellationToken cancellationToken = default);
        Task<GetByIdResponseDto<List<Exam>>> GetExamsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<GetByIdResponseDto<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<GetByIdResponseDto<int>> GetQuestionsCountAsync(Guid examId, CancellationToken cancellationToken = default);

        // ===== WRITE OPERATIONS =====
        Task<ResponseMessage> AddAsync(ExamDto exam, CancellationToken cancellationToken = default);
        Task<ResponseMessage> UpdateAsync(Exam exam, CancellationToken cancellationToken = default);
        Task<ResponseMessage> DeleteAsync(Exam exam, CancellationToken cancellationToken = default);
        Task<ResponseMessage> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // ===== VALIDATION =====
        Task<GetByIdResponseDto<bool>> IsExamAvailableForStudentAsync(Guid examId, CancellationToken cancellationToken = default);
        Task<GetByIdResponseDto<bool>> HasStudentStartedExamAsync(Guid examId, string studentId, CancellationToken cancellationToken = default);

        // ===== STUDENT EXAM OPERATIONS =====
        Task<ResponseMessage> StartExamAsync(Guid examId, string studentId, CancellationToken cancellationToken = default);
        Task<GetByIdResponseDto<StudentExam>> GetStudentExamAsync(Guid examId, string studentId, CancellationToken cancellationToken = default);
        Task<ResponseMessage> SubmitAnswerAsync(StudentAnswer answer, CancellationToken cancellationToken = default);
        Task<ResponseMessage> SubmitExamAsync(Guid studentExamId, CancellationToken cancellationToken = default);
        Task<GetByIdResponseDto<StudentExam>> GetStudentExamResultAsync(Guid studentExamId, CancellationToken cancellationToken = default);

    }
}
