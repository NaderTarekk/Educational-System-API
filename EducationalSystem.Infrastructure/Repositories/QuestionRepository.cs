using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Interfaces;
using EducationalSystem.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EducationalSystem.Infrastructure.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly EducationalSystemApiContext _context;
        private readonly ILogger<QuestionRepository> _logger;

        public QuestionRepository(EducationalSystemApiContext context, ILogger<QuestionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ===== READ OPERATIONS =====

        public async Task<Question?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestions
                    .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting question by id: {QuestionId}", id);
                throw;
            }
        }

        public async Task<Question?> GetByIdWithOptionsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestions
                    .Include(q => q.Options.OrderBy(o => o.Order))
                    .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting question with options by id: {QuestionId}", id);
                throw;
            }
        }

        public async Task<List<Question>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestions
                    .OrderBy(q => q.Order)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all questions");
                throw;
            }
        }

        public async Task<List<Question>> GetByExamIdAsync(Guid examId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestions
                    .Where(q => q.ExamId == examId)
                    .OrderBy(q => q.Order)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting questions by exam id: {ExamId}", examId);
                throw;
            }
        }

        public async Task<List<Question>> GetByExamIdWithOptionsAsync(Guid examId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestions
                    .Include(q => q.Options.OrderBy(o => o.Order))
                    .Where(q => q.ExamId == examId)
                    .OrderBy(q => q.Order)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting questions with options by exam id: {ExamId}", examId);
                throw;
            }
        }

        public async Task<List<Question>> GetByTypeAsync(QuestionType type, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestions
                    .Where(q => q.Type == type)
                    .OrderBy(q => q.Order)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting questions by type: {Type}", type);
                throw;
            }
        }

        public async Task<int> GetCountByExamIdAsync(Guid examId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestions
                    .Where(q => q.ExamId == examId)
                    .CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting question count by exam id: {ExamId}", examId);
                throw;
            }
        }

        public async Task<decimal> GetTotalMarksByExamIdAsync(Guid examId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestions
                    .Where(q => q.ExamId == examId)
                    .SumAsync(q => q.Marks, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total marks by exam id: {ExamId}", examId);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestions.AnyAsync(q => q.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking question existence: {QuestionId}", id);
                throw;
            }
        }

        // ===== WRITE OPERATIONS =====

        public async Task<Question> AddAsync(Question question, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.TbQuestions.AddAsync(question, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return question;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding question");
                throw;
            }
        }

        public async Task AddRangeAsync(List<Question> questions, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.TbQuestions.AddRangeAsync(questions, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding questions");
                throw;
            }
        }

        public async Task UpdateAsync(Question question, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TbQuestions.Update(question);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating question: {QuestionId}", question.Id);
                throw;
            }
        }

        public async Task DeleteAsync(Question question, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TbQuestions.Remove(question);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting question: {QuestionId}", question.Id);
                throw;
            }
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var question = await GetByIdAsync(id, cancellationToken);
                if (question != null)
                {
                    await DeleteAsync(question, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting question by id: {QuestionId}", id);
                throw;
            }
        }

        public async Task DeleteRangeAsync(List<Question> questions, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TbQuestions.RemoveRange(questions);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting questions");
                throw;
            }
        }

        // ===== ORDERING =====

        public async Task<int> GetMaxOrderByExamIdAsync(Guid examId, CancellationToken cancellationToken = default)
        {
            try
            {
                var maxOrder = await _context.TbQuestions
                    .Where(q => q.ExamId == examId)
                    .MaxAsync(q => (int?)q.Order, cancellationToken);

                return maxOrder ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting max order by exam id: {ExamId}", examId);
                throw;
            }
        }

        public async Task ReorderQuestionsAsync(Guid examId, List<Guid> questionIds, CancellationToken cancellationToken = default)
        {
            try
            {
                var questions = await _context.TbQuestions
                    .Where(q => q.ExamId == examId && questionIds.Contains(q.Id))
                    .ToListAsync(cancellationToken);

                for (int i = 0; i < questionIds.Count; i++)
                {
                    var question = questions.FirstOrDefault(q => q.Id == questionIds[i]);
                    if (question != null)
                    {
                        question.Order = i + 1;
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reordering questions for exam: {ExamId}", examId);
                throw;
            }
        }
    }
}
