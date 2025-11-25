// GetActiveExamsQuery.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetActiveExams
{
    public record GetActiveExamsQuery() : IRequest<GetByIdResponseDto<List<Exam>>>;
}