using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ejercicio1_CRUD_Clientes.Models;
using Ejercicio1_CRUD_Clientes.Services;
using System.Web.Services.Description;
using System.Net;
using System.Net.Http;
using Serilog;  

namespace Ejercicio1_CRUD_Clientes.Controllers
{
    public class ClienteController : Controller
    {
        ClienteService _servicio = new ClienteService();

        public async Task<ActionResult> Index()
        {
            try
            {
                Log.Information("Accediendo a la lista de clientes");
                var clientes = await _servicio.ObtenerTodosAsync();
                Log.Information("Lista de clientes cargada correctamente");
                return View(clientes);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener los clientes");
                TempData["Error"] = "Ocurrió un error al cargar la lista de clientes.";
                return View(new List<Cliente>());
            }
        }

        public ActionResult Create()
        {
            Log.Information("Accediendo a la vista de creación de cliente");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _servicio.AgregarAsync(cliente);
                    Log.Information($"Cliente agregado: {cliente.RazonSocial} - {cliente.Cuit}");
                    TempData["Success"] = "Cliente creado exitosamente.";
                    return RedirectToAction("Index");
                }
                Log.Warning("Modelo inválido al crear cliente");
                return View(cliente);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                Log.Error(ex, "Error al crear el cliente.Cliente ya existente");
                return View(cliente);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al crear el cliente");
                TempData["Error"] = "Ocurrió un error al intentar crear el cliente.";
                return View(cliente);
            }
        }

        public async Task<ActionResult> Edit(string cuit)
        {
            try
            {
                Log.Information($"Accediendo a la vista de edición del cliente con CUIT: {cuit}");
                var cliente = await _servicio.ObtenerPorCuitAsync(cuit);
                if (cliente == null)
                {
                    Log.Warning($"Cliente con CUIT {cuit} no encontrado");
                    TempData["Error"] = "Cliente no encontrado.";
                    return RedirectToAction("Index");
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error al cargar el cliente con CUIT {cuit}");
                TempData["Error"] = "Ocurrió un error al cargar el cliente.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _servicio.EditarAsync(cliente);
                    Log.Information($"Cliente editado exitosamente: {cliente.Cuit}");
                    TempData["Success"] = "Cliente editado exitosamente.";
                    return RedirectToAction("Index");
                }

                Log.Warning($"Modelo inválido al editar cliente: {cliente.Cuit}");
                return View(cliente);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al editar el cliente");
                TempData["Error"] = "Ocurrió un error al intentar editar el cliente.";
                return View(cliente);
            }
        }

        public async Task<ActionResult> Delete(string cuit)
        {
            try
            {
                Log.Information($"Accediendo a la vista de eliminación del cliente con CUIT: {cuit}");
                var cliente = await _servicio.ObtenerPorCuitAsync(cuit);
                if (cliente == null)
                {
                    Log.Warning($"Cliente con CUIT {cuit} no encontrado para eliminar");
                    TempData["Error"] = "Cliente no encontrado.";
                    return RedirectToAction("Index");
                }

                return View(cliente);
            }
            catch (Exception)
            {
                Log.Error("Error al intentar cargar los datos para eliminar el cliente");
                TempData["Error"] = "Ocurrió un error al cargar el cliente.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string cuit)
        {
            try
            {
                await _servicio.EliminarAsync(cuit);
                Log.Information($"Cliente con CUIT {cuit} eliminado exitosamente");
                TempData["Success"] = "Cliente eliminado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                Log.Error($"Error al intentar eliminar el cliente con CUIT {cuit}");
                TempData["Error"] = "Ocurrió un error al intentar eliminar el cliente.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerRazonSocialPorCuit(string cuit)
        {
            if (string.IsNullOrEmpty(cuit))
            {
                Log.Warning("Cuit vacío recibido en ObtenerRazonSocialPorCuit");
                return Json("", JsonRequestBehavior.AllowGet);
            }

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string url = $"https://sistemaintegracomex.com.ar/Account/GetNombreByCuit?cuit={cuit}";
                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string razonSocial = await response.Content.ReadAsStringAsync();
                        Log.Information($"Razon social obtenida para el CUIT: {cuit}");
                        return Json(razonSocial, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Log.Warning($"No se obtuvo una respuesta exitosa para el CUIT {cuit}");
                        return Json("", JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error al obtener la razón social");
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}
