using EducationalSystem.Application.Features.PaymentFeature.Commands.CreatePayment;
using EducationalSystem.Application.Features.PaymentFeature.Commands.DeletePayment;
using EducationalSystem.Application.Features.PaymentFeature.Commands.UpdateFeature;
using EducationalSystem.Application.Features.PaymentFeature.Queries.GetAllPayments;
using EducationalSystem.Application.Features.PaymentFeature.Queries.GetPaymentById;
using EducationalSystem.Application.Features.PaymentFeature.Queries.GetPaymentsByDate;
using EducationalSystem.Application.Features.PaymentFeature.Queries.GetPaymentsByStudentId;
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalSystem.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto payment)
        {
            var command = new CreatePaymentCommand(payment);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {
            var query = new GetAllPaymentsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePayment([FromBody] CreatePaymentDto payment)
        {
            var command = new UpdatePaymentCommand(payment);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(string id, [FromQuery] string deletedBy)
        {
            var command = new DeletePaymentCommand(id, deletedBy);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(Guid id)
        {
            var query = new GetPaymentByIdQuery(id.ToString());
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetPaymentsByStudentId(string studentId)
        {
            var query = new GetPaymentsByStudentIdQuery(studentId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetPaymentsByDateRange([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            var query = new GetPaymentsByDateQuery(fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
