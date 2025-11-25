// GetExamsByGroupIdQueryHandler.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetExamsByGroupId
{
    public class GetExamsByGroupIdQueryHandler : IRequestHandler<GetExamsByGroupIdQuery, GetByIdResponseDto<List<Exam>>>
    {
        private readonly IExamRepository _examRepository;

        public GetExamsByGroupIdQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<GetByIdResponseDto<List<Exam>>> Handle(GetExamsByGroupIdQuery request, CancellationToken cancellationToken)
        {
            return await _examRepository.GetByGroupIdAsync(request.GroupId, cancellationToken);
        }
    }
}