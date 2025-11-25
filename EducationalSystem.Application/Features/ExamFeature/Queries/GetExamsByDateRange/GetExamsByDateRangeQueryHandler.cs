// GetExamsByDateRangeQueryHandler.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetExamsByDateRange
{
    public class GetExamsByDateRangeQueryHandler : IRequestHandler<GetExamsByDateRangeQuery, GetByIdResponseDto<List<Exam>>>
    {
        private readonly IExamRepository _examRepository;

        public GetExamsByDateRangeQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<GetByIdResponseDto<List<Exam>>> Handle(GetExamsByDateRangeQuery request, CancellationToken cancellationToken)
        {
            return await _examRepository.GetExamsByDateRangeAsync(request.StartDate, request.EndDate, cancellationToken);
        }
    }
}