// GetStudentExamResultQuery.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetStudentExamResult
{
    public record GetStudentExamResultQuery(Guid StudentExamId) : IRequest<GetByIdResponseDto<StudentExam>>;
}