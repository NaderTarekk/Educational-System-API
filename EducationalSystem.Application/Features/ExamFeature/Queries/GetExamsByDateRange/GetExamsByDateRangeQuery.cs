// GetExamsByDateRangeQuery.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetExamsByDateRange
{
    public record GetExamsByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<GetByIdResponseDto<List<Exam>>>;
}