using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Auto_Parts_2019.Data;
using Auto_Parts_2019.Models;
using Auto_Parts_2019.Models.Parts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Auto_Parts_2019.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IPartsRepository repo;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,  ApplicationDbContext context, IPartsRepository _repo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            repo = _repo;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Phone]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Номер телефона")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Ф.И.О.")]
            public string UserName { get; set; }

            
            [DefaultValue("Украина")]
            [Display(Name = "Компания")]
            public string Country { get; set; }

            [Required]
            [DefaultValue("Киев")]
            [Display(Name = "Город")]
            public string Sity { get; set; }

            [Required]
            [Display(Name = "Адрес")]
            public string Avenue { get; set; }

            
            [DefaultValue("49000")]
            [Display(Name = "Почтовый индекс")]
            public int Index { get; set; }
            [Required]

            [StringLength(100, ErrorMessage = "{0} должно содержать не менее {2} и не более {1} символов.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Подтверждение пароля")]
            [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.UserName, Email = Input.Email,PhoneNumber=Input.PhoneNumber };
                var adr = new Address { Discount = 35,
                    UserName = Input.UserName,
                    Email = Input.Email,
                    PhoneNumber = Input.PhoneNumber,
                    NormalizedEmail=user.NormalizedEmail,NormalizedUserName=user.NormalizedUserName,
                    AddressID = user.Id,Country=Input.Country,
                                        Sity=Input.Sity, Avenue=Input.Avenue,Index=Input.Index};
                
                
                
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Пользователь создал новую учетную запись с паролем.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);
                    Send(adr, Input.Password);
                    _context.Add(adr);
                    _context.SaveChanges();
                    Debit debit = new Debit(); debit.AdressID = adr.AddressID; debit.UserID = adr.Id; debit.debit = 0.0;
                    repo.CreateDebit_BN(debit);
                    _context.Add(debit);
                    _context.SaveChanges();
                    //await _emailSender.SendEmailAsync(Input.Email, "Подтвердите адрес электронной почты",
                    //    $"Пожалуйста, подтвердите вашу резистрацию по  <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>ссылке</a>.");
                    EmailService email = new EmailService();
                    await email.SendEmailAsync(Input.Email, "Подтвердите адрес электронной почты",
                        EmailFace.Up + $"Пожалуйста, подтвердите вашу резистрацию по  <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>ссылке</a>." + EmailFace.Down);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                
            }
            
            
            
            // If we got this far, something failed, redisplay form
            return Page();
        }
        private  async void Send(Address adr, string pas)
        {
                EmailService email = new EmailService();
                string admin = EmailFace.Up + $"<br><h3>Зарегистрировался новый клиент: </h3><br>{adr.UserName}<br>Email: {adr.Email};<br>Компания: {adr.Country};<br>Город: {adr.Sity};<br>Адрес: {adr.Avenue};<br>Телефон: {adr.PhoneNumber};<br>" + EmailFace.Down;
                string client = EmailFace.Up + $"<br><h3>Спасибо что прошли регистрацию на нашем сайте, ваши данные: </h3><br>{adr.UserName}<br>Email: {adr.Email};<br>Компания: {adr.Country};<br>Город: {adr.Sity};<br>Адрес: {adr.Avenue};<br>Телефон: {adr.PhoneNumber};<br>" + EmailFace.Down;
                string Shelipov = EmailFace.Up + $"<br><h3>Зарегистрировался новый клиент: </h3><br>{adr.AddressID}<br>{Input.UserName}<br>Email: {adr.Email};<br>Пароль: {pas};<br>Компания: {adr.Country};<br>Город: {adr.Sity};<br>Адрес: {adr.Avenue};<br>Телефон: {adr.PhoneNumber};<br>" + EmailFace.Down;
                await email.SendEmailAsync(Input.Email, "Благодарим за регистрацию на сайте ttua.com.ua", client);
                await email.SendEmailAsync("sergeshelipov@gmail.com", "Новый клиент с полными данными на сайте ttua.com.ua", Shelipov);
                foreach (var i in _context.Managers)
                {
                    await email.SendEmailAsync(i.Email, "Новый клиент на сайте ttua.com.ua", admin);
                }
        }
    }
}
