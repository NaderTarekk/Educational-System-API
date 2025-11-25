using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Interfaces;
using EducationalSystem.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EducationalSystem.Infrastructure.Repositories
{
    public class QuestionOptionRepository : IQuestionOptionRepository
    {
        private readonly EducationalSystemApiContext _context;
        private readonly ILogger<QuestionOptionRepository> _logger;

        public QuestionOptionRepository(EducationalSystemApiContext context, ILogger<QuestionOptionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ===== READ OPERATIONS =====

        public async Task<QuestionOption?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestionOptions
                    .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting option by id: {OptionId}", id);
                throw;
            }
        }

        public async Task<List<QuestionOption>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestionOptions
                    .OrderBy(o => o.Order)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all options");
                throw;
            }
        }

        public async Task<List<QuestionOption>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestionOptions
                    .Where(o => o.QuestionId == questionId)
                    .OrderBy(o => o.Order)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting options by question id: {QuestionId}", questionId);
                throw;
            }
        }

        public async Task<QuestionOption?> GetCorrectOptionByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestionOptions
                    .FirstOrDefaultAsync(o => o.QuestionId == questionId && o.IsCorrect, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting correct option by question id: {QuestionId}", questionId);
                throw;
            }
        }

        public async Task<List<QuestionOption>> GetCorrectOptionsByQuestionIdsAsync(List<Guid> questionIds, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestionOptions
                    .Where(o => questionIds.Contains(o.QuestionId) && o.IsCorrect)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting correct options by question ids");
                throw;
            }
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestionOptions.AnyAsync(o => o.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking option existence: {OptionId}", id);
                throw;
            }
        }

        public async Task<int> GetCountByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbQuestionOptions
                    .Where(o => o.QuestionId == questionId)
                    .CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting options count by question id: {QuestionId}", questionId);
                throw;
            }
        }

        // ===== WRITE OPERATIONS =====

        public async Task<QuestionOption> AddAsync(QuestionOption option, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.TbQuestionOptions.AddAsync(option, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return option;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding option");
                throw;
            }
        }

        public async Task AddRangeAsync(List<QuestionOption> options, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.TbQuestionOptions.AddRangeAsync(options, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding options");
                throw;
            }
        }

        public async Task UpdateAsync(QuestionOption option, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TbQuestionOptions.Update(option);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating option: {OptionId}", option.Id);
                throw;
            }
        }

        public async Task UpdateRangeAsync(List<QuestionOption> options, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TbQuestionOptions.UpdateRange(options);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating options");
                throw;
            }
        }

        public async Task DeleteAsync(QuestionOption option, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TbQuestionOptions.Remove(option);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting option: {OptionId}", option.Id);
                throw;
            }
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var option = await GetByIdAsync(id, cancellationToken);
                if (option != null)
                {
                    await DeleteAsync(option, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting option by id: {OptionId}", id);
                throw;
            }
        }

        public async Task DeleteRangeAsync(List<QuestionOption> options, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TbQuestionOptions.RemoveRange(options);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting options");
                throw;
            }
        }

        public async Task DeleteByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default)
        {
            try
            {
                var options = await GetByQuestionIdAsync(questionId, cancellationToken);
                if (options.Any())
                {
                    await DeleteRangeAsync(options, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting options by question id: {QuestionId}", questionId);
                throw;
            }
        }
    }
}