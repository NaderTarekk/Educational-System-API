using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetExamByIdWithQuestions
{
    public record GetExamByIdWithQuestionsQuery(Guid Id) : IRequest<GetByIdResponseDto<Exam>>;
}
