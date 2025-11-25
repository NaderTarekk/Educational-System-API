namespace EducationalSystem.Domain.Entities.DTOs
{
    public class CreateExamDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid GroupId { get; set; }
        public int Duration { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal PassingMarks { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; }
    }

    public class UpdateExamDto
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
    }

    public class CreateQuestionDto
    {
        public string QuestionText { get; set; }
        public QuestionType Type { get; set; }
        public decimal Marks { get; set; }
        public int Order { get; set; }
        public List<CreateQuestionOptionDto> Options { get; set; }
    }

    public class UpdateQuestionDto : CreateQuestionDto { }

    public class CreateQuestionOptionDto
    {
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }
        public int Order { get; set; }
    }

    public class SaveAnswerDto
    {
        public Guid QuestionId { get; set; }
        public Guid? SelectedOptionId { get; set; }
        public string AnswerText { get; set; }
    }
}
