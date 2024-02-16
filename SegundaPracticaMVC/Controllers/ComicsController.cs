using Microsoft.AspNetCore.Mvc;
using SegundaPracticaMVC.Models;
using SegundaPracticaMVC.Repositories;

namespace SegundaPracticaMVC.Controllers
{
    public class ComicsController : Controller
    {
        private IRepositoyComics repo;

        public ComicsController(IRepositoyComics repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            List<Comic> list = this.repo.GetComics();
            return View(list);
        }

        public IActionResult Insertar() { 
            return View();
        }

        [HttpPost]

        public IActionResult Insertar(Comic comic)
        {
            this.repo.InsertarComic( comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }

        public IActionResult BuscadorComics()
        {
            List<Comic> comics = this.repo.GetComics();
            ViewData["nombres"] = this.repo.NombreComic();
                return View(comics);
            
        }

        [HttpPost]
        public IActionResult BuscadorComics(string nombre)
        {
            ViewData["nombres"] = this.repo.NombreComic();
            List<Comic> comic = this.repo.BuscarComics(nombre);
            return View(comic);
        }

        
        public IActionResult Details(int id)
        {
            Comic comic = this.repo.FindComic(id);
            return View(comic);
        }
        public IActionResult Delete(int id)
        {
            Comic comic = this.repo.FindComic(id);
            return View(comic);
        }
        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            this.repo.DeleteComic(id);
            return RedirectToAction("Index");
        }

        public IActionResult InsertarLamb()
        {
            return View();
        }

        [HttpPost]

        public IActionResult InsertarLamb(Comic comic)
        {
            this.repo.InsertarComic(comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }
    }
}
