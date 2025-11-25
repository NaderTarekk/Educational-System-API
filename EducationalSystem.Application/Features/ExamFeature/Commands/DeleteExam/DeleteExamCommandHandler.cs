// DeleteExamCommandHandler.cs
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Commands.DeleteExam
{
    public class DeleteExamCommandHandler : IRequestHandler<DeleteExamCommand, ResponseMessage>
    {
        private readonly IExamRepository _examRepository;

        public DeleteExamCommandHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<ResponseMessage> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
        {
            return await _examRepository.DeleteAsync(request.Exam, cancellationToken);
        }
    }
}