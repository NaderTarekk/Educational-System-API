// HasStudentStartedExamQuery.cs
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.HasStudentStartedExam
{
    public record HasStudentStartedExamQuery(Guid ExamId, string StudentId) : IRequest<GetByIdResponseDto<bool>>;
}