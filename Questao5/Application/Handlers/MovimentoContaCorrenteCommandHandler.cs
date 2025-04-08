using System.Data;
using Dapper;
using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands;
using Questao5.Domain.Entities;

namespace Questao5.Application.Handlers
{
    public class MovimentoContaCorrenteCommandHandler : IRequestHandler<MovimentoContaCorrenteCommand, Movimento>
    {
        private readonly IDbConnection _dbConnection;

        public MovimentoContaCorrenteCommandHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Guid> Handle(MovimentoCommand request, CancellationToken cancellationToken)
        {

            var conta = await _dbConnection.QueryFirstOrDefaultAsync<Movimento>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @Id", new { Id = request.IdContaCorrente });
            /*
            if (conta == null)
                throw new Exception("Conta inválida", "INVALID_ACCOUNT");

            if (conta.Ativo == 0)
                throw new Exception("Conta inativa", "INACTIVE_ACCOUNT");

            if (request.Valor <= 0)
                throw new Exception("Valor inválido", "INVALID_VALUE");

            if (request.tipoMovimento != "C" && request.tipoMovimento != "D")
                throw new Exception("Tipo de movimento inválido", "INVALID_TYPE");
            */
            // Verificar idempotência
            var idempotencia = await _dbConnection.QueryFirstOrDefaultAsync<Idempotencia>(
                "SELECT * FROM idempotencia WHERE chave_idempotencia = @Chave", new { Chave = request.ChaveIdempotencia });

            if (idempotencia != null)
                return Guid.Parse(idempotencia.Resultado);

            // Inserir movimento
            var idMovimento = Guid.NewGuid();
            await _dbConnection.ExecuteAsync(
                "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@Id, @ContaId, @Data, @Tipo, @Valor)",
                new { Id = idMovimento, ContaId = request.IdContaCorrente, Data = DateTime.Now.ToString("dd/MM/yyyy"), Tipo = request.TipoMovimento, request.Valor });

            // Salvar chave de idempotência
            await _dbConnection.ExecuteAsync(
                "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) VALUES (@Chave, @Requisicao, @Resultado)",
                new { Chave = request.ChaveIdempotencia, Requisicao = JsonConvert.SerializeObject(request), Resultado = idMovimento.ToString() });

            return idMovimento;
        }

        public Task<Movimento> Handle(MovimentoContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
