namespace API_REST
{
    public class Pessoa
    {
        public long Codigo { get; set; }
        public string? Nome { get; set; }
        public string? CPF { get; set; }
        public string? UF { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}