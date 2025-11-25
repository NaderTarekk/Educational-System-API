// HasStudentStartedExamQueryHandler.cs
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.HasStudentStartedExam
{
    public class HasStudentStartedExamQueryHandler : IRequestHandler<HasStudentStartedExamQuery, GetByIdResponseDto<bool>>
    {
        private readonly IExamRepository _examRepository;

        public HasStudentStartedExamQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<GetByIdResponseDto<bool>> Handle(HasStudentStartedExamQuery request, CancellationToken cancellationToken)
        {
            return await _examRepository.HasStudentStartedExamAsync(request.ExamId, request.StudentId, cancellationToken);
        }
    }
}