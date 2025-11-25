using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.PaymentFeature.Commands.DeletePayment
{
    public record DeletePaymentCommand(string id, string deletedById) : IRequest<ResponseMessage>;
}
