using Microsoft.AspNetCore.Http.HttpResults;
using Oracle.ManagedDataAccess.Client;
using SegundaPracticaMVC.Models;
using System.Collections.Generic;
using System.Data;
using System.Numerics;

#region
//CREATE OR REPLACE PROCEDURE SP_INSERTCOMIC (
//   NOMBRE IN VARCHAR2,
//  IMAGEN IN VARCHAR2,
//DESCRIPCION IN VARCHAR2
//)
//AS
//nextId NUMBER;
//BEGIN
//SELECT NVL(MAX(IDCOMIC), 0) + 1 INTO nextId FROM COMICS;

//INSERT INTO COMICS (IDCOMIC, NOMBRE, IMAGEN, DESCRIPCION)
//VALUES(nextId, NOMBRE, IMAGEN, DESCRIPCION);

//COMMIT;
//end;
#endregion

namespace SegundaPracticaMVC.Repositories
{
    public class RepositoryComicsOracle : IRepositoyComics
    {
        private DataTable tablaComics;
        private OracleConnection cn;
        private OracleCommand com;
        public RepositoryComicsOracle() {
            string connectionString = @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True;User Id=SYSTEM;Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = cn;
            string sql = "select * from Comics";
            OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
            this.tablaComics= new DataTable();
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
            string sql = "DELETE FROM Comics WHERE IDCOMIC = :id";

            using (OracleCommand command = new OracleCommand(sql, this.cn))
            {
                command.Parameters.Add(":id", OracleDbType.Int32).Value = id;

                this.cn.Open();
                int rowsAffected = command.ExecuteNonQuery();
                this.cn.Close();

               
            }
        }

        public Comic FindComic(int id)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == id
                           select datos;
            var row = consulta.First();
            Comic comic = new Comic
            {

                Id = row.Field<int>("IDCOMIC"),
                Nombre = row.Field<string>("NOMBRE"),
                Imagen = row.Field<string>("IMAGEN"),
                Descripcion = row.Field<string>("DESCRIPCION"),
            };
            return comic;
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

        public List<Comic> GetComicsNombre(string nombre)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<string>("NOMBRE").ToUpper() == nombre.ToUpper()
                           select datos;

            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
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
        }

        public void InsertarComic(string nombre, string imagen, string descripcion)
        {

            
            OracleParameter pamNombrer = new OracleParameter(":NOMBRE", nombre);
            OracleParameter pamimagen = new OracleParameter(":IMAGEN", imagen);
            OracleParameter pamdescripcion = new OracleParameter(":DESCRIPCION", descripcion);
            this.com.Parameters.Add(pamNombrer);
            this.com.Parameters.Add(pamimagen);
            this.com.Parameters.Add(pamdescripcion);
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

            string sql = "insert into COMICS VALUES (:idcomics,:nombre,:imagen,:descripcion)";

            OracleParameter pamid = new OracleParameter(":idcomics", nextid);
            OracleParameter pamNombrer = new OracleParameter(":NOMBRE", nombre);
            OracleParameter pamimagen = new OracleParameter(":IMAGEN", imagen);
            OracleParameter pamdescripcion = new OracleParameter(":DESCRIPCION", descripcion);
            this.com.Parameters.Add(pamid);
            this.com.Parameters.Add(pamNombrer);
            this.com.Parameters.Add(pamimagen);
            this.com.Parameters.Add(pamdescripcion);
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
