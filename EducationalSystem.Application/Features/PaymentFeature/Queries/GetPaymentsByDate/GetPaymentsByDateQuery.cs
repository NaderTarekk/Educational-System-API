using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.PaymentFeature.Queries.GetPaymentsByDate
{
    public record GetPaymentsByDateQuery(DateTime fromDate, DateTime toDate) : IRequest<GetByIdResponseDto<List<Payment>>>;
}
