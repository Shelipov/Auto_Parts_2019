using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Auto_Parts_2019.Models;
using Auto_Parts_2019.Data;
using Auto_Parts_2019.Models.Parts;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Auto_Parts_2019.Helpers;
using System.Text.RegularExpressions;


namespace Auto_Parts_2019.Controllers
{
    public class HomeController : Controller
    {
        
        public ApplicationDbContext db;
        private Repository<Part> Repo;
        IPartsRepository repo;
        public HomeController(ApplicationDbContext db,IPartsRepository _repo)
        {
            this.db = db;
            repo = _repo;
            this.Repo = new Repository<Part>(this.db);
        }

        public  IActionResult Index(int page = 1)
        {
            
            return View(GetParts(page));

        }
        [Route("addtocart")]
        public IActionResult AddToCart(int PartID, int UserID)
        {

            return View("Index");
        }
        //[Route("search")]
        [HttpPost("search")]
        public IActionResult Search(string number)
        {
            var num = Search_num(number);
            return View(num);
        }
        [Route("partaddexel")]
        public IActionResult PartAddExel()
        {

            return View("Index");
        }

        public async Task<IActionResult> SendMessage(int id, string text, string name)
        {
            EmailService emailService = new EmailService();
            string email = (from i in db.Users
                            where i.Id == id.ToString()
                            select i.Email).FirstOrDefault();
            await emailService.SendEmailAsync(email, text, name);
            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private IEnumerable<Part> Search_num(string number)
        {
            string[] warn = { @"'","--","Select","From","Create","Update","Delete","Where","and","or","+","=","database","insert" };
            foreach (var i in warn)
            {
                var str = i.ToUpper();
                number = number.Replace(str, "").ToUpper();
            }

            return repo.GetParts(number);
            
        }

        private IndexViewModel GetParts(int page)
        {

            IndexViewModel returnModel = new IndexViewModel();
            int pageSize = 10;
            try
            {
                IQueryable<Part> source = repo.GetParts().AsQueryable();
                //double d =Convert.ToDouble(repo.GetCourseEuro());
                double course = 30.3; //=Convert.ToDouble(repo.GetCourseEuro());
                foreach (var i in source) { i.Price = System.Math.Round((i.Price * course),2); }
                var count = source.Count();
                var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
                IndexViewModel viewModel = new IndexViewModel
                {
                    PageViewModel = pageViewModel,
                    Parts = items
                };
                returnModel = viewModel;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return returnModel;
        }
        public JsonResult GetData()
        {
            // создадим список данных
            List<Station> stations = new List<Station>();
            stations.Add(new Station()
            {
                Id = 1,
                PlaceName = "г. Киев ул. Дегтяревская 21",
                GeoLat = 30.459701739011,
                GeoLong = 50.4625697943559,
                
            });
            stations.Add(new Station()
            {
                Id = 2,
                PlaceName = "г. Днепр ул. Братьев Бестужевых 7 ",
                GeoLat = 35.005002,
                GeoLong = 48.480340,
               
            });
            

            return Json(stations);
        }
    }
}
