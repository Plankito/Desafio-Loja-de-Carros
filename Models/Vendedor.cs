using NuGet.Protocol.Plugins;
using System.Drawing;

namespace Desafio_Loja_de_Carros.Models
{
    public class Vendedor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataAdmissao { get; set; }
        public int Matricula { get; set; }
        public double Salario { get; set; }
        public double ComissaoTotal { get; set; }

        public double ComissaoManager(string operacao, double valor)
        {
            double valorFinal = 0;
            
            if (operacao == "Subtracao")
            {
                valorFinal = ComissaoTotal - (0.05 * valor);
            }
            else
            {
                valorFinal = ComissaoTotal + (0.05 * valor);
            }
            return valorFinal;
        }

    }
}
