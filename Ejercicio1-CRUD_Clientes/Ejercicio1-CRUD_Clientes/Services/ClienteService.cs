using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ejercicio1_CRUD_Clientes.Context;
using Ejercicio1_CRUD_Clientes.Models;

namespace Ejercicio1_CRUD_Clientes.Services
{
    public class ClienteService
    {
        private readonly AppDbContext db = new AppDbContext();

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            try
            {
                return await db.Clientes.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Cliente> ObtenerPorCuitAsync(string cuit)
        {
            try
            {
                return await db.Clientes.FindAsync(cuit);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AgregarAsync(Cliente cliente)
        {
            try
            {
                db.Clientes.Add(cliente);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task EditarAsync(Cliente cliente)
        {
            try
            {
                db.Entry(cliente).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task EliminarAsync(string cuit)
        {
            try
            {
                var cliente = await db.Clientes.FindAsync(cuit);
                if (cliente != null)
                {
                    db.Clientes.Remove(cliente);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}