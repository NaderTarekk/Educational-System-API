// GetQuestionsCountQuery.cs
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetQuestionsCount
{
    public record GetQuestionsCountQuery(Guid ExamId) : IRequest<GetByIdResponseDto<int>>;
}