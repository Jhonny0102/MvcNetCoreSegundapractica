using MvcNetCoreSegundapractica.Models;

namespace MvcNetCoreSegundapractica.Repositories
{
    public interface IRepositoryComic
    {
        List<Comic> GetComics();
        void InsertComicConsulta(string nombre, string imagen, string descripcion);
        void InsertComicProcedure(string nombre, string imagen, string descripcion);
        Comic FindComic(int idComic);
        void DeleteComic(int idComic);
    }
}
