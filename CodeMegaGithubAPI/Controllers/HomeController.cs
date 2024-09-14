using Microsoft.AspNetCore.Mvc;

namespace CodeMegaGithubAPI.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}