using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.PaymentFeature.Queries.GetAllPayments
{
    public class GetAllPaymentsQueryHandler:IRequestHandler<GetAllPaymentsQuery, GetByIdResponseDto<List<Payment>>>
    {
        private readonly IPayment _payment;
        public GetAllPaymentsQueryHandler(IPayment payment)
        {
            _payment = payment;
        }

        public async Task<GetByIdResponseDto<List<Payment>>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
        {
            return await _payment.GetPaymentsAsync();
        }
    }
}
