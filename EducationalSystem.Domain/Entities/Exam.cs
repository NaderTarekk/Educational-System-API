using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EducationalSystem.Domain.Entities
{
    public class Exam
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "عنوان الامتحان مطلوب")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "وصف الامتحان مطلوب")]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "معرف المجموعة مطلوب")]
        [ForeignKey("Group")]
        public Guid GroupId { get; set; }

        [Required(ErrorMessage = "مدة الامتحان مطلوبة")]
        [Range(1, 300, ErrorMessage = "مدة الامتحان يجب أن تكون بين 1 و 300 دقيقة")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "الدرجة الكلية مطلوبة")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalMarks { get; set; }

        [Required(ErrorMessage = "درجة النجاح مطلوبة")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PassingMarks { get; set; }

        [Required(ErrorMessage = "تاريخ بداية الامتحان مطلوب")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "تاريخ نهاية الامتحان مطلوب")]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "معرف منشئ الامتحان مطلوب")]
        [MaxLength(450)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties - بدون [Required]
        [JsonIgnore]
        public virtual Group? Group { get; set; }

        [JsonIgnore]
        public virtual ICollection<Question>? Questions { get; set; } = new List<Question>();

        [JsonIgnore]
        public virtual ICollection<StudentExam>? StudentExams { get; set; } = new List<StudentExam>();
    }

    // Models/Question.cs
    public enum QuestionType
    {
        MultipleChoice = 0,
        TrueFalse = 1,
        Essay = 2
    }

    public class Question
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ExamId { get; set; }
        public string QuestionText { get; set; }
        public QuestionType Type { get; set; }
        public decimal Marks { get; set; }
        public int Order { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual ICollection<QuestionOption> Options { get; set; }
    }

    // Models/QuestionOption.cs
    public class QuestionOption
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid QuestionId { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }
        public int Order { get; set; }

        public virtual Question Question { get; set; }
    }

    // Models/StudentExam.cs
    public enum ExamStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Submitted = 2,
        Graded = 3
    }

    public class StudentExam
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ExamId { get; set; }
        public string StudentId { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public decimal? Score { get; set; }
        public ExamStatus Status { get; set; } = ExamStatus.NotStarted;

        public virtual Exam Exam { get; set; }
        public virtual ICollection<StudentAnswer> Answers { get; set; }
    }

    // Models/StudentAnswer.cs
    public class StudentAnswer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StudentExamId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid? SelectedOptionId { get; set; }
        public string AnswerText { get; set; }
        public bool? IsCorrect { get; set; }
        public decimal? MarksObtained { get; set; }

        public virtual StudentExam StudentExam { get; set; }
        public virtual Question Question { get; set; }
        public virtual QuestionOption SelectedOption { get; set; }
    }
}
