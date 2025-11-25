// SubmitAnswerCommandHandler.cs
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Commands.SubmitAnswer
{
    public class SubmitAnswerCommandHandler : IRequestHandler<SubmitAnswerCommand, ResponseMessage>
    {
        private readonly IExamRepository _examRepository;

        public SubmitAnswerCommandHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<ResponseMessage> Handle(SubmitAnswerCommand request, CancellationToken cancellationToken)
        {
            return await _examRepository.SubmitAnswerAsync(request.Answer, cancellationToken);
        }
    }
}