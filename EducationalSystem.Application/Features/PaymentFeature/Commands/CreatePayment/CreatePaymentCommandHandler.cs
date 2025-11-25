using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.PaymentFeature.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, ResponseMessage>
    {
        private readonly IPayment _payment;
        public CreatePaymentCommandHandler(IPayment payment)
        {
            _payment = payment;
        }
        public async Task<ResponseMessage> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            return await _payment.CreatePaymentAsync(request.payment);
        }
    }
}
