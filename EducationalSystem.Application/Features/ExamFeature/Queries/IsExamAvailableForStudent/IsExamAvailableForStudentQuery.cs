// IsExamAvailableForStudentQuery.cs
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.IsExamAvailableForStudent
{
    public record IsExamAvailableForStudentQuery(Guid ExamId) : IRequest<GetByIdResponseDto<bool>>;
}