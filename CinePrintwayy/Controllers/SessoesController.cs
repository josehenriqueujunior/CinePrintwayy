using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinePrintwayy.Models;
using Microsoft.AspNetCore.Authorization;

namespace CinePrintwayy.Controllers
{
    [Authorize]
    public class SessoesController : Controller
    {
        private readonly AppDbContext _context;
        
        public SessoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Sessoes
        public async Task<IActionResult> Index()
        {
            List<Sessao> sessoes = await _context.Sessao.ToListAsync();

            foreach(Sessao s in sessoes)
            {
                s.Sala = await _context.Sala.FirstOrDefaultAsync(m => m.Id == s.IdSala);
                s.Filme = await _context.Filme.FirstOrDefaultAsync(m => m.Id == s.IdFilme);
            }

            return View(sessoes);
        }

        // GET: Sessoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessao = await _context.Sessao
                .FirstOrDefaultAsync(m => m.Id == id);

            sessao.Sala = await _context.Sala.FirstOrDefaultAsync(m => m.Id == sessao.IdSala);
            sessao.Filme = await _context.Filme.FirstOrDefaultAsync(m => m.Id == sessao.IdFilme);

            if (sessao == null)
            {
                return NotFound();
            }

            return View(sessao);
        }

        // GET: Sessoes/Create
        public async Task<IActionResult> Create()
        {
            Sessao sessao = new Sessao();

            List<Sala> salas = await _context.Sala.ToListAsync();
            List<Filme> filmes = await _context.Filme.ToListAsync();

            ViewBag.Salas = new SelectList(salas, "Id", "Nome", salas[0].Id);
            ViewBag.Filmes = new SelectList(filmes, "Id", "Titulo", filmes[0].Id);

            return View();
        }

        // POST: Sessoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,HorarioInicio,ValorIngresso,TipoAnimacao,TipoAudio,IdFilme,IdSala")] Sessao sessao)
        {
            List<Sala> salas = await _context.Sala.ToListAsync();
            List<Filme> filmes = await _context.Filme.ToListAsync();

            if (ModelState.IsValid)
            {
                sessao.Filme = await _context.Filme.FirstOrDefaultAsync(m => m.Id == sessao.IdFilme);

                sessao.HorarioInicio = sessao.Data.Add(TimeSpan.Parse(sessao.HorarioInicio.TimeOfDay.ToString()));
                sessao.HorarioFim = sessao.HorarioInicio.Add(TimeSpan.Parse(sessao.Filme.Duracao.TimeOfDay.ToString()));

                if (_context.Sessao.Any(x => x.IdSala == sessao.IdSala && (sessao.HorarioInicio >= x.HorarioInicio && sessao.HorarioInicio <= x.HorarioFim)))
                {
                    ViewBag.Salas = new SelectList(salas, "Id", "Nome", sessao.IdSala);
                    ViewBag.Filmes = new SelectList(filmes, "Id", "Titulo", sessao.IdFilme);
                    ModelState.AddModelError("IdSala", "Sala não disponível para o horário selecionado");
                    return View(sessao);
                }

                _context.Add(sessao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Salas = new SelectList(salas, "Id", "Nome", sessao.IdSala);
            ViewBag.Filmes = new SelectList(filmes, "Id", "Titulo", sessao.IdFilme);

            return View(sessao);
        }

        // GET: Sessoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessao = await _context.Sessao.FindAsync(id);
            if (sessao == null)
            {
                return NotFound();
            }
            return View(sessao);
        }

        // POST: Sessoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,HorarioInicio,HorarioFim,ValorIngresso,TipoAnimacao,TipoAudio,IdSala")] Sessao sessao)
        {
            if (id != sessao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sessao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessaoExists(sessao.Id))
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
            return View(sessao);
        }

        // GET: Sessoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessao = await _context.Sessao
                .FirstOrDefaultAsync(m => m.Id == id);


            sessao.Sala = await _context.Sala.FirstOrDefaultAsync(m => m.Id == sessao.IdSala);
            sessao.Filme = await _context.Filme.FirstOrDefaultAsync(m => m.Id == sessao.IdFilme);

            if (sessao == null)
            {
                return NotFound();
            }

            return View(sessao);
        }

        // POST: Sessoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var s = await _context.Sessao.FirstOrDefaultAsync(m => m.Id == id);

            s.Sala = await _context.Sala.FirstOrDefaultAsync(m => m.Id == s.IdSala);
            s.Filme = await _context.Filme.FirstOrDefaultAsync(m => m.Id == s.IdFilme);

            if (s.Data.AddDays(-10) < DateTime.Now)
            {
                ModelState.AddModelError("IdSala", "Só é possível excluir uma sessão até 10 dias antes de sua ocorrência");
                return View(s);
            }

            var sessao = await _context.Sessao.FindAsync(id);
            _context.Sessao.Remove(sessao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessaoExists(int id)
        {
            return _context.Sessao.Any(e => e.Id == id);
        }
    }
}
