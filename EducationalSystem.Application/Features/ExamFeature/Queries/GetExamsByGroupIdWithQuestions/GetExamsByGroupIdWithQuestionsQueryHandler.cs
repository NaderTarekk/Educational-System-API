// GetExamsByGroupIdWithQuestionsQueryHandler.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetExamsByGroupIdWithQuestions
{
    public class GetExamsByGroupIdWithQuestionsQueryHandler : IRequestHandler<GetExamsByGroupIdWithQuestionsQuery, GetByIdResponseDto<List<Exam>>>
    {
        private readonly IExamRepository _examRepository;

        public GetExamsByGroupIdWithQuestionsQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<GetByIdResponseDto<List<Exam>>> Handle(GetExamsByGroupIdWithQuestionsQuery request, CancellationToken cancellationToken)
        {
            return await _examRepository.GetByGroupIdWithQuestionsAsync(request.GroupId, cancellationToken);
        }
    }
}