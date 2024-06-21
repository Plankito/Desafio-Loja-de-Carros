using NuGet.Protocol.Plugins;

namespace Desafio_Loja_de_Carros.Models
{
    public class Carro
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo {  get; set; }
        public int AnoFabricacao { get; set; }
        public int AnoModelo { get; set; }
        public string Chassi {  get; set; }
        public double Preco {  get; set; }


    }
}
