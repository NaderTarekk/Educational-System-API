// ExamExistsQuery.cs
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.ExamExists
{
    public record ExamExistsQuery(Guid Id) : IRequest<GetByIdResponseDto<bool>>;
}