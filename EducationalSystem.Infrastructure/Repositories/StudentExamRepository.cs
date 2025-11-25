using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Interfaces;
using EducationalSystem.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EducationalSystem.Infrastructure.Repositories
{
    public class StudentExamRepository : IStudentExamRepository
    {
        private readonly EducationalSystemApiContext _context;
        private readonly ILogger<StudentExamRepository> _logger;

        public StudentExamRepository(EducationalSystemApiContext context, ILogger<StudentExamRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ===== READ OPERATIONS =====

        public async Task<StudentExam?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentExams
                    .FirstOrDefaultAsync(se => se.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student exam by id: {StudentExamId}", id);
                throw;
            }
        }

        public async Task<StudentExam?> GetByIdWithExamAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentExams
                    .Include(se => se.Exam)
                    .FirstOrDefaultAsync(se => se.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student exam with exam by id: {StudentExamId}", id);
                throw;
            }
        }

        public async Task<StudentExam?> GetByIdWithAnswersAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentExams
                    .Include(se => se.Answers)
                    .FirstOrDefaultAsync(se => se.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student exam with answers by id: {StudentExamId}", id);
                throw;
            }
        }

        public async Task<StudentExam?> GetByIdWithFullDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentExams
                    .Include(se => se.Exam)
                        .ThenInclude(e => e.Questions)
                            .ThenInclude(q => q.Options)
                    .Include(se => se.Answers)
                        .ThenInclude(a => a.Question)
                            .ThenInclude(q => q.Options)
                    .FirstOrDefaultAsync(se => se.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student exam with full details by id: {StudentExamId}", id);
                throw;
            }
        }

        public async Task<List<StudentExam>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentExams
                    .OrderByDescending(se => se.StartedAt)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all student exams");
                throw;
            }
        }

        public async Task<List<StudentExam>> GetByExamIdAsync(Guid examId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentExams
                    .Where(se => se.ExamId == examId)
                    .OrderByDescending(se => se.StartedAt)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student exams by exam id: {ExamId}", examId);
                throw;
            }
        }

        public async Task<List<StudentExam>> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentExams
                    .Include(se => se.Exam)
                    .Where(se => se.StudentId == studentId)
                    .OrderByDescending(se => se.StartedAt)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student exams by student id: {StudentId}", studentId);
                throw;
            }
        }

        public async Task<StudentExam?> GetByExamAndStudentAsync(Guid examId, string studentId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentExams
                    .FirstOrDefaultAsync(se => se.ExamId == examId && se.StudentId == studentId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student exam by exam and student: {ExamId}, {StudentId}", examId, studentId);
                throw;
            }
        }

        public async Task<List<StudentExam>> GetByStatusAsync(ExamStatus status, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentExams
                    .Where(se => se.Status == status)
                    .OrderByDescending(se => se.StartedAt)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student exams by status: {Status}", status);
                throw;
            }
        }

        public async Task<List<StudentExam>> GetInProgressExamsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetByStatusAsync(ExamStatus.InProgress, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting in-progress exams");
                throw;
            }
        }

        public async Task<List<StudentExam>> GetSubmittedExamsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetByStatusAsync(ExamStatus.Submitted, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting submitted exams");
                throw;
            }
        }

        public async Task<List<StudentExam>> GetGradedExamsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetByStatusAsync(ExamStatus.Graded, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting graded exams");
                throw;
            }
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentExams.AnyAsync(se => se.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking student exam existence: {StudentExamId}", id);
                throw;
            }
        }

        public async Task<bool> HasStudentStartedExamAsync(Guid examId, string studentId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentExams
                    .AnyAsync(se => se.ExamId == examId && se.StudentId == studentId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if student started exam: {ExamId}, {StudentId}", examId, studentId);
                throw;
            }
        }

        public async Task<bool> HasStudentSubmittedExamAsync(Guid examId, string studentId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentExams
                    .AnyAsync(se => se.ExamId == examId &&
                                   se.StudentId == studentId &&
                                   (se.Status == ExamStatus.Submitted || se.Status == ExamStatus.Graded),
                            cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if student submitted exam: {ExamId}, {StudentId}", examId, studentId);
                throw;
            }
        }

        // ===== WRITE OPERATIONS =====

        public async Task<StudentExam> AddAsync(StudentExam studentExam, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.TbStudentExams.AddAsync(studentExam, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return studentExam;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding student exam");
                throw;
            }
        }

        public async Task UpdateAsync(StudentExam studentExam, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TbStudentExams.Update(studentExam);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student exam: {StudentExamId}", studentExam.Id);
                throw;
            }
        }

        public async Task DeleteAsync(StudentExam studentExam, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TbStudentExams.Remove(studentExam);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student exam: {StudentExamId}", studentExam.Id);
                throw;
            }
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var studentExam = await GetByIdAsync(id, cancellationToken);
                if (studentExam != null)
                {
                    await DeleteAsync(studentExam, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student exam by id: {StudentExamId}", id);
                throw;
            }
        }

        // ===== STATISTICS =====

        public async Task<int> GetTotalAttemptsAsync(Guid examId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentExams
                    .Where(se => se.ExamId == examId)
                    .CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total attempts: {ExamId}", examId);
                throw;
            }
        }

        public async Task<int> GetCompletedAttemptsAsync(Guid examId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TbStudentExams
                    .Where(se => se.ExamId == examId &&
                                (se.Status == ExamStatus.Submitted || se.Status == ExamStatus.Graded))
                    .CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting completed attempts: {ExamId}", examId);
                throw;
            }
        }

        public async Task<decimal> GetAverageScoreAsync(Guid examId, CancellationToken cancellationToken = default)
        {
            try
            {
                var scores = await _context.TbStudentExams
                    .Where(se => se.ExamId == examId && se.Score.HasValue)
                    .Select(se => se.Score!.Value)
                    .ToListAsync(cancellationToken);

                return scores.Any() ? scores.Average() : 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting average score: {ExamId}", examId);
                throw;
            }
        }

        public async Task<int> GetPassedCountAsync(Guid examId, CancellationToken cancellationToken = default)
        {
            try
            {
                var exam = await _context.TbExams.FindAsync(new object[] { examId }, cancellationToken);
                if (exam == null) return 0;

                return await _context.TbStudentExams
                    .Where(se => se.ExamId == examId &&
                                se.Score.HasValue &&
                                se.Score.Value >= exam.PassingMarks)
                    .CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting passed count: {ExamId}", examId);
                throw;
            }
        }

        public async Task<int> GetFailedCountAsync(Guid examId, CancellationToken cancellationToken = default)
        {
            try
            {
                var exam = await _context.TbExams.FindAsync(new object[] { examId }, cancellationToken);
                if (exam == null) return 0;

                return await _context.TbStudentExams
                    .Where(se => se.ExamId == examId &&
                                se.Score.HasValue &&
                                se.Score.Value < exam.PassingMarks)
                    .CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting failed count: {ExamId}", examId);
                throw;
            }
        }
    }
}
