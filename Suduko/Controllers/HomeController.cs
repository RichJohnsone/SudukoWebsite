using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Suduko.Models;
using Suduko.Services;

namespace Suduko.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public Board _board { get; set; } = null!;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _board = new Board(9);
        HomeViewModel view = new(_board);
        view.BoardJson = JsonConvert.SerializeObject(_board);
        return View(view);
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
}

