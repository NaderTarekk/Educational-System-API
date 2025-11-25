using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.ExpenseFeature.Commands.UpdateExpense
{
    public record UpdateExpenseCommand(CreateExpenseDto expense) : IRequest<ResponseMessage>;
}
