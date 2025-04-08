namespace Questao5.Domain.Entities
{
    public class Saldo
    {
        public int NumeroConta { get; set; }
        public string NomeTitular { get; set; }
        public DateTime DataConsulta { get; set; }
        public decimal SaldoAtual { get; set; }
    }
}
