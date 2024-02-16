using SegundaPracticaMVC.Models;

namespace SegundaPracticaMVC.Repositories
{
    public interface IRepositoyComics
    {
        List<Comic> GetComics();

        void InsertarComic( string nombre, string imagen, string descripcion);

        void InsertarComicLambda(string nombre, string imagen, string descripcion);

        Comic FindComic(int id);

        List<Comic> BuscarComics(string nombre);

        void DeleteComic(int id);

        List<string> NombreComic();

    }
}
