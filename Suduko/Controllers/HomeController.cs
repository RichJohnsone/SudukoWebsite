using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Suduko.Models;
using Suduko.Services;

namespace Suduko.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISolver _solver;
    public Board _board { get; set; } = null!;

    public HomeController(ILogger<HomeController> logger, ISolver solver)
    {
        _logger = logger;
        _solver = solver;
    }

    public IActionResult Index()
    {
        _board = new Board(9);
        _solver.SolveSudoku(_board);
        HomeViewModel view = new(_board);
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

