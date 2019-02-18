using System.Collections.Generic;
using System.Web.Mvc;
using Test.Data.Entities;
using Test.Model.Models;
using Test.Model.Services.Abstract;
using Test.Model.Services.Concrete;
namespace Test.Web.Controllers
{
    public class HomeController : Controller
    {
        private IParseService _parseService;

        //HomeController(IParseService parseService)
        //{
        //    _parseService = parseService;
        //}

        public HomeController()
        {
            _parseService = new ParseService();
        }
        public ActionResult Index()
        {
            List<Vacancy> vacancies = _parseService.GetVacancies();
            ViewBag.Vacancies = vacancies;
            return View();
        }

        [HttpPost]
        public ActionResult Parse(string url)
        {
            List<Vacancy> vacancies = _parseService.Parse(url);
            ViewBag.Vacancies = vacancies;
            return View("~/Views/Home/Index.cshtml");
          //  return RedirectToAction("Index", "Home");
        }


    }
}