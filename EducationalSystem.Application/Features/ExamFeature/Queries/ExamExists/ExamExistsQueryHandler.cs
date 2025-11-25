// ExamExistsQueryHandler.cs
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.ExamExists
{
    public class ExamExistsQueryHandler : IRequestHandler<ExamExistsQuery, GetByIdResponseDto<bool>>
    {
        private readonly IExamRepository _examRepository;

        public ExamExistsQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<GetByIdResponseDto<bool>> Handle(ExamExistsQuery request, CancellationToken cancellationToken)
        {
            return await _examRepository.ExistsAsync(request.Id, cancellationToken);
        }
    }
}