using Microsoft.AspNetCore.Http.HttpResults;
using SegundaPracticaMVC.Models;
using System.Data;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

#region
//create procedure SP_INSERTCOMIC
//(@NOMBRE NVARCHAR(50), @IMAGEN NVARCHAR(200), @DESCRIPCION NVARCHAR(100))
//AS
//declare @nextId int
//select @nextId = MAX(IDCOMIC) +1 from COMICS
//insert into COMICS values (@nextId, @NOMBRE, @IMAGEN, @DESCRIPCION)

//GO
#endregion

namespace SegundaPracticaMVC.Repositories
{
    public class RepositoryComicsSQLServer : IRepositoyComics
    {

        private DataTable tablaComics;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryComicsSQLServer()
        {
            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=Hospitales;Persist Security Info=True;User ID=SA;Password=MCSD2023;";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = cn;
            this.tablaComics = new DataTable();
            string sql = "select * from Comics";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            ad.Fill(this.tablaComics);

        }

        public List<Comic> BuscarComics(string nombre)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() where datos.Field<string>("NOMBRE") == nombre select datos;
            List<Comic> comics = new List<Comic>();

            foreach (var row in consulta)
            {

                Comic comic = new Comic
                {
                    Id = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION"),
                };
                comics.Add(comic);

            }
            return comics;
        }

        public void DeleteComic(int id)
        {
            string sql = "delete from Comics where idcomic=@id";
            this.com.Parameters.AddWithValue("@id", id);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();

        }

        public Comic FindComic(int id)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == id
                           select datos;
            var row = consulta.FirstOrDefault();
            if (row != null)
            {
                Comic comic = new Comic
                {
                    Id = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION"),
                };
                return comic;
            }
            else
            {
                
                throw new InvalidOperationException("No se encontró ningún cómic con el ID especificado.");
            }

        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() select datos;
            List<Comic> comics = new List<Comic>();

            foreach (var row in consulta)
            {

                Comic comic = new Comic
                {
                    Id = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION"),
                };
                comics.Add(comic);

            }
            return comics;
        }

       

            public void InsertarComic(string nombre, string imagen, string descripcion)
            {
                this.com.Parameters.AddWithValue("@NOMBRE", nombre);
                this.com.Parameters.AddWithValue("@IMAGEN", imagen);
                this.com.Parameters.AddWithValue("@DESCRIPCION", descripcion);
                this.com.CommandType = CommandType.StoredProcedure;
                this.com.CommandText = "SP_INSERTCOMIC";

                this.cn.Open();
                int af = this.com.ExecuteNonQuery();
                this.cn.Close();
                this.com.Parameters.Clear();
            }

        public void InsertarComicLambda(string nombre, string imagen, string descripcion)
        {
            int nextid = this.tablaComics.AsEnumerable().Max(x => x.Field<int>("IDCOMIC")) + 1;

            string sql = "insert into COMICS VALUES (@idcomics,@nombre,@imagen,@descripcion)";

            this.com.Parameters.AddWithValue("@idcomics", nextid);
            this.com.Parameters.AddWithValue("@NOMBRE", nombre);
            this.com.Parameters.AddWithValue("@IMAGEN", imagen);
            this.com.Parameters.AddWithValue("@DESCRIPCION", descripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public List<string> NombreComic()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() select datos;
            List<string> nombre = new List<string>();

            foreach (var row in consulta)
            {

                string nom = row.Field<string>("NOMBRE");
                nombre.Add(nom);

            }
            return nombre;
        }
    }
    }

