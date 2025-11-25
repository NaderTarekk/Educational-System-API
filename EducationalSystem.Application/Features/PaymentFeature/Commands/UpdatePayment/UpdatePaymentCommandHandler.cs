using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.PaymentFeature.Commands.UpdateFeature
{
    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, ResponseMessage>
    {
        private readonly IPayment _payment;
        public UpdatePaymentCommandHandler(IPayment payment)
        {
            _payment = payment;
        }
        public async Task<ResponseMessage> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            return await _payment.UpdatePaymentAsync(request.payment);
        }
    }
}
