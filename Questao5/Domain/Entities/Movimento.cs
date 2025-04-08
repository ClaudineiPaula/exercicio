namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public int idMovimento { get; set; }
        public int idcontacorrente { get; set; }
        public DateTime dataMovimento { get; set; }
        public string tipoMovimento { get; set; }
        public double valor { get; set; }
        public string ChaveIdempotencia { get; set; }
    }
}
