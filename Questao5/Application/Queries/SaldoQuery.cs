using MediatR;
using Questao5.Domain.Entities;

namespace Questao5.Application.Queries
{
    public class SaldoQuery : IRequest<Saldo>
    {
        public Guid ContaCorrenteId { get; set; }
    }
}
