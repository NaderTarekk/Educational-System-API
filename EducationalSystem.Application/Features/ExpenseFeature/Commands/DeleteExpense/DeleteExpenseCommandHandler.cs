using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.ExpenseFeature.Commands.DeleteExpense
{
    public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, ResponseMessage>
    {
        private readonly IExpense _expense;
        public DeleteExpenseCommandHandler(IExpense expense)
        {
            _expense = expense;
        }
        public async Task<ResponseMessage> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            return await _expense.DeleteExpenseAsync(request.id);
        }
    }
}
