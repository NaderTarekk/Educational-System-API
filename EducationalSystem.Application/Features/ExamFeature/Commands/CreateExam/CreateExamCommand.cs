// CreateExamCommand.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Commands.CreateExam
{
    public record CreateExamCommand(ExamDto Exam) : IRequest<ResponseMessage>;
}