using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.PaymentFeature.Commands.UpdateFeature
{
    public record UpdatePaymentCommand(CreatePaymentDto payment) : IRequest<ResponseMessage>;
}
