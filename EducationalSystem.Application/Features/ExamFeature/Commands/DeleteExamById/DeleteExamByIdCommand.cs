// DeleteExamByIdCommand.cs
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Commands.DeleteExamById
{
    public record DeleteExamByIdCommand(Guid Id) : IRequest<ResponseMessage>;
}