using System.Threading.Tasks;
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

        public MutualSettlementModel(
            UserManager<IdentityUser> userManager,
            ILogger<MutualSettlementModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Невозможно загрузить пользователя с идентификатором '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }
    }
}