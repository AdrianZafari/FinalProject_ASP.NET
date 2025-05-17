using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

public class ExternalAuthController : Controller
{
    [HttpPost]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ExternalCallback()
    {
        return View();
    }
}
