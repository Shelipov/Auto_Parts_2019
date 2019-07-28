using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Auto_Parts_2019.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Auto_Parts_2019.Areas.Identity.Pages.Account.Manage
{
    public class MutualSettlementModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<MutualSettlementModel> _logger;
        private readonly IPartsRepository repo;
        public MutualSettlementModel(
            UserManager<IdentityUser> userManager,
            ILogger<MutualSettlementModel> logger,
            IPartsRepository _repo)
        {
            _userManager = userManager;
            _logger = logger;
            repo = _repo;
        }

        public List<MutualSettlementModelDTO> ModelList { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Невозможно загрузить пользователя с идентификатором '{_userManager.GetUserId(User)}'.");
            }
            ModelList = repo.GetMutualSettlemenList(user.Id);
            return Page();
        }
    }
    public class MutualSettlementModelDTO
    {
        public int MutualSettlementID { get; set; }

        public string UserID { get; set; }

        [Display(Name = "Накладная")]
        public string InvoiceType { get; set; }

        [Display(Name = "Номер Накладнщй")]
        public string InvoceNumber { get; set; }

        [Display(Name = "euro")]
        public decimal EU { get; set; }

        [Display(Name = "грн.")]
        public decimal UA { get; set; }

        [Display(Name = "Курс")]
        public decimal Course { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Дата")]
        public DateTime LastСhange { get; set; }

        [Display(Name = "Задолженость")]
        public double Debit { get; set; }

        [Display(Name = "Б.Н. Задолженость")]
        public double DebitBN { get; set; }

        public bool Del { get; set; }
    }
}