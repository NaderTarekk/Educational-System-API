using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Entities;
using MediatR;

namespace EducationalSystem.Application.Features.ExpenseFeature.Commands.CreateExpense
{
    public record CreateExpenseCommand(CreateExpenseDto expense) : IRequest<ResponseMessage>;
}
