// GetStudentExamQuery.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetStudentExam
{
    public record GetStudentExamQuery(Guid ExamId, string StudentId) : IRequest<GetByIdResponseDto<StudentExam>>;
}