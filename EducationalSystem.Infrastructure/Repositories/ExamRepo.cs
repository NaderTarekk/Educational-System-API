using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using EducationalSystem.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EducationalSystem.Infrastructure.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly EducationalSystemApiContext _context;
        private readonly ILogger<ExamRepository> _logger;

        public ExamRepository(EducationalSystemApiContext context, ILogger<ExamRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ===== READ OPERATIONS =====
        public async Task<GetByIdResponseDto<ExamDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var exam = await _context.TbExams
          .Include(e => e.Questions)
              .ThenInclude(q => q.Options)
          .Include(e => e.Group)
          .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

                var examDto = new ExamDto
                {
                    Id = exam.Id,
                    Title = exam.Title,
                    Description = exam.Description,
                    GroupId = exam.GroupId,
                    Duration = exam.Duration,
                    TotalMarks = exam.TotalMarks,
                    PassingMarks = exam.PassingMarks,
                    StartDate = exam.StartDate,
                    EndDate = exam.EndDate,
                    IsActive = exam.IsActive,
                    CreatedAt = exam.CreatedAt,
                    QuestionsCount = exam.Questions?.Count ?? 0,
                    Group = exam.Group != null ? new GroupDto
                    {
                        Id = exam.Group.Id,
                        Name = exam.Group.Name
                    } : null,
                    Questions = exam.Questions?.Select(q => new QuestionDto
                    {
                        Id = q.Id,
                        QuestionText = q.QuestionText,
                        Type = q.Type,
                        Marks = q.Marks,
                        Order = q.Order,
                        Options = q.Options?.Select(o => new QuestionOptionDto
                        {
                            Id = o.Id,
                            OptionText = o.OptionText,
                            IsCorrect = o.IsCorrect,
                            Order = o.Order
                        }).OrderBy(o => o.Order).ToList()
                    }).OrderBy(q => q.Order).ToList()
                };

                return new GetByIdResponseDto<ExamDto>
                {
                    Success = true,
                    Message = "تم جلب الامتحان بنجاح",
                    Data = examDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting exam by id: {ExamId}", id);
                return new GetByIdResponseDto<ExamDto>
                {
                    Success = false,
                    Message = $"حدث خطأ أثناء جلب الامتحان: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GetByIdResponseDto<Exam>> GetByIdWithQuestionsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var exam = await _context.TbExams
                    .Include(e => e.Questions.OrderBy(q => q.Order))
                    .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

                if (exam == null)
                {
                    return new GetByIdResponseDto<Exam>
                    {
                        Success = false,
                        Message = "الامتحان غير موجود",
                        Data = null
                    };
                }

                return new GetByIdResponseDto<Exam>
                {
                    Success = true,
                    Message = "تم جلب الامتحان مع الأسئلة بنجاح",
                    Data = exam
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting exam with questions by id: {ExamId}", id);
                return new GetByIdResponseDto<Exam>
                {
                    Success = false,
                    Message = $"حدث خطأ أثناء جلب الامتحان مع الأسئلة: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GetByIdResponseDto<Exam>> GetByIdWithQuestionsAndOptionsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var exam = await _context.TbExams
                    .Include(e => e.Questions.OrderBy(q => q.Order))
                        .ThenInclude(q => q.Options.OrderBy(o => o.Order))
                    .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

                if (exam == null)
                {
                    return new GetByIdResponseDto<Exam>
                    {
                        Success = false,
                        Message = "الامتحان غير موجود",
                        Data = null
                    };
                }

                return new GetByIdResponseDto<Exam>
                {
                    Success = true,
                    Message = "تم جلب الامتحان مع الأسئلة والاختيارات بنجاح",
                    Data = exam
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting exam with full details by id: {ExamId}", id);
                return new GetByIdResponseDto<Exam>
                {
                    Success = false,
                    Message = $"حدث خطأ أثناء جلب الامتحان بالتفاصيل الكاملة: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GetByIdResponseDto<List<ExamDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var exams = await _context.TbExams
                    .Include(a => a.Group)
                    .OrderByDescending(e => e.CreatedAt)
                    .ToListAsync(cancellationToken);

                // التحويل اليدوي
                var mapped = exams.Select(exam => new ExamDto
                {
                    Id = exam.Id,
                    Title = exam.Title,
                    Description = exam.Description,
                    GroupId = exam.GroupId,
                    GroupName = exam.Group?.Name ?? string.Empty,
                    Duration = exam.Duration,
                    TotalMarks = exam.TotalMarks,
                    PassingMarks = exam.PassingMarks,
                    StartDate = exam.StartDate,
                    EndDate = exam.EndDate,
                    IsActive = exam.IsActive,
                    QuestionsCount = exam.Questions?.Count ?? 0,
                    CreatedAt = exam.CreatedAt,
                    Group = exam.Group != null ? new GroupDto
                    {
                        Id = exam.Group.Id,
                        Name = exam.Group.Name,
                        // أضف باقي properties GroupDto
                    } : null
                }).ToList();

                return new GetByIdResponseDto<List<ExamDto>>
                {
                    Success = true,
                    Message = $"تم جلب {exams.Count} امتحان بنجاح",
                    Data = mapped
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all exams");
                return new GetByIdResponseDto<List<ExamDto>>
                {
                    Success = false,
                    Message = $"حدث خطأ أثناء جلب الامتحانات: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GetByIdResponseDto<List<Exam>>> GetByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default)
        {
            try
            {
                var exams = await _context.TbExams
                    .Where(e => e.GroupId == groupId)
                    .OrderByDescending(e => e.CreatedAt)
                    .ToListAsync(cancellationToken);

                return new GetByIdResponseDto<List<Exam>>
                {
                    Success = true,
                    Message = $"تم جلب {exams.Count} امتحان للمجموعة بنجاح",
                    Data = exams
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting exams by group id: {GroupId}", groupId);
                return new GetByIdResponseDto<List<Exam>>
                {
                    Success = false,
                    Message = $"حدث خطأ أثناء جلب امتحانات المجموعة: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GetByIdResponseDto<List<Exam>>> GetByGroupIdWithQuestionsAsync(Guid groupId, CancellationToken cancellationToken = default)
        {
            try
            {
                var exams = await _context.TbExams
                    .Include(e => e.Questions)
                    .Include(e => e.Group)
                    .Where(e => e.GroupId == groupId)
                    .OrderByDescending(e => e.CreatedAt)
                    .ToListAsync(cancellationToken);

                return new GetByIdResponseDto<List<Exam>>
                {
                    Success = true,
                    Message = $"تم جلب {exams.Count} امتحان مع الأسئلة للمجموعة بنجاح",
                    Data = exams
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting exams with questions by group id: {GroupId}", groupId);
                return new GetByIdResponseDto<List<Exam>>
                {
                    Success = false,
                    Message = $"حدث خطأ أثناء جلب امتحانات المجموعة مع الأسئلة: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GetByIdResponseDto<List<Exam>>> GetActiveExamsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var now = DateTime.UtcNow;
                var exams = await _context.TbExams
                    .Where(e => e.IsActive && e.StartDate <= now && e.EndDate >= now)
                    .OrderBy(e => e.StartDate)
                    .ToListAsync(cancellationToken);

                return new GetByIdResponseDto<List<Exam>>
                {
                    Success = true,
                    Message = $"تم جلب {exams.Count} امتحان نشط بنجاح",
                    Data = exams
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active exams");
                return new GetByIdResponseDto<List<Exam>>
                {
                    Success = false,
                    Message = $"حدث خطأ أثناء جلب الامتحانات النشطة: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GetByIdResponseDto<List<Exam>>> GetExamsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            try
            {
                var exams = await _context.TbExams
                    .Where(e => e.StartDate >= startDate && e.EndDate <= endDate)
                    .OrderBy(e => e.StartDate)
                    .ToListAsync(cancellationToken);

                return new GetByIdResponseDto<List<Exam>>
                {
                    Success = true,
                    Message = $"تم جلب {exams.Count} امتحان في النطاق الزمني المحدد بنجاح",
                    Data = exams
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting exams by date range");
                return new GetByIdResponseDto<List<Exam>>
                {
                    Success = false,
                    Message = $"حدث خطأ أثناء جلب الامتحانات في النطاق الزمني: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GetByIdResponseDto<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var exists = await _context.TbExams.AnyAsync(e => e.Id == id, cancellationToken);

                return new GetByIdResponseDto<bool>
                {
                    Success = true,
                    Message = exists ? "الامتحان موجود" : "الامتحان غير موجود",
                    Data = exists
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking exam existence: {ExamId}", id);
                return new GetByIdResponseDto<bool>
                {
                    Success = false,
                    Message = $"حدث خطأ أثناء التحقق من وجود الامتحان: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<GetByIdResponseDto<int>> GetQuestionsCountAsync(Guid examId, CancellationToken cancellationToken = default)
        {
            try
            {
                var count = await _context.TbQuestions
                    .Where(q => q.ExamId == examId)
                    .CountAsync(cancellationToken);

                return new GetByIdResponseDto<int>
                {
                    Success = true,
                    Message = "تم جلب عدد الأسئلة بنجاح",
                    Data = count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting questions count for exam: {ExamId}", examId);
                return new GetByIdResponseDto<int>
                {
                    Success = false,
                    Message = $"حدث خطأ أثناء جلب عدد الأسئلة: {ex.Message}",
                    Data = 0
                };
            }
        }

        // ===== WRITE OPERATIONS =====

        public async Task<ResponseMessage> AddAsync(ExamDto exam, CancellationToken cancellationToken = default)
        {
            try
            {
                // ✅ 1. Validate GroupId exists
                var groupExists = await _context.TbGroups
                    .AnyAsync(g => g.Id == exam.GroupId, cancellationToken);

                if (!groupExists)
                {
                    return new ResponseMessage
                    {
                        success = false,
                        message = "المجموعة المحددة غير موجودة"
                    };
                }

                // ✅ 2. Map ExamDto to Exam Entity correctly
                var mapped = new Exam
                {
                    Id = Guid.NewGuid(), // Generate new ID
                    Title = exam.Title,
                    Description = exam.Description,
                    GroupId = exam.GroupId,
                    Duration = exam.Duration,
                    TotalMarks = exam.TotalMarks,
                    PassingMarks = exam.PassingMarks,
                    StartDate = exam.StartDate,
                    EndDate = exam.EndDate,
                    IsActive = exam.IsActive,
                    CreatedBy = "d25e0cb0-d097-447a-af99-0dba25525522", // من الـ token/claims
                    CreatedAt = DateTime.UtcNow
                };

                // ✅ 3. Map Questions from QuestionDto to Question Entity
                if (exam.Questions != null && exam.Questions.Any())
                {
                    mapped.Questions = exam.Questions.Select(qDto => new Question
                    {
                        Id = Guid.NewGuid(),
                        ExamId = mapped.Id, // Set foreign key
                        QuestionText = qDto.QuestionText,
                        Type = qDto.Type,
                        Marks = qDto.Marks,
                        Order = qDto.Order,

                        // ✅ 4. Map Options from QuestionOptionDto to QuestionOption Entity
                        Options = qDto.Options?.Select(oDto => new QuestionOption
                        {
                            Id = Guid.NewGuid(),
                            // QuestionId will be set by EF Core
                            OptionText = oDto.OptionText,
                            IsCorrect = oDto.IsCorrect,
                            Order = oDto.Order
                        }).ToList() ?? new List<QuestionOption>()

                    }).ToList();
                }

                await _context.TbExams.AddAsync(mapped, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return new ResponseMessage
                {
                    success = true,
                    message = "تم إضافة الامتحان بنجاح"
                };
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("FOREIGN KEY") == true)
            {
                _logger.LogError(ex, "Foreign key constraint violation");
                return new ResponseMessage
                {
                    success = false,
                    message = "المجموعة المحددة غير موجودة في النظام"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding exam");
                return new ResponseMessage
                {
                    success = false,
                    message = $"حدث خطأ أثناء إضافة الامتحان: {ex.Message}"
                };
            }
        }


        public async Task<ResponseMessage> UpdateAsync(Exam exam, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TbExams.Update(exam);
                await _context.SaveChangesAsync(cancellationToken);

                return new ResponseMessage
                {
                    success = true,
                    message = "تم تحديث الامتحان بنجاح"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating exam: {ExamId}", exam.Id);
                return new ResponseMessage
                {
                    success = false,
                    message = $"حدث خطأ أثناء تحديث الامتحان: {ex.Message}"
                };
            }
        }

        public async Task<ResponseMessage> DeleteAsync(Exam exam, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TbExams.Remove(exam);
                await _context.SaveChangesAsync(cancellationToken);

                return new ResponseMessage
                {
                    success = true,
                    message = "تم حذف الامتحان بنجاح"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exam: {ExamId}", exam.Id);
                return new ResponseMessage
                {
                    success = false,
                    message = $"حدث خطأ أثناء حذف الامتحان: {ex.Message}"
                };
            }
        }

        public async Task<ResponseMessage> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var examResponse = await GetByIdAsync(id, cancellationToken);

                if (!examResponse.Success || examResponse.Data == null)
                {
                    return new ResponseMessage
                    {
                        success = false,
                        message = "الامتحان غير موجود"
                    };
                }

                var exam = await _context.TbExams.FirstOrDefaultAsync(e => e.Id == id);

                return await DeleteAsync(exam, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exam by id: {ExamId}", id);
                return new ResponseMessage
                {
                    success = false,
                    message = $"حدث خطأ أثناء حذف الامتحان: {ex.Message}"
                };
            }
        }

        // ===== VALIDATION =====

        public async Task<GetByIdResponseDto<bool>> IsExamAvailableForStudentAsync(Guid examId, CancellationToken cancellationToken = default)
        {
            try
            {
                var examResponse = await GetByIdAsync(examId, cancellationToken);

                if (!examResponse.Success || examResponse.Data == null)
                {
                    return new GetByIdResponseDto<bool>
                    {
                        Success = false,
                        Message = "الامتحان غير موجود",
                        Data = false
                    };
                }

                var exam = examResponse.Data;
                var now = DateTime.UtcNow;
                var isAvailable = exam.IsActive && exam.StartDate <= now && exam.EndDate >= now;

                return new GetByIdResponseDto<bool>
                {
                    Success = true,
                    Message = isAvailable ? "الامتحان متاح للطالب" : "الامتحان غير متاح للطالب",
                    Data = isAvailable
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking exam availability: {ExamId}", examId);
                return new GetByIdResponseDto<bool>
                {
                    Success = false,
                    Message = $"حدث خطأ أثناء التحقق من توفر الامتحان: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<GetByIdResponseDto<bool>> HasStudentStartedExamAsync(Guid examId, string studentId, CancellationToken cancellationToken = default)
        {
            try
            {
                var hasStarted = await _context.TbStudentExams
                    .AnyAsync(se => se.ExamId == examId && se.StudentId == studentId, cancellationToken);

                return new GetByIdResponseDto<bool>
                {
                    Success = true,
                    Message = hasStarted ? "الطالب بدأ الامتحان" : "الطالب لم يبدأ الامتحان",
                    Data = hasStarted
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if student started exam: {ExamId}, {StudentId}", examId, studentId);
                return new GetByIdResponseDto<bool>
                {
                    Success = false,
                    Message = $"حدث خطأ أثناء التحقق من بدء الطالب للامتحان: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<ResponseMessage> StartExamAsync(Guid examId, string studentId, CancellationToken cancellationToken = default)
        {
            try
            {
                // تحقق من وجود الامتحان
                var examResponse = await GetByIdAsync(examId, cancellationToken);
                if (!examResponse.Success || examResponse.Data == null)
                {
                    return new ResponseMessage
                    {
                        success = false,
                        message = "الامتحان غير موجود"
                    };
                }

                var exam = examResponse.Data;

                // تحقق من توفر الامتحان
                var now = DateTime.UtcNow;
                if (!exam.IsActive || exam.StartDate > now || exam.EndDate < now)
                {
                    return new ResponseMessage
                    {
                        success = false,
                        message = "الامتحان غير متاح في الوقت الحالي"
                    };
                }

                // تحقق إذا الطالب بدأ الامتحان قبل كده
                var existingStudentExam = await _context.TbStudentExams
                    .FirstOrDefaultAsync(se => se.ExamId == examId && se.StudentId == studentId, cancellationToken);

                if (existingStudentExam != null)
                {
                    return new ResponseMessage
                    {
                        success = false,
                        message = "لقد بدأت هذا الامتحان بالفعل"
                    };
                }

                // إنشاء StudentExam جديد
                var studentExam = new StudentExam
                {
                    Id = Guid.NewGuid(),
                    ExamId = examId,
                    StudentId = studentId,
                    StartedAt = DateTime.UtcNow,
                    Status = ExamStatus.InProgress
                };

                await _context.TbStudentExams.AddAsync(studentExam, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return new ResponseMessage
                {
                    success = true,
                    message = "تم بدء الامتحان بنجاح"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting exam: {ExamId} for student: {StudentId}", examId, studentId);
                return new ResponseMessage
                {
                    success = false,
                    message = $"حدث خطأ أثناء بدء الامتحان: {ex.Message}"
                };
            }
        }

        public async Task<GetByIdResponseDto<StudentExam>> GetStudentExamAsync(Guid examId, string studentId, CancellationToken cancellationToken = default)
        {
            try
            {
                var studentExam = await _context.TbStudentExams
                    .Include(se => se.Exam)
                        .ThenInclude(e => e.Questions)
                            .ThenInclude(q => q.Options)
                    .Include(se => se.Answers)
                    .FirstOrDefaultAsync(se => se.ExamId == examId && se.StudentId == studentId, cancellationToken);

                if (studentExam == null)
                {
                    return new GetByIdResponseDto<StudentExam>
                    {
                        Success = false,
                        Message = "لم تبدأ هذا الامتحان بعد",
                        Data = null
                    };
                }

                return new GetByIdResponseDto<StudentExam>
                {
                    Success = true,
                    Message = "تم جلب بيانات الامتحان بنجاح",
                    Data = studentExam
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student exam: {ExamId} for student: {StudentId}", examId, studentId);
                return new GetByIdResponseDto<StudentExam>
                {
                    Success = false,
                    Message = $"حدث خطأ أثناء جلب بيانات الامتحان: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseMessage> SubmitAnswerAsync(StudentAnswer answer, CancellationToken cancellationToken = default)
        {
            try
            {
                // تحقق من StudentExam
                var studentExam = await _context.TbStudentExams
                    .FirstOrDefaultAsync(se => se.Id == answer.StudentExamId, cancellationToken);

                if (studentExam == null)
                {
                    return new ResponseMessage
                    {
                        success = false,
                        message = "الامتحان غير موجود"
                    };
                }

                if (studentExam.Status == ExamStatus.Submitted)
                {
                    return new ResponseMessage
                    {
                        success = false,
                        message = "تم تسليم الامتحان بالفعل"
                    };
                }

                // تحقق من السؤال
                var question = await _context.TbQuestions
                    .Include(q => q.Options)
                    .FirstOrDefaultAsync(q => q.Id == answer.QuestionId, cancellationToken);

                if (question == null)
                {
                    return new ResponseMessage
                    {
                        success = false,
                        message = "السؤال غير موجود"
                    };
                }

                // تحقق إذا الطالب جاوب على السؤال قبل كده
                var existingAnswer = await _context.TbStudentAnswers
                    .FirstOrDefaultAsync(sa => sa.StudentExamId == answer.StudentExamId
                        && sa.QuestionId == answer.QuestionId, cancellationToken);

                if (existingAnswer != null)
                {
                    // تحديث الإجابة
                    existingAnswer.SelectedOptionId = answer.SelectedOptionId;
                    existingAnswer.AnswerText = answer.AnswerText;

                    // تصحيح الإجابة
                    if (question.Type == QuestionType.MultipleChoice || question.Type == QuestionType.TrueFalse)
                    {
                        var selectedOption = question.Options.FirstOrDefault(o => o.Id == answer.SelectedOptionId);
                        existingAnswer.IsCorrect = selectedOption?.IsCorrect ?? false;
                        existingAnswer.MarksObtained = existingAnswer.IsCorrect == true ? question.Marks : 0;
                    }

                    _context.TbStudentAnswers.Update(existingAnswer);
                }
                else
                {
                    // إضافة إجابة جديدة
                    answer.Id = Guid.NewGuid();

                    // تصحيح الإجابة
                    if (question.Type == QuestionType.MultipleChoice || question.Type == QuestionType.TrueFalse)
                    {
                        var selectedOption = question.Options.FirstOrDefault(o => o.Id == answer.SelectedOptionId);
                        answer.IsCorrect = selectedOption?.IsCorrect ?? false;
                        answer.MarksObtained = answer.IsCorrect == true ? question.Marks : 0;
                    }

                    await _context.TbStudentAnswers.AddAsync(answer, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return new ResponseMessage
                {
                    success = true,
                    message = "تم حفظ الإجابة بنجاح"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting answer");
                return new ResponseMessage
                {
                    success = false,
                    message = $"حدث خطأ أثناء حفظ الإجابة: {ex.Message}"
                };
            }
        }

        public async Task<ResponseMessage> SubmitExamAsync(Guid studentExamId, CancellationToken cancellationToken = default)
        {
            try
            {
                var studentExam = await _context.TbStudentExams
                    .Include(se => se.Answers)
                    .FirstOrDefaultAsync(se => se.Id == studentExamId, cancellationToken);

                if (studentExam == null)
                {
                    return new ResponseMessage
                    {
                        success = false,
                        message = "الامتحان غير موجود"
                    };
                }

                if (studentExam.Status == ExamStatus.Submitted)
                {
                    return new ResponseMessage
                    {
                        success = false,
                        message = "تم تسليم الامتحان بالفعل"
                    };
                }

                // حساب الدرجة الكلية
                var totalScore = studentExam.Answers
                    .Where(a => a.MarksObtained.HasValue)
                    .Sum(a => a.MarksObtained.Value);

                studentExam.SubmittedAt = DateTime.UtcNow;
                studentExam.Score = totalScore;
                studentExam.Status = ExamStatus.Submitted;

                _context.TbStudentExams.Update(studentExam);
                await _context.SaveChangesAsync(cancellationToken);

                return new ResponseMessage
                {
                    success = true,
                    message = $"تم تسليم الامتحان بنجاح. درجتك: {totalScore}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting exam: {StudentExamId}", studentExamId);
                return new ResponseMessage
                {
                    success = false,
                    message = $"حدث خطأ أثناء تسليم الامتحان: {ex.Message}"
                };
            }
        }

        public async Task<GetByIdResponseDto<StudentExam>> GetStudentExamResultAsync(Guid studentExamId, CancellationToken cancellationToken = default)
        {
            try
            {
                var studentExam = await _context.TbStudentExams
                    .Include(se => se.Exam)
                    .Include(se => se.Answers)
                        .ThenInclude(a => a.Question)
                    .Include(se => se.Answers)
                        .ThenInclude(a => a.SelectedOption)
                    .FirstOrDefaultAsync(se => se.Id == studentExamId, cancellationToken);

                if (studentExam == null)
                {
                    return new GetByIdResponseDto<StudentExam>
                    {
                        Success = false,
                        Message = "الامتحان غير موجود",
                        Data = null
                    };
                }

                return new GetByIdResponseDto<StudentExam>
                {
                    Success = true,
                    Message = "تم جلب نتيجة الامتحان بنجاح",
                    Data = studentExam
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student exam result: {StudentExamId}", studentExamId);
                return new GetByIdResponseDto<StudentExam>
                {
                    Success = false,
                    Message = $"حدث خطأ أثناء جلب النتيجة: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}