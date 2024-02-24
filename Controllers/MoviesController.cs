using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaManager_X.Models.Cinema;
using CinemaManager_X.Models.ViewModels;

namespace CinemaManager_X.Controllers
{
    public class MoviesController : Controller
    {
        private readonly CinemaDbContext _context;

        public MoviesController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var cinemaDbContext = _context.Movies.Include(m => m.Producer);
            return View(await cinemaDbContext.ToListAsync());
        }

        public async Task<IActionResult> MoviesAndTheirProds()
        {
            var cinemaDbContext = _context.Movies.Include(m => m.Producer);
            return View(await cinemaDbContext.ToListAsync());
        }

        public  IActionResult MoviesAndTheirProds_UsingModel()
        {
            var movies = _context.Movies.ToList();
            var prods = _context.Producers.ToList();
            var querry_res = from m in movies
                             join p in prods on m.ProducerId equals  p.Id 
                             select new ProdMovie
                             {
                                 mTitle = m.Title,
                                 mGenre = m.Genre,
                                 pName = p.Name,
                                 pNat = p.Nationalty

                             };



            return View(querry_res);
        }


        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Genre,ProducerId")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id", movie.ProducerId);
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id", movie.ProducerId);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Genre,ProducerId")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id", movie.ProducerId);
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
        public IActionResult SearchBy2() {
            var movies = _context.Movies.AsQueryable();
            ViewBag.Genre = movies.Select(movies => movies.Genre).ToList();
            return View(movies);
        
        }
        [HttpPost]
        public IActionResult SearchBy2(string genre, string title)
        {
            var movies = _context.Movies.AsQueryable();
            ViewBag.Genre = movies.Select(movies => movies.Genre).ToList();
          
            if (!string.IsNullOrEmpty(genre))
            {
                movies = movies.Where(m => m.Genre == genre);

            }

            if (genre == "All")
            {
                 movies = _context.Movies.AsQueryable();
            }
            if (!string.IsNullOrEmpty(title))
            {
                movies = movies.Where(m => m.Title.Contains(title));
            }
            return View(movies);
        }


        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movies == null)
            {
                return Problem("Entity set 'CinemaDbContext.Movies'  is null.");
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
