// SubmitExamCommand.cs
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Commands.SubmitExam
{
    public record SubmitExamCommand(Guid StudentExamId) : IRequest<ResponseMessage>;
}