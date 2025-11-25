namespace EducationalSystem.Domain.Entities.DTOs
{
    public class ExamDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid GroupId { get; set; }
        public string? GroupName { get; set; }
        public int Duration { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal PassingMarks { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int QuestionsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public GroupDto? Group { get; set; }
        public ICollection<QuestionDto>? Questions { get; set; } = new List<QuestionDto>();
    }

    public class GroupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class QuestionDto
    {
        public Guid Id { get; set; }
        public string QuestionText { get; set; }
        public QuestionType Type { get; set; }
        public decimal Marks { get; set; }
        public int Order { get; set; }
        public List<QuestionOptionDto> Options { get; set; }
    }

    public class QuestionOptionDto
    {
        public Guid Id { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }
        public int Order { get; set; }
    }

    public class ExamDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid GroupId { get; set; }
        public int Duration { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal PassingMarks { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }

    public class StudentExamDto
    {
        public Guid Id { get; set; }
        public Guid ExamId { get; set; }
        public string StudentId { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public decimal? Score { get; set; }
        public ExamStatus Status { get; set; }
        public int QuestionsCount { get; set; }
    }

    public class ExamResultDto
    {
        public string ExamTitle { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal PassingMarks { get; set; }
        public decimal? Score { get; set; }
        public ExamStatus Status { get; set; }
        public bool Passed { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public List<AnswerResultDto> Answers { get; set; }
    }

    public class AnswerResultDto
    {
        public string QuestionText { get; set; }
        public string YourAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public bool? IsCorrect { get; set; }
        public decimal? MarksObtained { get; set; }
        public decimal QuestionMarks { get; set; }
    }
}
