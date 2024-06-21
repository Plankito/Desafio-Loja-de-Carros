namespace Desafio_Loja_de_Carros.Models
{
    public class Nota
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public DateTime DataEmissao { get; set; }
        public string Garantia { get; set; }
        public double ValorVenda { get; set; }
        public Cliente Comprador { get; set; }
        public Vendedor Vendedor { get; set; }
        public Carro Carro { get; set; }

        public int CompradorId { get; set; }
        public int VendedorId { get; set; }
        public int CarroId { get; set; }

    }
}
