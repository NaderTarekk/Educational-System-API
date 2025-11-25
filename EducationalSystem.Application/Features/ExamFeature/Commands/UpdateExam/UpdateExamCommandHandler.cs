// UpdateExamCommandHandler.cs
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Commands.UpdateExam
{
    public class UpdateExamCommandHandler : IRequestHandler<UpdateExamCommand, ResponseMessage>
    {
        private readonly IExamRepository _examRepository;

        public UpdateExamCommandHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<ResponseMessage> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
        {
            return await _examRepository.UpdateAsync(request.Exam, cancellationToken);
        }
    }
}