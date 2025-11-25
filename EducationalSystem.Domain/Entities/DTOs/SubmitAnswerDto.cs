using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.Domain.Entities.DTOs
{
    public class SubmitAnswerDto
    {
        public Guid StudentExamId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid? SelectedOptionId { get; set; }
        public string? AnswerText { get; set; }
    }
}
