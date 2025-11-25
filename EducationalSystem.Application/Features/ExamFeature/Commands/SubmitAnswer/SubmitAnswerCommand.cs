// SubmitAnswerCommand.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Commands.SubmitAnswer
{
    public record SubmitAnswerCommand(StudentAnswer Answer) : IRequest<ResponseMessage>;
}