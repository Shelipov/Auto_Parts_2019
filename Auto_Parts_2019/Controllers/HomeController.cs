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
using Auto_Parts_2019.Models.Parts.DTO;
using Microsoft.AspNetCore.Http;

namespace Auto_Parts_2019.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        public ApplicationDbContext db;
       
        IPartsRepository repo;
        List<_OrderDTO> orders = new List<_OrderDTO>();
        const string SessionId = "_Id";
        public HomeController(ApplicationDbContext db,IPartsRepository _repo, SignInManager<IdentityUser> _signInManager, UserManager<IdentityUser> _userManager)
        {
           
            this.db = db;
            repo = _repo;
            
            signInManager = _signInManager;
            userManager = _userManager;
            
        }

        //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
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
        public IActionResult AddToCart(int PartID )
        {

            if (User.Identity.IsAuthenticated)
            {
                AddParts(PartID, userManager.GetUserId(User));
                return Redirect("~/basket");
            }
            else
                return Redirect("~/Identity/Account/Register");

        }
        [Route("basket")]
        public IActionResult Basket()
        {
            if (User.Identity.IsAuthenticated)
            {
                var order = from i in db._OrdersDTO
                            where i.AddressID == userManager.GetUserId(User)
                            select i;
                return View(order);
            }
            else
                return Redirect("~/Identity/Account/Register");
        }

        [Route("editbasket")]
        [HttpPost]
        public void EditBasket(int PartID,int Quantity)
        {           
                var order = from i in db._OrdersDTO
                            where i.AddressID == userManager.GetUserId(User)
                            select i;
            foreach(var or in order)
            {
                if(or.ID==PartID)
                {
                    or.Quantity = Quantity;
                    db.Update(or);  
                }
            }
            db.SaveChanges();
        }
        //[Route("add/{coments}")]
        [HttpPost]
        public IActionResult Create_Basket(string comment)   //(List<OrderDTO> list)
        {
            AddOrder(comment);
            return Redirect("~/Home/Index");
        }

        //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        [HttpPost("search")]
        public IActionResult Search(string number)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var result = Search_num(number, userManager.GetUserId(User));
                    if (result != null)
                        return View(result);
                    else
                    { return Redirect("~/Home/Index");  }
                }
                else
                {
                    var result = Search_num(number);
                    if (result != null)
                        return View(result);
                    else
                    { return Redirect("~/Home/Index"); }
                }
                    
            }
            catch(Exception ex)
            {
                return Redirect("~/Home/Index"); 
            }
            
        }
        
        [NonAction]
        private async Task<bool> SendMessage(string id, string text, string name)
        {
            try
            {
                EmailService emailService = new EmailService();
                string email = (from i in db.Users
                                where i.Id == id
                                select i.Email).FirstOrDefault();
                await emailService.SendEmailAsync(email, text, name);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public IActionResult Contact()
        {
            return View();
        }

        //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [NonAction]
        private IEnumerable<Part> Search_num(string number)
        {
            string[] warn = { @"'","--","Select","From","Create","Update","Delete","Where","and","or","+","=","database","insert" };
            foreach (var i in warn)
            {
                var str = i.ToUpper();
                number = number.Replace(str, "").ToUpper();
            }

            var parts= repo.GetParts(number);
            if (parts != null)
            {
                double course = Convert.ToDouble(repo.GetCourseEuro());
                var disc = from i in db.DefaultDiscounts
                           select i.TheDefaultDiscount;
                foreach (var i in parts)
                {
                    if (disc.FirstOrDefault() != 0)
                        i.Price = System.Math.Round((i.Price * course * (100 - disc.FirstOrDefault()) / 100), 2);
                    else
                        i.Price = i.Price;
                }
                return parts;
            }
            else
                return null;
        }
        [NonAction]
        private IEnumerable<Part> Search_num(string number, string UserID)
        {
            string[] warn = { @"'", "--", "Select", "From", "Create", "Update", "Delete", "Where", "and", "or", "+", "=", "database", "insert" };
            foreach (var i in warn)
            {
                var str = i.ToUpper();
                number = number.Replace(str, "").ToUpper();
            }
            var parts = repo.GetParts(number);
            if (parts != null)
            {
                var user = repo.GetUser(UserID);
                double course = Convert.ToDouble(repo.GetCourseEuro());
                var disc = from i in db.DefaultDiscounts
                           select i.TheDefaultDiscount;

                foreach (var i in parts)
                {
                    if (user.Discount != 0)
                    {
                        i.Price = System.Math.Round((i.Price * course * (100 - user.Discount) / 100), 2);
                    }
                    else
                    {
                        if (disc.FirstOrDefault() != 0)
                            i.Price = System.Math.Round((i.Price * course * ((100 - disc.FirstOrDefault()) / 100)), 2);
                        else
                            i.Price = i.Price;
                    }
                }
                return parts;
            }
            else
                return null;
        }
        [NonAction]
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
                    if (disc.FirstOrDefault() != 0)
                        i.Price = System.Math.Round((i.Price * course * (100 - disc.FirstOrDefault()) / 100), 2);
                    else
                        i.Price = i.Price;
                    
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
        [NonAction]
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
                    if (user.Discount != 0)
                        i.Price = System.Math.Round((i.Price * course* (100 - user.Discount) / 100), 2);
                    else
                        i.Price = i.Price;
                    
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

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
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
        [NonAction]
        private List<_OrderDTO> AddParts(int PartID, string UserID)
        {
            
            var part = repo.GetParts(PartID);
            var user = repo.GetUser(UserID);
            double course = Convert.ToDouble(repo.GetCourseEuro());
            double price;
            if (user.Discount != 0)
            {
                price = ((part.Price) * (course) * ((100.00 - Convert.ToDouble(user.Discount)) / 100.00));
                price = System.Math.Round((price),2);
            }
            else
            { part.Price = part.Price; price = System.Math.Round((part.Price), 2); }
            int quantity=0;
            if (part.Group_Parts == "Zimmermann диски")
                quantity = 2;
            else if (part.Group_Parts == "Zimmermann колодки")
                quantity = 1;
            else
                quantity = 1;
            _OrderDTO order = new _OrderDTO() { AddressID = UserID, Analogues = part.Analogues, Avenue = user.Avenue,
                Brand = part.Brand, Comment = "", Country = user.Country, Description = part.Description,
                Foto_link = part.Foto_link, Group_Auto = part.Group_Auto, Group_Parts = part.Group_Parts,
                OrderID = HttpContext.Session.Id,IP="",Number=part.Number,PartID=part.ID,Price=price,Quantity=quantity,Sity=user.Sity};
            db.Add(order);
            db.SaveChanges();
            orders.Add(order);
            return orders;
        }
        [NonAction]
        private List<_OrderDTO> AddParts(int PartID)
        {
            
            var part = repo.GetParts(PartID);
            double course = Convert.ToDouble(repo.GetCourseEuro());
            double price;
            int discount = (from i in db.DefaultDiscounts
                           select i.TheDefaultDiscount).FirstOrDefault();
            if (discount != 0)
            {
                price = part.Price*course;
                course = 100 - discount;course = course / 100;
                part.Price = price*course;
                part.Price = System.Math.Round((part.Price), 2);
            }
            else
            {
                part.Price = System.Math.Round((part.Price), 2);
            }
            int quantity =0;
            if (part.Group_Parts == "Zimmermann диски")
                quantity = 2;
            else if (part.Group_Parts == "Zimmermann колодки")
                quantity = 1;
            else
                quantity = 1;
            
            _OrderDTO order = new _OrderDTO()
            {
                
                Analogues = part.Analogues,
                Brand = part.Brand,
                Comment = "",
                
                Description = part.Description,
                Foto_link = part.Foto_link,
                Group_Auto = part.Group_Auto,
                Group_Parts = part.Group_Parts,
                OrderID = HttpContext.Session.Id,
                IP = "",
                Number = part.Number,
                PartID = part.ID,
                Price = part.Price,
                Quantity = quantity,
            };
            db.Add(order);
            db.SaveChanges();
            orders.Add(order);
            return orders;
        }
        [NonAction]
        private IActionResult AddOrder(string coment)
        {
            _Order or = new _Order();
            var order = from i in db._OrdersDTO
                        where i.AddressID == userManager.GetUserId(User)
                        select i;
            List<string> text = new List<string>();
            foreach(var i in order)
            {
                or.UserID = repo.GetUserID(i.AddressID);
                or.AddressID = i.AddressID;
                or.Avenue = i.Avenue;
                or.Brand = i.Brand;
                or.Comment = coment;
                or.Country = i.Country;
                or.Group_Parts = i.Group_Parts;
                or.SessionID = HttpContext.Session.Id;
                or.IP = i.IP;
                or.Number = i.Number;
                or.OneCCreate = false;
                or.OrderID = i.OrderID;
                or.PartID = i.PartID;
                or.Price = i.Price;
                or.Quantity = i.Quantity;
                or.Sity = i.Sity;
                text.Add($"Ваш заказ: Номер товара: {i.Number}; Количевство {i.Quantity}; Цена {i.Price} ;");
                repo.Delete(i.OrderID);
                repo.Create(or);
                //db.Add(or);
                //db.SaveChanges();
            }
           
            string texts = null;
            foreach (string s in text)
            { texts = texts + s; }
            texts = texts + $"Коментарий : {coment};";
            SendMessage(userManager.GetUserId(User),"Заказ на сайте ttua.com.ua",texts);
            EmailService email = new EmailService();
            foreach (var i in db.Managers)
            {
                email.SendEmailAsync(i.Email,"Новый заказ на сайте ttua.com.ua",texts);
            }

            return Ok();
        }
    }
}
