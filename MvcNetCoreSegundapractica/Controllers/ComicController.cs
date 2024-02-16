using Microsoft.AspNetCore.Mvc;
using MvcNetCoreSegundapractica.Models;
using MvcNetCoreSegundapractica.Repositories;

namespace MvcNetCoreSegundapractica.Controllers
{
    public class ComicController : Controller
    {
        IRepositoryComic repo;

        public ComicController(IRepositoryComic repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Comic> comics = this.repo.GetComics();
            return View(comics);
        }

        //Details, buscador de comics
        public IActionResult Details()
        {
            List<Comic> comics = this.repo.GetComics();
            ViewData["COMICS"] = comics;
            return View();
        }
        [HttpPost]
        public IActionResult Details(int idComic)
        {
            List<Comic> comics = this.repo.GetComics();
            ViewData["COMICS"] = comics;
            Comic comic = this.repo.FindComic(idComic);
            return View(comic);
        }

        //Create con consultas normales.
        public IActionResult CreateConsulta()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateConsulta(string nombre, string imagen, string descripcion)
        {
            this.repo.InsertComicConsulta(nombre, imagen,descripcion);
            return RedirectToAction("Index");
        }

        //create con Procedures
        public IActionResult CreateProcedure()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProcedure(string nombre, string imagen, string descripcion)
        {
            this.repo.InsertComicProcedure(nombre, imagen, descripcion);
            return RedirectToAction("Index");
        }

        //Delete
        public IActionResult Delete(int idComic)
        {
            Comic comic = this.repo.FindComic(idComic);
            return View(comic);
        }

        public IActionResult DeletePost(int idComic)
        {
            this.repo.DeleteComic(idComic);
            return RedirectToAction("Index");
        }
    }
}
