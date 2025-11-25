using EducationalSystem.Application.Features.ExamFeature.Commands.CreateExam;
using EducationalSystem.Application.Features.ExamFeature.Commands.DeleteExam;
using EducationalSystem.Application.Features.ExamFeature.Commands.DeleteExamById;
using EducationalSystem.Application.Features.ExamFeature.Commands.StartExam;
using EducationalSystem.Application.Features.ExamFeature.Commands.SubmitAnswer;
using EducationalSystem.Application.Features.ExamFeature.Commands.SubmitExam;
using EducationalSystem.Application.Features.ExamFeature.Commands.UpdateExam;
using EducationalSystem.Application.Features.ExamFeature.Queries.ExamExists;
using EducationalSystem.Application.Features.ExamFeature.Queries.GetActiveExams;
using EducationalSystem.Application.Features.ExamFeature.Queries.GetAllExams;
using EducationalSystem.Application.Features.ExamFeature.Queries.GetExamById;
using EducationalSystem.Application.Features.ExamFeature.Queries.GetExamByIdWithQuestions;
using EducationalSystem.Application.Features.ExamFeature.Queries.GetExamByIdWithQuestionsAndOptions;
using EducationalSystem.Application.Features.ExamFeature.Queries.GetExamsByDateRange;
using EducationalSystem.Application.Features.ExamFeature.Queries.GetExamsByGroupId;
using EducationalSystem.Application.Features.ExamFeature.Queries.GetExamsByGroupIdWithQuestions;
using EducationalSystem.Application.Features.ExamFeature.Queries.GetQuestionsCount;
using EducationalSystem.Application.Features.ExamFeature.Queries.GetStudentExam;
using EducationalSystem.Application.Features.ExamFeature.Queries.GetStudentExamResult;
using EducationalSystem.Application.Features.ExamFeature.Queries.HasStudentStartedExam;
using EducationalSystem.Application.Features.ExamFeature.Queries.IsExamAvailableForStudent;
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExamsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ===== READ OPERATIONS =====

        /// <summary>
        /// جلب امتحان بواسطة المعرف
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Assistant,Student")]
        public async Task<IActionResult> GetExamById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetExamByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// جلب امتحان مع الأسئلة
        /// </summary>
        [HttpGet("{id}/with-questions")]
        [Authorize(Roles = "Admin,Assistant,Student")]
        public async Task<IActionResult> GetExamByIdWithQuestions(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetExamByIdWithQuestionsQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// جلب امتحان مع الأسئلة والاختيارات
        /// </summary>
        [HttpGet("{id}/with-questions-and-options")]
        [Authorize(Roles = "Admin,Assistant,Student")]
        public async Task<IActionResult> GetExamByIdWithQuestionsAndOptions(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetExamByIdWithQuestionsAndOptionsQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// جلب جميع الامتحانات
        /// </summary>
        [HttpGet("GetAllExams")]
        [Authorize(Roles = "Admin,Assistant")]
        public async Task<IActionResult> GetAllExams(CancellationToken cancellationToken)
        {
            var query = new GetAllExamsQuery();
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// جلب امتحانات مجموعة معينة
        /// </summary>
        [HttpGet("group/{groupId}")]
        [Authorize(Roles = "Admin,Assistant,Student")]
        public async Task<IActionResult> GetExamsByGroupId(Guid groupId, CancellationToken cancellationToken)
        {
            var query = new GetExamsByGroupIdQuery(groupId);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// جلب امتحانات مجموعة معينة مع الأسئلة
        /// </summary>
        [HttpGet("group/{groupId}/with-questions")]
        [Authorize(Roles = "Admin,Assistant,Student")]
        public async Task<IActionResult> GetExamsByGroupIdWithQuestions(Guid groupId, CancellationToken cancellationToken)
        {
            var query = new GetExamsByGroupIdWithQuestionsQuery(groupId);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// جلب الامتحانات النشطة
        /// </summary>
        [HttpGet("active")]
        [Authorize(Roles = "Admin,Assistant,Student")]
        public async Task<IActionResult> GetActiveExams(CancellationToken cancellationToken)
        {
            var query = new GetActiveExamsQuery();
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// جلب امتحانات في نطاق زمني محدد
        /// </summary>
        [HttpGet("date-range")]
        [Authorize(Roles = "Admin,Assistant")]
        public async Task<IActionResult> GetExamsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken cancellationToken)
        {
            var query = new GetExamsByDateRangeQuery(startDate, endDate);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// التحقق من وجود امتحان
        /// </summary>
        [HttpGet("{id}/exists")]
        [Authorize(Roles = "Admin,Assistant,Student")]
        public async Task<IActionResult> ExamExists(Guid id, CancellationToken cancellationToken)
        {
            var query = new ExamExistsQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// جلب عدد الأسئلة في امتحان
        /// </summary>
        [HttpGet("{examId}/questions-count")]
        [Authorize(Roles = "Admin,Assistant,Student")]
        public async Task<IActionResult> GetQuestionsCount(Guid examId, CancellationToken cancellationToken)
        {
            var query = new GetQuestionsCountQuery(examId);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// التحقق من توفر الامتحان للطالب
        /// </summary>
        [HttpGet("{examId}/available-for-student")]
        [Authorize(Roles = "Admin,Assistant,Student")]
        public async Task<IActionResult> IsExamAvailableForStudent(Guid examId, CancellationToken cancellationToken)
        {
            var query = new IsExamAvailableForStudentQuery(examId);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// التحقق من بدء الطالب للامتحان
        /// </summary>
        [HttpGet("{examId}/student/{studentId}/started")]
        [Authorize(Roles = "Admin,Assistant,Student")]
        public async Task<IActionResult> HasStudentStartedExam(Guid examId, string studentId, CancellationToken cancellationToken)
        {
            var query = new HasStudentStartedExamQuery(examId, studentId);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // ===== WRITE OPERATIONS =====

        /// <summary>
        /// إنشاء امتحان جديد
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Assistant")]
        public async Task<IActionResult> CreateExam([FromBody] ExamDto exam, CancellationToken cancellationToken)
        {
            var command = new CreateExamCommand(exam);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetExamById), new { id = exam.Id }, result);
        }

        /// <summary>
        /// تحديث امتحان
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Assistant")]
        public async Task<IActionResult> UpdateExam(Guid id, [FromBody] Exam exam, CancellationToken cancellationToken)
        {
            if (id != exam.Id)
                return BadRequest(new { success = false, message = "معرف الامتحان غير متطابق" });

            var command = new UpdateExamCommand(exam);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// حذف امتحان بواسطة الكائن
        /// </summary>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteExam([FromBody] Exam exam, CancellationToken cancellationToken)
        {
            var command = new DeleteExamCommand(exam);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// حذف امتحان بواسطة المعرف
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteExamById(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteExamByIdCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.success)
                return BadRequest(result);

            return Ok(result);
        }

        // ===== STUDENT EXAM OPERATIONS =====

        /// <summary>
        /// بدء الامتحان للطالب
        /// </summary>
        [HttpPost("{examId}/start")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StartExam(Guid examId, CancellationToken cancellationToken)
        {
            // جلب الـ StudentId من الـ Token
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(studentId))
                return Unauthorized(new { success = false, message = "غير مصرح لك بهذا الإجراء" });

            var command = new StartExamCommand(examId, studentId);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// جلب امتحان الطالب (الأسئلة والإجابات)
        /// </summary>
        [HttpGet("{examId}/student-exam")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetStudentExam(Guid examId, CancellationToken cancellationToken)
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(studentId))
                return Unauthorized(new { success = false, message = "غير مصرح لك بهذا الإجراء" });

            var query = new GetStudentExamQuery(examId, studentId);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// إرسال إجابة سؤال
        /// </summary>
        [HttpPost("submit-answer")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerDto answerDto, CancellationToken cancellationToken)
        {
            var answer = new StudentAnswer
            {
                StudentExamId = answerDto.StudentExamId,
                QuestionId = answerDto.QuestionId,
                SelectedOptionId = answerDto.SelectedOptionId,
                AnswerText = answerDto.AnswerText
            };

            var command = new SubmitAnswerCommand(answer);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// تسليم الامتحان
        /// </summary>
        [HttpPost("submit/{studentExamId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitExam(Guid studentExamId, CancellationToken cancellationToken)
        {
            var command = new SubmitExamCommand(studentExamId);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// جلب نتيجة الامتحان
        /// </summary>
        [HttpGet("result/{studentExamId}")]
        [Authorize(Roles = "Admin,Assistant,Student")]
        public async Task<IActionResult> GetStudentExamResult(Guid studentExamId, CancellationToken cancellationToken)
        {
            var query = new GetStudentExamResultQuery(studentExamId);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}