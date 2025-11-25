using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetExamByIdWithQuestionsAndOptions
{
    public class GetExamByIdWithQuestionsAndOptionsQueryHandler : IRequestHandler<GetExamByIdWithQuestionsAndOptionsQuery, GetByIdResponseDto<Exam>>
    {
        private readonly IExamRepository _examRepository;

        public GetExamByIdWithQuestionsAndOptionsQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<GetByIdResponseDto<Exam>> Handle(GetExamByIdWithQuestionsAndOptionsQuery request, CancellationToken cancellationToken)
        {
            return await _examRepository.GetByIdWithQuestionsAndOptionsAsync(request.Id, cancellationToken);
        }
    }
}