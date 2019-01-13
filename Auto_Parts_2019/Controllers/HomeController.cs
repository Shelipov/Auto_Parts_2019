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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace Auto_Parts_2019.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        public ApplicationDbContext db;
        private Repository<Part> Repo;
        IPartsRepository repo;
        public HomeController(ApplicationDbContext db,IPartsRepository _repo, SignInManager<IdentityUser> _signInManager, UserManager<IdentityUser> _userManager)
        {
            this.db = db;
            repo = _repo;
            this.Repo = new Repository<Part>(this.db);
            signInManager = _signInManager;
            userManager = _userManager;
        }

        public  IActionResult Index(int page = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(GetParts(page,userManager.GetUserId(User)));
            }
            else
                return View(GetParts(page));

        }
        
        [Route("addtocart")]
        public IActionResult AddToCart(int PartID, string UserID)
        {
            return View(AddParts(PartID, UserID));
            //return View("AddToCart");
        }
        [Route("addtocart")]
        public IActionResult AddToCart(List<Order> orders)
        {
            
            return View("AddToCart");
        }

        //[Route("search")]
        [HttpPost("search")]
        public IActionResult Search(string number)
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(Search_num(number,userManager.GetUserId(User)));
            }
            else
                return View(Search_num(number));
            
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

            var parts= repo.GetParts(number);
            
            double course = Convert.ToDouble(repo.GetCourseEuro());
            var disc = from i in db.DefaultDiscounts
                       select i.TheDefaultDiscount;            
                foreach (var i in parts)
                {
                    if (disc.FirstOrDefault() != 0)
                        i.Price = (System.Math.Round((i.Price * course), 2)) / ((disc.FirstOrDefault() / 100) + 1);
                    else
                        i.Price = i.Price;
                }
            return parts;
        }
        private IEnumerable<Part> Search_num(string number,string UserID)
        {
            string[] warn = { @"'", "--", "Select", "From", "Create", "Update", "Delete", "Where", "and", "or", "+", "=", "database", "insert" };
            foreach (var i in warn)
            {
                var str = i.ToUpper();
                number = number.Replace(str, "").ToUpper();
            }
            var parts = repo.GetParts(number);
            var user = repo.GetUser(UserID);
            double course = Convert.ToDouble(repo.GetCourseEuro());
            var disc = from i in db.DefaultDiscounts
                       select i.TheDefaultDiscount;
            
                foreach (var i in parts)
                {
                if (user.Discount != 0)
                { i.Price = (System.Math.Round((i.Price * course), 2)) / ((user.Discount / 100) + 1); }
                else
                {
                    if (disc.FirstOrDefault() != 0)
                        i.Price = (System.Math.Round((i.Price * course), 2)) / ((disc.FirstOrDefault() / 100) + 1);
                    else
                        i.Price = i.Price;
                }
            }
            return parts;
        }

        private IndexViewModel GetParts(int page)
        {

            IndexViewModel returnModel = new IndexViewModel();
            int pageSize = 10;
            try
            {
                IQueryable<Part> source = repo.GetParts().AsQueryable();
                double course =Convert.ToDouble(repo.GetCourseEuro());
                var disc = from i in db.DefaultDiscounts
                           select i.TheDefaultDiscount;
                foreach (var i in source)
                {
                    i.Price = (System.Math.Round((i.Price * course), 2)) / ((disc.FirstOrDefault() / 100) + 1);
                }
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
        private IndexViewModel GetParts(int page,string userid)
        {
            var user = repo.GetUser(userid);
            IndexViewModel returnModel = new IndexViewModel();
            int pageSize = 10;
            try
            {
                IQueryable<Part> source = repo.GetParts().AsQueryable();
                double course =Convert.ToDouble(repo.GetCourseEuro());
                foreach (var i in source)
                {
                    i.Price = (System.Math.Round((i.Price * course), 2)) / ((user.Discount / 100) + 1);
                }
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
        private List<Order> AddParts(int PartID, string UserID)
        {
            List<Order> orders = new List<Order>();
            var part = repo.GetParts(PartID);
            var user = repo.GetUser(UserID);
            if (user.Discount != 0)
                part.Price = part.Price / ((user.Discount / 100)+1);
            else
                part.Price = part.Price;
            Order order = new Order();
            order.Address = user;
            order.Part = part;
            orders.Add(order);
            return orders;
        }
    }
}
