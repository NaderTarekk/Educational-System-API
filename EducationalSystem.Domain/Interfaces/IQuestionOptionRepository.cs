using EducationalSystem.Domain.Entities;

namespace EducationalSystem.Domain.Interfaces
{
    public interface IQuestionOptionRepository
    {
        // ===== READ OPERATIONS =====
        Task<QuestionOption?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<QuestionOption>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<QuestionOption>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default);
        Task<QuestionOption?> GetCorrectOptionByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default);
        Task<List<QuestionOption>> GetCorrectOptionsByQuestionIdsAsync(List<Guid> questionIds, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<int> GetCountByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default);

        // ===== WRITE OPERATIONS =====
        Task<QuestionOption> AddAsync(QuestionOption option, CancellationToken cancellationToken = default);
        Task AddRangeAsync(List<QuestionOption> options, CancellationToken cancellationToken = default);
        Task UpdateAsync(QuestionOption option, CancellationToken cancellationToken = default);
        Task UpdateRangeAsync(List<QuestionOption> options, CancellationToken cancellationToken = default);
        Task DeleteAsync(QuestionOption option, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(List<QuestionOption> options, CancellationToken cancellationToken = default);
        Task DeleteByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default);
    }
}
