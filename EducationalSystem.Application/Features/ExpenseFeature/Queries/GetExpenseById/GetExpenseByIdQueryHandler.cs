using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExpenseFeature.Queries.GetExpenseById
{
    public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, GetByIdResponseDto<Expense>>
    {
        private readonly IExpense _expense;
        public GetExpenseByIdQueryHandler(IExpense expense)
        {
            _expense = expense;
        }
        public async Task<GetByIdResponseDto<Expense>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            return await _expense.GetExpenseByIdAsync(request.id);
        }
    }
}
