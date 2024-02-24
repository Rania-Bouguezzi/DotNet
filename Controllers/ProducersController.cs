using CinemaManager_X.Models.Cinema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CinemaManager_X.Controllers
{

  
    public class ProducersController : Controller
    {
         CinemaDbContext ctx;
        public ProducersController(CinemaDbContext context)
        {
            ctx = context;
        }
        // GET: ProducersController
        public ActionResult Index()
        {
            return View(ctx.Producers.ToList());
        }

        public ActionResult ProdsAndTheirMoviesdex()
        {
            var movies = ctx.Movies.ToList();
            return View(ctx.Producers.ToList());
        }


        public IActionResult MyMovies(int id)
        {
            var prods = ctx.Producers.ToList();
            var movies = ctx.Movies.ToList();
            var res = from m in movies where m.ProducerId == id select m;
            var res2 = ctx.Movies.Where(m => m.ProducerId == id);
            return View(res2);



        }

        public IActionResult SearchByTitle(string s)
        {  // if (s.IsNullOrEmpty())
               // return View(ctx.Producers.ToList());

            var prods = ctx.Producers.ToList();
            var movies = ctx.Movies.ToList();
            // var res = from m in movies where m.ProducerId == id select m;
            var res2 = ctx.Movies.Where(m => m.Title.Contains(s));
            return View(res2);



        }
        // GET: ProducersController/Details/5
        public ActionResult Details(int id)
        {
            Producer p = ctx.Producers.Find(id);
            return View(p);
          
        }

        // GET: ProducersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProducersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Producer prod)
        {
            try
            {
                ctx.Producers.Add(prod);
                ctx.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProducersController/Edit/5
        public ActionResult Edit(int id)
        {
            Producer p = ctx.Producers.Find(id);
            return View(p);
        }

        // POST: ProducersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection , Producer prod)
        {
            try
            {
                ctx.Producers.Update(prod);
                ctx.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProducersController/Delete/5
        public ActionResult Delete(int id)
        {
            Producer p = ctx.Producers.Find(id);
            return View(p);
       
        }

        // POST: ProducersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection , Producer prod)
        {
            try
            {
                ctx.Producers.Remove(prod);
                ctx.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



   
    }
}
