using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetExamById
{
    public class GetExamByIdQueryHandler : IRequestHandler<GetExamByIdQuery, GetByIdResponseDto<Exam>>
    {
        private readonly IExamRepository _examRepository;

        public GetExamByIdQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<GetByIdResponseDto<Exam>> Handle(GetExamByIdQuery request, CancellationToken cancellationToken)
        {
            return await _examRepository.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
