using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Desafio_Loja_de_Carros.Models;

namespace Desafio_Loja_de_Carros.Data
{
    public class Desafio_Loja_de_CarrosContext : DbContext
    {
        public Desafio_Loja_de_CarrosContext (DbContextOptions<Desafio_Loja_de_CarrosContext> options)
            : base(options)
        {
        }

        public DbSet<Desafio_Loja_de_Carros.Models.Vendedor> Vendedor { get; set; } = default!;

        public DbSet<Desafio_Loja_de_Carros.Models.Cliente> Cliente { get; set; } = default!;

        public DbSet<Desafio_Loja_de_Carros.Models.Carro> Carro { get; set; } = default!;

        public DbSet<Desafio_Loja_de_Carros.Models.Nota> Nota { get; set; } = default!;
    }
}
