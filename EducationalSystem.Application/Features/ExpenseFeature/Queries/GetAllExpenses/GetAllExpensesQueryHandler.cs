using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Entities;
using MediatR;
using EducationalSystem.Domain.Interfaces;

namespace EducationalSystem.Application.Features.ExpenseFeature.Queries.GetAllExpenses
{
    public class GetAllExpensesQueryHandler : IRequestHandler<GetAllExpensesQuery, GetByIdResponseDto<List<Expense>>>
    {
        private readonly IExpense _expense;
        public GetAllExpensesQueryHandler(IExpense expense)
        {
            _expense = expense;
        }
        public async Task<GetByIdResponseDto<List<Expense>>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
        {
            return await _expense.GetAllExpensesAsync();
        }
    }
}
