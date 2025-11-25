using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExamFeature.Queries.GetAllExams
{
    public record GetAllExamsQuery() : IRequest<GetByIdResponseDto<List<ExamDto>>>;
}
