using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetExamsByGroupId
{
    public record GetExamsByGroupIdQuery(Guid GroupId) : IRequest<GetByIdResponseDto<List<Exam>>>;
}
