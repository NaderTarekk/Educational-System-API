// SubmitExamCommandHandler.cs
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Commands.SubmitExam
{
    public class SubmitExamCommandHandler : IRequestHandler<SubmitExamCommand, ResponseMessage>
    {
        private readonly IExamRepository _examRepository;

        public SubmitExamCommandHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<ResponseMessage> Handle(SubmitExamCommand request, CancellationToken cancellationToken)
        {
            return await _examRepository.SubmitExamAsync(request.StudentExamId, cancellationToken);
        }
    }
}