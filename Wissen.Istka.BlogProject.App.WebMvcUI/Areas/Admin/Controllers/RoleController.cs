using Microsoft.AspNetCore.Mvc;
using Wissen.Istka.BlogProject.App.Entity.Services;

namespace Wissen.Istka.BlogProject.App.WebMvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly IAccountService _accountService;

        public RoleController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _accountService.GetAllRoles();
            return View(roles);
        }
    }
}
