using EducationalSystem.Application.Features.ExpenseFeature.Commands.CreateExpense;
using EducationalSystem.Application.Features.ExpenseFeature.Commands.DeleteExpense;
using EducationalSystem.Application.Features.ExpenseFeature.Commands.UpdateExpense;
using EducationalSystem.Application.Features.ExpenseFeature.Queries.GetAllExpenses;
using EducationalSystem.Application.Features.ExpenseFeature.Queries.GetExpenseById;
using EducationalSystem.Application.Features.ExpenseFeature.Queries.GetExpensesByDate;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalSystem.API.Controllers
{
    [Authorize(Roles = "Admin,Assistant")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpensesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseCommand command)
            => Ok(await _mediator.Send(command));

        [HttpGet]
        public async Task<IActionResult> GetAllExpenses()
            => Ok(await _mediator.Send(new GetAllExpensesQuery()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseById(Guid id)
            => Ok(await _mediator.Send(new GetExpenseByIdQuery(id.ToString())));

        [HttpPut]
        public async Task<IActionResult> UpdateExpense([FromBody] CreateExpenseDto dto)
            => Ok(await _mediator.Send(new UpdateExpenseCommand(dto)));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(Guid id)
            => Ok(await _mediator.Send(new DeleteExpenseCommand(id.ToString())));

        [HttpGet("range")]
        public async Task<IActionResult> GetExpensesByDateRange([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
            => Ok(await _mediator.Send(new GetExpensesByDateQuery(fromDate, toDate)));
    }
}
