using MediatR;
using Questao5.Domain.Entities;

namespace Questao5.Application.Commands
{
    public class MovimentoCommand : IRequest<Movimento>
    {
        public int IdMovimento { get; set; }
        public int IdContaCorrente { get; set; }
        public DateTime DataMovimento { get; set; }
        public string TipoMovimento { get; set; }
        public double Valor { get; set; }
        public string ChaveIdempotencia {  get; set; }
    }
}
