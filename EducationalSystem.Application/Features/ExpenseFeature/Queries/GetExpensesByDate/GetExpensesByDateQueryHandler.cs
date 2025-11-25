using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExpenseFeature.Queries.GetExpensesByDate
{
    public class GetExpensesByDateQueryHandler : IRequestHandler<GetExpensesByDateQuery, GetByIdResponseDto<List<Expense>>>
    {
        private readonly IExpense _expense;
        public GetExpensesByDateQueryHandler(IExpense expense)
        {
            _expense = expense;
        }

        public async Task<GetByIdResponseDto<List<Expense>>> Handle(GetExpensesByDateQuery request, CancellationToken cancellationToken)
        {
            return await _expense.GetExpensesByDateRangeAsync(request.fromDate, request.toDate);
        }
    }
}
