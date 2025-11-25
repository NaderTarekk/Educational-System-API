// GetQuestionsCountQueryHandler.cs
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetQuestionsCount
{
    public class GetQuestionsCountQueryHandler : IRequestHandler<GetQuestionsCountQuery, GetByIdResponseDto<int>>
    {
        private readonly IExamRepository _examRepository;

        public GetQuestionsCountQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<GetByIdResponseDto<int>> Handle(GetQuestionsCountQuery request, CancellationToken cancellationToken)
        {
            return await _examRepository.GetQuestionsCountAsync(request.ExamId, cancellationToken);
        }
    }
}