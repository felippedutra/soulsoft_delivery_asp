using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Controllers
{
    public class TiposEnderecosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int id = 0)
        {
            if (id != 0)
            {
                return View();
            }
            else
            {
                return View();
            }
        }
    }
}
