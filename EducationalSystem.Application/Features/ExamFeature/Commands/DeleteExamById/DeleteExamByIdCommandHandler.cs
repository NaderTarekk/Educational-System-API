// DeleteExamByIdCommandHandler.cs
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Commands.DeleteExamById
{
    public class DeleteExamByIdCommandHandler : IRequestHandler<DeleteExamByIdCommand, ResponseMessage>
    {
        private readonly IExamRepository _examRepository;

        public DeleteExamByIdCommandHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<ResponseMessage> Handle(DeleteExamByIdCommand request, CancellationToken cancellationToken)
        {
            return await _examRepository.DeleteByIdAsync(request.Id, cancellationToken);
        }
    }
}