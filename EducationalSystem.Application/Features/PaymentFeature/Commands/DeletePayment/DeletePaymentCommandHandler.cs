using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.PaymentFeature.Commands.DeletePayment
{
    public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, ResponseMessage>
    {
        private readonly IPayment _payment;
        public DeletePaymentCommandHandler(IPayment payment)
        {
            _payment = payment;
        }
        public async Task<ResponseMessage> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            return await _payment.DeletePaymentAsync(request.id, request.deletedById);
        }
    }
}
