// GetStudentExamResultQueryHandler.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetStudentExamResult
{
    public class GetStudentExamResultQueryHandler : IRequestHandler<GetStudentExamResultQuery, GetByIdResponseDto<StudentExam>>
    {
        private readonly IExamRepository _examRepository;

        public GetStudentExamResultQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<GetByIdResponseDto<StudentExam>> Handle(GetStudentExamResultQuery request, CancellationToken cancellationToken)
        {
            return await _examRepository.GetStudentExamResultAsync(request.StudentExamId, cancellationToken);
        }
    }
}