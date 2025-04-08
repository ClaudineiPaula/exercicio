using System.Data;
using Dapper;
using MediatR;
using Questao5.Application.Queries;
using Questao5.Domain.Entities;

namespace Questao5.Application.Handlers
{
    public class SaldoQueryHandler : IRequestHandler<SaldoQuery, Saldo>
    {
        private readonly IDbConnection _dbConnection;

        public SaldoQueryHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Saldo> Handle(SaldoQuery request, CancellationToken cancellationToken)
        {
            var conta = await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @Id", new { Id = request.ContaCorrenteId });
            /*
            if (conta == null)
                throw new Exception("Conta inválida", "INVALID_ACCOUNT");

            if (conta.Ativo == 0)
                throw new Exception("Conta inativa", "INACTIVE_ACCOUNT");
            */
            var creditos = await _dbConnection.QuerySingleAsync<decimal>(
                "SELECT COALESCE(SUM(valor), 0) FROM movimento WHERE idcontacorrente = @Id AND tipomovimento = 'C'", new { Id = request.ContaCorrenteId });

            var debitos = await _dbConnection.QuerySingleAsync<decimal>(
                "SELECT COALESCE(SUM(valor), 0) FROM movimento WHERE idcontacorrente = @Id AND tipomovimento = 'D'", new { Id = request.ContaCorrenteId });

            return new Saldo
            {
                NumeroConta = conta.Numero,
                NomeTitular = conta.Nome,
                DataConsulta = DateTime.Now,
                SaldoAtual = creditos - debitos
            };
        }
    }
}
