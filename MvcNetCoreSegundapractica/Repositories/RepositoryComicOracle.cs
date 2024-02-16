using Microsoft.AspNetCore.Http.HttpResults;
using MvcNetCoreSegundapractica.Models;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System;
using System.Data;

#region Procedure

//create or replace procedure sp_create_comic
//(
//p_nombre COMICS.NOMBRE%TYPE,
//p_imagen COMICS.IMAGEN%TYPE,
//p_descripcion COMICS.DESCRIPCION%TYPE
//)
//as
//p_idComic COMICS.IDCOMIC % TYPE;
//begin
//  select MAX(IDCOMIC)+1 into p_idComic from comics;
//insert into comics values (p_idComic, p_nombre, p_imagen, p_descripcion);
//commit;
//end;

#endregion

namespace MvcNetCoreSegundapractica.Repositories
{
    public class RepositoryComicOracle : IRepositoryComic
    {
        OracleConnection cn;
        OracleCommand com;
        DataTable tablaComic;
        public RepositoryComicOracle()
        {
            string connectionString = @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True; User Id=SYSTEM; Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;

            string sql = "select * from comics";
            this.tablaComic = new DataTable();
            OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
            ad.Fill(tablaComic);
        }

        public void DeleteComic(int idComic)
        {
            string sql = "delete from comics where idcomic=:idcomic";
            OracleParameter pamIdComic = new OracleParameter(":idcomic", idComic);
            this.com.Parameters.Add(pamIdComic);
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
            string sql = "insert into comics values (:idcomic, :nombre, :imagen, :descripcion)";
            OracleParameter pamIdComic = new OracleParameter(":idcomic", maxId);
            this.com.Parameters.Add(pamIdComic);
            OracleParameter pamNombre = new OracleParameter(":nombre", nombre);
            this.com.Parameters.Add(pamNombre);
            OracleParameter pamImagen = new OracleParameter(":imagen", imagen);
            this.com.Parameters.Add(pamImagen);
            OracleParameter pamDescripcion = new OracleParameter(":descripcion", descripcion);
            this.com.Parameters.Add(pamDescripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void InsertComicProcedure(string nombre, string imagen, string descripcion)
        {
            OracleParameter pamNombre = new OracleParameter(":nombre", nombre);
            this.com.Parameters.Add(pamNombre);
            OracleParameter pamImagen = new OracleParameter(":imagen", imagen);
            this.com.Parameters.Add(pamImagen);
            OracleParameter pamDescripcion = new OracleParameter(":descripcion", descripcion);
            this.com.Parameters.Add(pamDescripcion);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "sp_create_comic";
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
