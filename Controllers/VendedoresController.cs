using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Desafio_Loja_de_Carros.Data;
using Desafio_Loja_de_Carros.Models;

namespace Desafio_Loja_de_Carros.Controllers
{
    public class VendedoresController : Controller
    {
        private readonly Desafio_Loja_de_CarrosContext _context;

        public VendedoresController(Desafio_Loja_de_CarrosContext context)
        {
            _context = context;
        }

        // GET: Vendedores
        public async Task<IActionResult> Index()
        {
              return _context.Vendedor != null ? 
                          View(await _context.Vendedor.ToListAsync()) :
                          Problem("Entity set 'Desafio_Loja_de_CarrosContext.Vendedor'  is null.");
        }

        // GET: Vendedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vendedor == null)
            {
                return NotFound();
            }

            var vendedor = await _context.Vendedor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendedor == null)
            {
                return NotFound();
            }

            return View(vendedor);
        }

        // GET: Vendedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vendedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,DataAdmissao,Matricula,Salario")] Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vendedor);
        }

        // GET: Vendedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vendedor == null)
            {
                return NotFound();
            }

            var vendedor = await _context.Vendedor.FindAsync(id);
            if (vendedor == null)
            {
                return NotFound();
            }
            return View(vendedor);
        }

        // POST: Vendedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DataAdmissao,Matricula,Salario")] Vendedor vendedor)
{
    if (id != vendedor.Id)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            // Busca o vendedor antigo no contexto
            var vendedorAntigo = await _context.Vendedor.FindAsync(vendedor.Id);
            if (vendedorAntigo == null)
            {
                return NotFound();
            }

            // Atualiza apenas as propriedades necessárias do vendedor antigo
            vendedorAntigo.Nome = vendedor.Nome;
            vendedorAntigo.DataAdmissao = vendedor.DataAdmissao;
            vendedorAntigo.Matricula = vendedor.Matricula;
            vendedorAntigo.Salario = vendedor.Salario;
                    // Mantém a comissão total do vendedor antigo
            vendedor.ComissaoTotal = vendedorAntigo.ComissaoTotal;

            // Salva as alterações
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VendedorExists(vendedor.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return RedirectToAction(nameof(Index));
    }
    return View(vendedor);
}

        // GET: Vendedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vendedor == null)
            {
                return NotFound();
            }

            var vendedor = await _context.Vendedor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendedor == null)
            {
                return NotFound();
            }

            return View(vendedor);
        }

        // POST: Vendedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vendedor == null)
            {
                return Problem("Entity set 'Desafio_Loja_de_CarrosContext.Vendedor'  is null.");
            }
            var vendedor = await _context.Vendedor.FindAsync(id);
            if (vendedor != null)
            {
                _context.Vendedor.Remove(vendedor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendedorExists(int id)
        {
          return (_context.Vendedor?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
