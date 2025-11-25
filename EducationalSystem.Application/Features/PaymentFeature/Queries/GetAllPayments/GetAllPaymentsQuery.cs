using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.PaymentFeature.Queries.GetAllPayments
{
    public record GetAllPaymentsQuery:IRequest<GetByIdResponseDto<List<Payment>>>;
}
