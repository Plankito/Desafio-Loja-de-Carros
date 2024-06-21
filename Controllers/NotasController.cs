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
    public class NotasController : Controller
    {
        private readonly Desafio_Loja_de_CarrosContext _context;

        public NotasController(Desafio_Loja_de_CarrosContext context)
        {
            _context = context;
        }

        // GET: Notas
        public async Task<IActionResult> Index()
        {
            var desafio_Loja_de_CarrosContext = _context.Nota.Include(n => n.Carro).Include(n => n.Comprador).Include(n => n.Vendedor);
            return View(await desafio_Loja_de_CarrosContext.ToListAsync());
        }

        // GET: Notas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Nota == null)
            {
                return NotFound();
            }

            var nota = await _context.Nota
                .Include(n => n.Carro)
                .Include(n => n.Comprador)
                .Include(n => n.Vendedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nota == null)
            {
                return NotFound();
            }

            return View(nota);
        }

        // GET: Notas/Create
        public IActionResult Create()
        {
            ViewBag.Compradores = new SelectList(_context.Cliente, "Id", "Nome");
            ViewBag.Vendedores = new SelectList(_context.Vendedor, "Id", "Nome");
            ViewBag.Carros = new SelectList(_context.Carro, "Id", "Modelo");
            //ViewData["CarroId"] = new SelectList(_context.Carro, "Id", "Id");
            //ViewData["CompradorId"] = new SelectList(_context.Cliente, "Id", "Id");
            //ViewData["VendedorId"] = new SelectList(_context.Vendedor, "Id", "Id");
            return View();
        }

        // POST: Notas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Numero,DataEmissao,Garantia,ValorVenda,CompradorId,VendedorId,CarroId")] Nota nota)
        {
            if (!ModelState.IsValid)
            {
                //nota.Vendedor.ComissaoManager("Adicao", nota.ValorVenda);
                
                _context.Add(nota);
                var vendedores = await _context.Vendedor.ToListAsync();

                foreach (var vendedor in vendedores)
                {
                    if (vendedor.Id == nota.VendedorId)
                    {
                        vendedor.ComissaoTotal = vendedor.ComissaoManager("Adicao", nota.ValorVenda);
                        await _context.SaveChangesAsync();
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarroId"] = new SelectList(_context.Carro, "Id", "Id", nota.CarroId);
            ViewData["CompradorId"] = new SelectList(_context.Cliente, "Id", "Id", nota.CompradorId);
            ViewData["VendedorId"] = new SelectList(_context.Vendedor, "Id", "Id", nota.VendedorId);
            return View(nota);
        }

        // GET: Notas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Nota == null)
            {
                return NotFound();
            }

            var nota = await _context.Nota.FindAsync(id);
            if (nota == null)
            {
                return NotFound();
            }
            ViewData["CarroId"] = new SelectList(_context.Carro, "Id", "Modelo", nota.CarroId);
            ViewData["CompradorId"] = new SelectList(_context.Cliente, "Id", "Nome", nota.CompradorId);
            ViewData["VendedorId"] = new SelectList(_context.Vendedor, "Id", "Nome", nota.VendedorId);
            return View(nota);
        }

        // POST: Notas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Numero,DataEmissao,Garantia,ValorVenda,CompradorId,VendedorId,CarroId")] Nota nota)
        {
            if (id != nota.Id)
            {
                return NotFound();
            }

            var notaAntiga = await _context.Nota.AsNoTracking().FirstOrDefaultAsync(n => n.Id == nota.Id);
            var vendedorAntigo = await _context.Vendedor.FindAsync(notaAntiga.VendedorId);
            var vendedorNovo = await _context.Vendedor.FindAsync(nota.VendedorId);

            if (!ModelState.IsValid)
            {
                try
                {
                    if (nota.ValorVenda != notaAntiga.ValorVenda)
                    {
                        if (nota.ValorVenda < notaAntiga.ValorVenda)
                        {
                            double diferencaValor = notaAntiga.ValorVenda - nota.ValorVenda;
                            vendedorAntigo.ComissaoTotal = vendedorAntigo.ComissaoManager("Subtracao", diferencaValor);
                        }
                        else
                        {
                            double diferencaValor = nota.ValorVenda - notaAntiga.ValorVenda;
                            vendedorAntigo.ComissaoTotal = vendedorAntigo.ComissaoManager("Adicao", diferencaValor);
                        }
                    }

                    if (nota.VendedorId != notaAntiga.VendedorId)
                    {
                        vendedorAntigo.ComissaoTotal = vendedorAntigo.ComissaoManager("Subtracao", notaAntiga.ValorVenda);
                        vendedorNovo.ComissaoTotal = vendedorNovo.ComissaoManager("Adicao", nota.ValorVenda);
                    }

                    _context.Entry(notaAntiga).State = EntityState.Detached; // Desanexa a nota antiga

                    _context.Update(nota);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotaExists(nota.Id))
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

            ViewData["CarroId"] = new SelectList(_context.Carro, "Id", "Id", nota.CarroId);
            ViewData["CompradorId"] = new SelectList(_context.Cliente, "Id", "Id", nota.CompradorId);
            ViewData["VendedorId"] = new SelectList(_context.Vendedor, "Id", "Id", nota.VendedorId);
            return View(nota);
        }

        // GET: Notas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Nota == null)
            {
                return NotFound();
            }

            var nota = await _context.Nota
                .Include(n => n.Carro)
                .Include(n => n.Comprador)
                .Include(n => n.Vendedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nota == null)
            {
                return NotFound();
            }

            return View(nota);
        }

        // POST: Notas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Nota == null)
            {
                return Problem("Entity set 'Desafio_Loja_de_CarrosContext.Nota'  is null.");
            }
            var nota = await _context.Nota.FindAsync(id);
            var vendedor = await _context.Vendedor.FindAsync(nota.VendedorId);
            
       
            if (nota != null)
            {
                vendedor.ComissaoTotal = vendedor.ComissaoManager("Subtracao", nota.ValorVenda);
                _context.Nota.Remove(nota);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotaExists(int id)
        {
          return (_context.Nota?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
