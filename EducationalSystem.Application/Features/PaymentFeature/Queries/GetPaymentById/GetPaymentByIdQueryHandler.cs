using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.PaymentFeature.Queries.GetPaymentById
{
    public class GetPaymentByIdQueryHandler:IRequestHandler<GetPaymentByIdQuery, GetByIdResponseDto<Payment>>
    {
        private readonly IPayment _payment;
        public GetPaymentByIdQueryHandler(IPayment payment)
        {
            _payment = payment;
        }

        public async Task<GetByIdResponseDto<Payment>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _payment.GetPaymentByIdAsync(request.id);
        }
    }
}
