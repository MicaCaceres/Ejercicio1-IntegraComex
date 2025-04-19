using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ejercicio1_CRUD_Clientes.Models;
using System.Web.Services.Description;
using Ejercicio1_CRUD_Clientes.Services;
using System.Net;
using System.Net.Http;

namespace Ejercicio1_CRUD_Clientes.Controllers
{
    public class ClienteController : Controller
    {   
        ClienteService _servicio =new ClienteService();
        public async Task<ActionResult> Index()
        {
            try
            {
                var clientes = await _servicio.ObtenerTodosAsync();
                return View(clientes);
            }
            catch (Exception ex)
            {
                // logger.Error(ex, "Error al obtener los clientes");
                TempData["Error"] = "Ocurrió un error al cargar la lista de clientes.";
                return View(new List<Cliente>());
            }
        }

        public ActionResult Create()
        {
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
                    // logger.Info($"Cliente agregado: {cliente.Nombre}");
                    TempData["Success"] = "Cliente creado exitosamente.";
                    return RedirectToAction("Index");
                }

                // logger.Warn("Modelo inválido al crear cliente");
                return View(cliente);
            }
            catch (Exception ex)
            {
                // logger.Error(ex, "Error al crear el cliente");
                TempData["Error"] = "Ocurrió un error al intentar crear el cliente.";
                return View(cliente);
            }
        }

        public async Task<ActionResult> Edit(string cuit)
        {
            try
            {
                var cliente = await _servicio.ObtenerPorCuitAsync(cuit);
                if (cliente == null)
                {
                    TempData["Error"] = "Cliente no encontrado.";
                    return RedirectToAction("Index");
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                // logger.Error(ex, $"Error al cargar el cliente con CUIT {cuit}");
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
                    TempData["Success"] = "Cliente editado exitosamente.";
                    return RedirectToAction("Index");
                }

                // logger.Warn($"Modelo inválido al editar cliente: {cliente.CUIT}");
                return View(cliente);
            }
            catch (Exception ex)
            {
                // logger.Error(ex, "Error al editar el cliente");
                TempData["Error"] = "Ocurrió un error al intentar editar el cliente.";
                return View(cliente);
            }
        }
        public async Task<ActionResult> Delete(string cuit)
        {
            try
            {
                var cliente = await _servicio.ObtenerPorCuitAsync(cuit);
                if (cliente == null)
                {
                    TempData["Error"] = "Cliente no encontrado.";
                    return RedirectToAction("Index");
                }

                return View(cliente);
            }
            catch (Exception)
            {
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
                TempData["Success"] = "Cliente eliminado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error al intentar eliminar el cliente.";
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public async Task<ActionResult> ObtenerRazonSocialPorCuit(string cuit)
        {
            if (string.IsNullOrEmpty(cuit))
                return Json("", JsonRequestBehavior.AllowGet);

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string url = $"https://sistemaintegracomex.com.ar/Account/GetNombreByCuit?cuit={cuit}";
                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string razonSocial = await response.Content.ReadAsStringAsync();
                        return Json(razonSocial, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("", JsonRequestBehavior.AllowGet);
                    }
                }
                catch
                {
                    // Manejo de error (por ejemplo, si no hay conexión)
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

    }
}