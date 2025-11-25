using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetAllExams
{
    public class GetAllExamsQueryHandler : IRequestHandler<GetAllExamsQuery, GetByIdResponseDto<List<ExamDto>>>
    {
        private readonly IExamRepository _examRepository;

        public GetAllExamsQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<GetByIdResponseDto<List<ExamDto>>> Handle(GetAllExamsQuery request, CancellationToken cancellationToken)
        {
            return await _examRepository.GetAllAsync(cancellationToken);
        }
    }
}