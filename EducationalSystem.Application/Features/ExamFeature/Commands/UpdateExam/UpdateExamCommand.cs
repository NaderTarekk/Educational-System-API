// UpdateExamCommand.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Commands.UpdateExam
{
    public record UpdateExamCommand(Exam Exam) : IRequest<ResponseMessage>;
}