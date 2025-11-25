using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.PaymentFeature.Queries.GetPaymentsByDate
{
    public class GetPaymentsByDateQueryHandler : IRequestHandler<GetPaymentsByDateQuery, GetByIdResponseDto<List<Payment>>>
    {
        private readonly IPayment _payment;
        public GetPaymentsByDateQueryHandler(IPayment payment)
        {
            _payment = payment;
        }

        public async Task<GetByIdResponseDto<List<Payment>>> Handle(GetPaymentsByDateQuery request, CancellationToken cancellationToken)
        {
            return await _payment.GetPaymentsByDateRangeAsync(request.fromDate, request.toDate);
        }
    }
}
