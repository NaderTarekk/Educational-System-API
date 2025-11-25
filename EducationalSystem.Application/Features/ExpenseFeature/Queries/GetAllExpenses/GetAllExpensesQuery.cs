using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExpenseFeature.Queries.GetAllExpenses
{
    public record GetAllExpensesQuery : IRequest<GetByIdResponseDto<List<Expense>>>;
}
