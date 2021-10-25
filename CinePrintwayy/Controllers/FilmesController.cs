using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinePrintwayy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using CinePrintwayy.ViewModels;

namespace CinePrintwayy.Controllers
{
    [Authorize]
    public class FilmesController : Controller
    {
        private readonly AppDbContext _context;
        IWebHostEnvironment webHostEnvironment;
        public FilmesController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Filmes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Filme.ToListAsync());
        }

        // GET: Filmes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filme
                .FirstOrDefaultAsync(m => m.Id == id);

            var filmeViewModel = new FilmeViewModel()
            {
                Id = filme.Id,
                Titulo = filme.Titulo,
                Descricao = filme.Descricao,
                Duracao = filme.Duracao,
                ExistingImage = filme.Imagem
            };

            if (filme == null)
            {
                return NotFound();
            }

            return View(filmeViewModel);
        }

        // GET: Filmes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Filmes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FilmeViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                Filme filme = new Filme
                {
                    Imagem = uniqueFileName,
                    Titulo = model.Titulo,
                    Descricao = model.Descricao,
                    Duracao = model.Duracao
                };

                _context.Add(filme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Filmes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filme.FindAsync(id);

            var filmeViewModel = new FilmeViewModel()
            {
                Id = filme.Id,
                Titulo = filme.Titulo,
                Descricao = filme.Descricao,
                Duracao = filme.Duracao,
                ExistingImage = filme.Imagem
            };

            if (filme == null)
            {
                return NotFound();
            }
            return View(filmeViewModel);
        }

        // POST: Filmes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Imagem,Titulo,Descricao,Duracao")] FilmeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var filme = await _context.Filme.FindAsync(model.Id);

                filme.Titulo = model.Titulo;
                filme.Descricao = model.Descricao;
                filme.Duracao = model.Duracao;

                if(model.Imagem != null)
                {
                    if(model.ExistingImage != null)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath, "Uploads", model.ExistingImage);
                        System.IO.File.Delete(filePath);
                    }

                    filme.Imagem = ProcessUploadedFile(model);
                }

                try
                {
                    _context.Update(filme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmeExists(filme.Id))
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
            return View();
        }

        // GET: Filmes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filme
                .FirstOrDefaultAsync(m => m.Id == id);

            var filmeViewModel = new FilmeViewModel()
            {
                Id = filme.Id,
                Titulo = filme.Titulo,
                Descricao = filme.Descricao,
                Duracao = filme.Duracao,
                ExistingImage = filme.Imagem
            };
            
            if (filme == null)
            {
                return NotFound();
            }

            return View(filmeViewModel);
        }

        // POST: Filmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var filme = await _context.Filme.FindAsync(id);
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", filme.Imagem);
            _context.Filme.Remove(filme);
            if (await _context.SaveChangesAsync() > 0)
            {
                if (System.IO.File.Exists(CurrentImage))
                {
                    System.IO.File.Delete(CurrentImage);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool FilmeExists(int id)
        {
            return _context.Filme.Any(e => e.Id == id);
        }

        private string ProcessUploadedFile(FilmeViewModel model)
        {
            string uniqueFileName = null;

            if (model.Imagem != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Imagem.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Imagem.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
