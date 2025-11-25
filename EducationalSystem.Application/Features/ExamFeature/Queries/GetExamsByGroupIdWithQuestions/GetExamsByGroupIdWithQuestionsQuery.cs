// GetExamsByGroupIdWithQuestionsQuery.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetExamsByGroupIdWithQuestions
{
    public record GetExamsByGroupIdWithQuestionsQuery(Guid GroupId) : IRequest<GetByIdResponseDto<List<Exam>>>;
}