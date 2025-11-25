// StartExamCommandHandler.cs
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Commands.StartExam
{
    public class StartExamCommandHandler : IRequestHandler<StartExamCommand, ResponseMessage>
    {
        private readonly IExamRepository _examRepository;

        public StartExamCommandHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<ResponseMessage> Handle(StartExamCommand request, CancellationToken cancellationToken)
        {
            return await _examRepository.StartExamAsync(request.ExamId, request.StudentId, cancellationToken);
        }
    }
}