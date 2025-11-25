// GetStudentExamQueryHandler.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetStudentExam
{
    public class GetStudentExamQueryHandler : IRequestHandler<GetStudentExamQuery, GetByIdResponseDto<StudentExam>>
    {
        private readonly IExamRepository _examRepository;

        public GetStudentExamQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<GetByIdResponseDto<StudentExam>> Handle(GetStudentExamQuery request, CancellationToken cancellationToken)
        {
            return await _examRepository.GetStudentExamAsync(request.ExamId, request.StudentId, cancellationToken);
        }
    }
}