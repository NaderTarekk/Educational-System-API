using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetExamByIdWithQuestions
{
    public class GetExamByIdWithQuestionsQueryHandler : IRequestHandler<GetExamByIdWithQuestionsQuery, GetByIdResponseDto<Exam>>
    {
        private readonly IExamRepository _examRepository;

        public GetExamByIdWithQuestionsQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<GetByIdResponseDto<Exam>> Handle(GetExamByIdWithQuestionsQuery request, CancellationToken cancellationToken)
        {
            return await _examRepository.GetByIdWithQuestionsAsync(request.Id, cancellationToken);
        }
    }
}