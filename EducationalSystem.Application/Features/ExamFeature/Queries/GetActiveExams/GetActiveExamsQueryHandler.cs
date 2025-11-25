// GetActiveExamsQueryHandler.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetActiveExams
{
    public class GetActiveExamsQueryHandler : IRequestHandler<GetActiveExamsQuery, GetByIdResponseDto<List<Exam>>>
    {
        private readonly IExamRepository _examRepository;

        public GetActiveExamsQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<GetByIdResponseDto<List<Exam>>> Handle(GetActiveExamsQuery request, CancellationToken cancellationToken)
        {
            return await _examRepository.GetActiveExamsAsync(cancellationToken);
        }
    }
}