using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exercise.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class Dashboard : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
