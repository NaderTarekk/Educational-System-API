using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.PaymentFeature.Commands.CreatePayment
{
    public record CreatePaymentCommand(CreatePaymentDto payment):IRequest<ResponseMessage>;
}
