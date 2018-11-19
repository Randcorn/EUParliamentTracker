using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EuropeanParliamentTracker.Controllers
{
    public class ExperimentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}