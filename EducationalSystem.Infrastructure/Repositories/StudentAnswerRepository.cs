using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Interfaces;
using EducationalSystem.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EducationalSystem.Infrastructure.Repositories
{
    public class StudentAnswerRepository : IStudentAnswerRepository
    {
        private readonly EducationalSystemApiContext _context;
        private readonly ILogger<StudentAnswerRepository> _logger;

        public StudentAnswerRepository(EducationalSystemApiContext context, ILogger<StudentAnswerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ===== READ OPERATIONS =====

        public async Task<StudentAnswer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentAnswers
                    .FirstOrDefaultAsync(sa => sa.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student answer by id: {AnswerId}", id);
                throw;
            }
        }

        public async Task<StudentAnswer?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentAnswers
                    .Include(sa => sa.Question)
                        .ThenInclude(q => q.Options)
                    .Include(sa => sa.SelectedOption)
                    .FirstOrDefaultAsync(sa => sa.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student answer with details by id: {AnswerId}", id);
                throw;
            }
        }

        public async Task<List<StudentAnswer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentAnswers.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all student answers");
                throw;
            }
        }

        public async Task<List<StudentAnswer>> GetByStudentExamIdAsync(Guid studentExamId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentAnswers
                    .Where(sa => sa.StudentExamId == studentExamId)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student answers by student exam id: {StudentExamId}", studentExamId);
                throw;
            }
        }

        public async Task<List<StudentAnswer>> GetByStudentExamIdWithDetailsAsync(Guid studentExamId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentAnswers
                    .Include(sa => sa.Question)
                        .ThenInclude(q => q.Options)
                    .Include(sa => sa.SelectedOption)
                    .Where(sa => sa.StudentExamId == studentExamId)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student answers with details by student exam id: {StudentExamId}", studentExamId);
                throw;
            }
        }

        public async Task<StudentAnswer?> GetByStudentExamAndQuestionAsync(Guid studentExamId, Guid questionId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentAnswers
                    .FirstOrDefaultAsync(sa => sa.StudentExamId == studentExamId && sa.QuestionId == questionId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student answer by student exam and question: {StudentExamId}, {QuestionId}", studentExamId, questionId);
                throw;
            }
        }

        public async Task<List<StudentAnswer>> GetCorrectAnswersAsync(Guid studentExamId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentAnswers
                    .Where(sa => sa.StudentExamId == studentExamId && sa.IsCorrect == true)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting correct answers: {StudentExamId}", studentExamId);
                throw;
            }
        }

        public async Task<List<StudentAnswer>> GetIncorrectAnswersAsync(Guid studentExamId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentAnswers
                    .Where(sa => sa.StudentExamId == studentExamId && sa.IsCorrect == false)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting incorrect answers: {StudentExamId}", studentExamId);
                throw;
            }
        }

        public async Task<List<StudentAnswer>> GetUngradedAnswersAsync(Guid studentExamId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentAnswers
                    .Where(sa => sa.StudentExamId == studentExamId && sa.IsCorrect == null)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ungraded answers: {StudentExamId}", studentExamId);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentAnswers.AnyAsync(sa => sa.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking student answer existence: {AnswerId}", id);
                throw;
            }
        }

        public async Task<bool> HasAnsweredQuestionAsync(Guid studentExamId, Guid questionId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentAnswers
                    .AnyAsync(sa => sa.StudentExamId == studentExamId && sa.QuestionId == questionId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if question answered: {StudentExamId}, {QuestionId}", studentExamId, questionId);
                throw;
            }
        }

        // ===== WRITE OPERATIONS =====

        public async Task<StudentAnswer> AddAsync(StudentAnswer answer, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.TbStudentAnswers.AddAsync(answer, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return answer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding student answer");
                throw;
            }
        }

        public async Task AddRangeAsync(List<StudentAnswer> answers, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.TbStudentAnswers.AddRangeAsync(answers, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding student answers");
                throw;
            }
        }

        public async Task UpdateAsync(StudentAnswer answer, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TbStudentAnswers.Update(answer);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student answer: {AnswerId}", answer.Id);
                throw;
            }
        }

        public async Task UpdateRangeAsync(List<StudentAnswer> answers, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TbStudentAnswers.UpdateRange(answers);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student answers");
                throw;
            }
        }

        public async Task DeleteAsync(StudentAnswer answer, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TbStudentAnswers.Remove(answer);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student answer: {AnswerId}", answer.Id);
                throw;
            }
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var answer = await GetByIdAsync(id, cancellationToken);
                if (answer != null)
                {
                    await DeleteAsync(answer, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student answer by id: {AnswerId}", id);
                throw;
            }
        }

        public async Task DeleteRangeAsync(List<StudentAnswer> answers, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TbStudentAnswers.RemoveRange(answers);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student answers");
                throw;
            }
        }

        // ===== GRADING =====

        public async Task<int> GetCorrectAnswersCountAsync(Guid studentExamId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentAnswers
                    .Where(sa => sa.StudentExamId == studentExamId && sa.IsCorrect == true)
                    .CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting correct answers count: {StudentExamId}", studentExamId);
                throw;
            }
        }

        public async Task<int> GetIncorrectAnswersCountAsync(Guid studentExamId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentAnswers
                    .Where(sa => sa.StudentExamId == studentExamId && sa.IsCorrect == false)
                    .CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting incorrect answers count: {StudentExamId}", studentExamId);
                throw;
            }
        }

        public async Task<decimal> GetTotalMarksObtainedAsync(Guid studentExamId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentAnswers
                    .Where(sa => sa.StudentExamId == studentExamId && sa.MarksObtained.HasValue)
                    .SumAsync(sa => sa.MarksObtained!.Value, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total marks obtained: {StudentExamId}", studentExamId);
                throw;
            }
        }

        public async Task GradeAnswerAsync(Guid answerId, bool isCorrect, decimal marksObtained, CancellationToken cancellationToken = default)
        {
            try
            {
                var answer = await GetByIdAsync(answerId, cancellationToken);
                if (answer != null)
                {
                    answer.IsCorrect = isCorrect;
                    answer.MarksObtained = marksObtained;
                    await UpdateAsync(answer, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error grading answer: {AnswerId}", answerId);
                throw;
            }
        }
    }
}
