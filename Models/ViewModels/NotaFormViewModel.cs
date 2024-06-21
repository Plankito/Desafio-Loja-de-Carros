namespace Desafio_Loja_de_Carros.Models.ViewModels
{
    public class NotaFormViewModel
    {   
        public Nota Nota { get; set; }
        public List<Carro> Carros { get; set; }
        public List<Vendedor> Vendedores { get; set; }
        public List<Cliente> Compradores { get; set; }
   
    }
}
