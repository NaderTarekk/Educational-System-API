using EducationalSystem.Domain.Entities;

namespace EducationalSystem.Domain.Interfaces
{
    public interface IStudentAnswerRepository
    {
        // ===== READ OPERATIONS =====
        Task<StudentAnswer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<StudentAnswer?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<StudentAnswer>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<StudentAnswer>> GetByStudentExamIdAsync(Guid studentExamId, CancellationToken cancellationToken = default);
        Task<List<StudentAnswer>> GetByStudentExamIdWithDetailsAsync(Guid studentExamId, CancellationToken cancellationToken = default);
        Task<StudentAnswer?> GetByStudentExamAndQuestionAsync(Guid studentExamId, Guid questionId, CancellationToken cancellationToken = default);
        Task<List<StudentAnswer>> GetCorrectAnswersAsync(Guid studentExamId, CancellationToken cancellationToken = default);
        Task<List<StudentAnswer>> GetIncorrectAnswersAsync(Guid studentExamId, CancellationToken cancellationToken = default);
        Task<List<StudentAnswer>> GetUngradedAnswersAsync(Guid studentExamId, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> HasAnsweredQuestionAsync(Guid studentExamId, Guid questionId, CancellationToken cancellationToken = default);

        // ===== WRITE OPERATIONS =====
        Task<StudentAnswer> AddAsync(StudentAnswer answer, CancellationToken cancellationToken = default);
        Task AddRangeAsync(List<StudentAnswer> answers, CancellationToken cancellationToken = default);
        Task UpdateAsync(StudentAnswer answer, CancellationToken cancellationToken = default);
        Task UpdateRangeAsync(List<StudentAnswer> answers, CancellationToken cancellationToken = default);
        Task DeleteAsync(StudentAnswer answer, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(List<StudentAnswer> answers, CancellationToken cancellationToken = default);

        // ===== GRADING =====
        Task<int> GetCorrectAnswersCountAsync(Guid studentExamId, CancellationToken cancellationToken = default);
        Task<int> GetIncorrectAnswersCountAsync(Guid studentExamId, CancellationToken cancellationToken = default);
        Task<decimal> GetTotalMarksObtainedAsync(Guid studentExamId, CancellationToken cancellationToken = default);
        Task GradeAnswerAsync(Guid answerId, bool isCorrect, decimal marksObtained, CancellationToken cancellationToken = default);
    }
}
