using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //
        // GET: /HelloWorld/

        //public string Index()
        //{
        //    return "This is my default action...";
        //}

        //
        // GET: /Helloworld/Welcome/
        // Requires using System.Text.Encodings.Web;
        //public string Welcome(string name, int numTimes = 1)
        public string Welcome(string name, int ID = 1)
        {
            //return "This is the Welcome action method...";
            //return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
            return HtmlEncoder.Default.Encode($"Hello {name}, ID: {ID}");
        }
    }
}
