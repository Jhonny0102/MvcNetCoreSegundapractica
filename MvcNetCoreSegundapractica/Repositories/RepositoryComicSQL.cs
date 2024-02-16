using Microsoft.AspNetCore.Http.HttpResults;
using MvcNetCoreSegundapractica.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;

#region Procedures


//create procedure sp_create_comic
//(@nombre nvarchar(50), @imagen nvarchar(100), @descripcion nvarchar(50))
//as
//    declare @maxId int;
//select @maxId = MAX(IDCOMIC) + 1 from comics
//	insert into comics values (@maxId, @nombre, @imagen, @descripcion)
//go

#endregion

namespace MvcNetCoreSegundapractica.Repositories
{
    public class RepositoryComicSQL : IRepositoryComic
    {
        SqlConnection cn;
        SqlCommand com;
        DataTable tablaComic;
        public RepositoryComicSQL()
        {
            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;

            this.tablaComic = new DataTable();
            string sql = "select * from comics";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            ad.Fill(tablaComic);
        }

        public void DeleteComic(int idComic)
        {
            string sql = "delete from comics where idcomic=@idcomic";
            this.com.Parameters.AddWithValue("@idcomic",idComic);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public Comic FindComic(int idComic)
        {
            var consulta = from datos in this.tablaComic.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == idComic
                           select datos;
            var row = consulta.First();
            Comic comic = new Comic
            {
                IdComic = row.Field<int>("IDCOMIC"),
                Nombre = row.Field<string>("NOMBRE"),
                Imagen = row.Field<string>("IMAGEN"),
                Descripcion = row.Field<string>("DESCRIPCION")
            };
            return comic;
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComic.AsEnumerable() select datos;
            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
            {
                Comic comic = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")
                };
                comics.Add(comic);
            }
            return comics;
        }

        public void InsertComicConsulta(string nombre, string imagen, string descripcion)
        {
            var consulta = from datos in this.tablaComic.AsEnumerable() select datos;
            var maxId = (consulta.Max(z => z.Field<int>("IDCOMIC"))) + 1;
            string sql = "insert into comics values (@idcomic, @nombre, @imagen, @descripcion)";
            this.com.Parameters.AddWithValue("@idcomic",maxId);
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", imagen);
            this.com.Parameters.AddWithValue("@descripcion", descripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void InsertComicProcedure(string nombre, string imagen, string descripcion)
        {
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", imagen);
            this.com.Parameters.AddWithValue("@descripcion", descripcion);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "sp_create_comic";
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }


    }
}
