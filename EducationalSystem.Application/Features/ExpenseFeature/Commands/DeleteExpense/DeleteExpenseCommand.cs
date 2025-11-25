using EducationalSystem.Domain.Entities.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.Application.Features.ExpenseFeature.Commands.DeleteExpense
{
    public record DeleteExpenseCommand(string id) : IRequest<ResponseMessage>;
}
