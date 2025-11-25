using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExpenseFeature.Queries.GetExpenseById
{
    public record GetExpenseByIdQuery(string id) : IRequest<GetByIdResponseDto<Expense>>;

}
