using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExpenseFeature.Queries.GetExpensesByDate
{
    public record GetExpensesByDateQuery(DateTime fromDate, DateTime toDate) : IRequest<GetByIdResponseDto<List<Expense>>>;
}
