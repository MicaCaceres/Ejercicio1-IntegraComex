﻿using System.Web;
using System.Web.Mvc;

namespace Ejercicio1_CRUD_Clientes
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
