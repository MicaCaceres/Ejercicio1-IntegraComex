using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Ejercicio1_CRUD_Clientes.Models;

namespace Ejercicio1_CRUD_Clientes.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
    }
}