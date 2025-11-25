using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.PaymentFeature.Queries.GetPaymentsByStudentId
{
    public record GetPaymentsByStudentIdQuery(string id) : IRequest<GetByIdResponseDto<List<Payment>>>;
}
