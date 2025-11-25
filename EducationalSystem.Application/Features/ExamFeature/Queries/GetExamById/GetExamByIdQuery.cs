using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetExamById
{
    public record GetExamByIdQuery(Guid Id) : IRequest<GetByIdResponseDto<Exam>>;
}
