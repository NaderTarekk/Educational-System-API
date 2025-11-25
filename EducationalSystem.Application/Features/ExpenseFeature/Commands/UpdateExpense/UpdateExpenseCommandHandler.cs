using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExpenseFeature.Commands.UpdateExpense
{
    public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, ResponseMessage>
    {
        private readonly IExpense _expense;
        public UpdateExpenseCommandHandler(IExpense expense)
        {
            _expense = expense;
        }
        public async Task<ResponseMessage> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            return await _expense.UpdateExpenseAsync(request.expense);
        }
    }
}
