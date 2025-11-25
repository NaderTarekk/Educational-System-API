using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExpenseFeature.Commands.CreateExpense
{
    public class CreateExpenseCommandHandler:IRequestHandler<CreateExpenseCommand, ResponseMessage>
    {
        private readonly IExpense _expense;
        public CreateExpenseCommandHandler(IExpense expense)
        {
            _expense = expense;           
        }

        public async Task<ResponseMessage> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            return await _expense.CreateExpenseAsync(request.expense);
        }
    }
}
