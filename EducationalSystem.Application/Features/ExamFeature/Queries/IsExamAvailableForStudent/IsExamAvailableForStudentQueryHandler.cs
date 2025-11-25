// IsExamAvailableForStudentQueryHandler.cs
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.IsExamAvailableForStudent
{
    public class IsExamAvailableForStudentQueryHandler : IRequestHandler<IsExamAvailableForStudentQuery, GetByIdResponseDto<bool>>
    {
        private readonly IExamRepository _examRepository;

        public IsExamAvailableForStudentQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<GetByIdResponseDto<bool>> Handle(IsExamAvailableForStudentQuery request, CancellationToken cancellationToken)
        {
            return await _examRepository.IsExamAvailableForStudentAsync(request.ExamId, cancellationToken);
        }
    }
}