// StartExamCommand.cs
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Commands.StartExam
{
    public record StartExamCommand(Guid ExamId, string StudentId) : IRequest<ResponseMessage>;
}