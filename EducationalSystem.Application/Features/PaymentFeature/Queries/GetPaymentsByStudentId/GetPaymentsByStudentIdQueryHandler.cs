using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.PaymentFeature.Queries.GetPaymentsByStudentId
{
    public class GetPaymentsByStudentIdQueryHandler:IRequestHandler<GetPaymentsByStudentIdQuery, GetByIdResponseDto<List<Payment>>>
    {
        private readonly IPayment _payment;
        public GetPaymentsByStudentIdQueryHandler(IPayment payment)
        {
            _payment = payment;
        }

        public async Task<GetByIdResponseDto<List<Payment>>> Handle(GetPaymentsByStudentIdQuery request, CancellationToken cancellationToken)
        {
            return await _payment.GetPaymentsByStudentIdAsync(request.id);
        }
    }
}
