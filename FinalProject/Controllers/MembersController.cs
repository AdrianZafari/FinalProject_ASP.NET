using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

public class MembersController : Controller
{
    [Route("admin/members")]
    public IActionResult Index()
    {
        var viewModel = new MembersViewModel()
        {
            Members = []
        };

        return View(viewModel);
    }
}
