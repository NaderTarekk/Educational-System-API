using EducationalSystem.Domain.Entities;

namespace EducationalSystem.Domain.Interfaces
{
    public interface IQuestionRepository
    {
        // ===== READ OPERATIONS =====
        Task<Question?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Question?> GetByIdWithOptionsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Question>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<Question>> GetByExamIdAsync(Guid examId, CancellationToken cancellationToken = default);
        Task<List<Question>> GetByExamIdWithOptionsAsync(Guid examId, CancellationToken cancellationToken = default);
        Task<List<Question>> GetByTypeAsync(QuestionType type, CancellationToken cancellationToken = default);
        Task<int> GetCountByExamIdAsync(Guid examId, CancellationToken cancellationToken = default);
        Task<decimal> GetTotalMarksByExamIdAsync(Guid examId, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

        // ===== WRITE OPERATIONS =====
        Task<Question> AddAsync(Question question, CancellationToken cancellationToken = default);
        Task AddRangeAsync(List<Question> questions, CancellationToken cancellationToken = default);
        Task UpdateAsync(Question question, CancellationToken cancellationToken = default);
        Task DeleteAsync(Question question, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(List<Question> questions, CancellationToken cancellationToken = default);

        // ===== ORDERING =====
        Task<int> GetMaxOrderByExamIdAsync(Guid examId, CancellationToken cancellationToken = default);
        Task ReorderQuestionsAsync(Guid examId, List<Guid> questionIds, CancellationToken cancellationToken = default);
    }
}
