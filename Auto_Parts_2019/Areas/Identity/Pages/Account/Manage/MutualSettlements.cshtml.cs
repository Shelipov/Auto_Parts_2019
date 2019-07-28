using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Auto_Parts_2019.Data;
using Auto_Parts_2019.Helpers;
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

        public async Task<IActionResult> OnGet(int page = 1)
        {
            //IndexViewModel returnModel = new IndexViewModel();
            int pageSize = 20;
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Невозможно загрузить пользователя с идентификатором '{_userManager.GetUserId(User)}'.");
            }
            ModelList = repo.GetMutualSettlemenList(user.Id);
            int count = 1;
            
            var counts = ModelList.Count();
            var items = ModelList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(counts, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                ModelList = items
            };
            foreach (var i in ModelList)
            {
                i.UserID = user.UserName;
                i.Count = count++;
                i.Email = user.Email;
                i.returnModel = viewModel;
            }
            return Page();
        }
    }
    public class MutualSettlementModelDTO
    {
        public int MutualSettlementID { get; set; }

        public string UserID { get; set; }
        public int Count { get; set; }

        [Display(Name = "Накладная")]
        public string InvoiceType { get; set; }

        [Display(Name = "Номер Накладнщй")]
        public string InvoceNumber { get; set; }

        [Display(Name = "euro")]
        public decimal EU { get; set; }

        [Display(Name = "грн.")]
        public decimal UA { get; set; }

        [Display(Name = "Курс")]
        public double Course { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Дата")]
        public DateTime DateCreate { get; set; }

        [Display(Name = "Задолженость")]
        public double Debit { get; set; }

        [Display(Name = "Б.Н. Задолженость")]
        public double DebitBN { get; set; }

        public bool Del { get; set; }
        public IndexViewModel returnModel { get; set; }
    }
}