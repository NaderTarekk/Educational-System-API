using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.PaymentFeature.Queries.GetPaymentById
{
    public record GetPaymentByIdQuery(string id):IRequest<GetByIdResponseDto<Payment>>;
}
