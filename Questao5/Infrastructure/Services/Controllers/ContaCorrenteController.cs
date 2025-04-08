using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands;
using Questao5.Application.Queries;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Movimentar([FromBody] MovimentoCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch ( Exception ex)
            {
                return BadRequest(new { Message = ex.Message, Type = ex.GetType() });
            }
        }

        [HttpGet("{contaCorrenteId}")]
        public async Task<IActionResult> ConsultarSaldo(Guid contaCorrenteId)
        {
            try
            {
                var result = await _mediator.Send(new SaldoQuery { ContaCorrenteId = contaCorrenteId });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, Type = ex.GetType() });
            }
        }
    }
}
