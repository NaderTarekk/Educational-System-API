using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetExamByIdWithQuestionsAndOptions
{
    public record GetExamByIdWithQuestionsAndOptionsQuery(Guid Id) : IRequest<GetByIdResponseDto<Exam>>;
}
