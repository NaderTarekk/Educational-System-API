using EducationalSystem.Domain.Entities;

namespace EducationalSystem.Domain.Interfaces
{
    public interface IStudentExamRepository
    {
        // ===== READ OPERATIONS =====
        Task<StudentExam?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<StudentExam?> GetByIdWithExamAsync(Guid id, CancellationToken cancellationToken = default);
        Task<StudentExam?> GetByIdWithAnswersAsync(Guid id, CancellationToken cancellationToken = default);
        Task<StudentExam?> GetByIdWithFullDetailsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<StudentExam>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<StudentExam>> GetByExamIdAsync(Guid examId, CancellationToken cancellationToken = default);
        Task<List<StudentExam>> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
        Task<StudentExam?> GetByExamAndStudentAsync(Guid examId, string studentId, CancellationToken cancellationToken = default);
        Task<List<StudentExam>> GetByStatusAsync(ExamStatus status, CancellationToken cancellationToken = default);
        Task<List<StudentExam>> GetInProgressExamsAsync(CancellationToken cancellationToken = default);
        Task<List<StudentExam>> GetSubmittedExamsAsync(CancellationToken cancellationToken = default);
        Task<List<StudentExam>> GetGradedExamsAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> HasStudentStartedExamAsync(Guid examId, string studentId, CancellationToken cancellationToken = default);
        Task<bool> HasStudentSubmittedExamAsync(Guid examId, string studentId, CancellationToken cancellationToken = default);

        // ===== WRITE OPERATIONS =====
        Task<StudentExam> AddAsync(StudentExam studentExam, CancellationToken cancellationToken = default);
        Task UpdateAsync(StudentExam studentExam, CancellationToken cancellationToken = default);
        Task DeleteAsync(StudentExam studentExam, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // ===== STATISTICS =====
        Task<int> GetTotalAttemptsAsync(Guid examId, CancellationToken cancellationToken = default);
        Task<int> GetCompletedAttemptsAsync(Guid examId, CancellationToken cancellationToken = default);
        Task<decimal> GetAverageScoreAsync(Guid examId, CancellationToken cancellationToken = default);
        Task<int> GetPassedCountAsync(Guid examId, CancellationToken cancellationToken = default);
        Task<int> GetFailedCountAsync(Guid examId, CancellationToken cancellationToken = default);
    }
}
