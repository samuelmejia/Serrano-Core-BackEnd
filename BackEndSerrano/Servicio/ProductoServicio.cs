﻿using BackEndSerrano.ConexionDB;
using BackEndSerrano.Model.Producto;
using Dapper;

namespace BackEndSerrano.Servicio
{
    public class ProductoServicio:ConexionDapper
    {
         readonly IConfiguration _configuration;

        #region ctor
        public ProductoServicio(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
           
        }

        #endregion

        #region Metodos
        public async Task<IEnumerable<ProductosModel>> GetAllProducto()
        {
            try
            {
                dapper.Open();
                var sql = "select * from TopGeneralProductos";              
                return await Task.FromResult(dapper.Query<ProductosModel>(sql));
            }
            catch (Exception ex)
            {

                return  (IEnumerable<ProductosModel>)Task.FromException(ex);
            }
            finally { dapper.Close(); }
        }

        public async Task<IEnumerable<ProductosModel>> GetProducto(string busqueda) {
            try
            {
                dapper.Open();
                var sql = "[dbo].[PGProductos] @busqueda";
                return await Task.FromResult(dapper.Query<ProductosModel>(sql, new {busqueda=busqueda }));
            }
            catch (Exception)  
            {
                 
                throw;

            }
            finally { dapper.Close(); }
        }

        public async Task<IEnumerable<ProductoExistenciaModel>> GetExistencias(string busqueda) {
            try
            {
                dapper.Open();
                var sql = "[dbo].[PSExistenciaProducto] @busqueda";
                return await Task.FromResult(dapper.Query<ProductoExistenciaModel>(sql, new { busqueda = busqueda }));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<ProveedoresModel>> GetProveedores()
        {
            try
            {
                dapper.Open();
                string sql = "select" +
                    "*" +
                    "from dbo.VProveedores";
                var result = dapper.Query<ProveedoresModel>(sql);

                return await Task.FromResult(result);
            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }
        }

        public async Task<IEnumerable<ProveedorMarcaModel>> GetProveedorMarca(string IDProveedor)
        {
            try
            {
                dapper.Open();
                string sql = "select" +
                    "*" +
                    "from VProveedorMarca where IDProveedor=@IDProveedor";
                var result = dapper.Query<ProveedorMarcaModel>(sql, new { IDProveedor= IDProveedor });

                return await Task.FromResult(result);
;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

                dapper.Close();
            }
        }



        #endregion
    }
}
