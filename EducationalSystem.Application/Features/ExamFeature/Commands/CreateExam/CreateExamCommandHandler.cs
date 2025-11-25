// CreateExamCommandHandler.cs
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Commands.CreateExam
{
    public class CreateExamCommandHandler : IRequestHandler<CreateExamCommand, ResponseMessage>
    {
        private readonly IExamRepository _examRepository;

        public CreateExamCommandHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<ResponseMessage> Handle(CreateExamCommand request, CancellationToken cancellationToken)
        {
            return await _examRepository.AddAsync(request.Exam, cancellationToken);
        }
    }
}