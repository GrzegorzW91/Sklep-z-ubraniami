using Microsoft.AspNetCore.Mvc;
using SklepZUbraniami.Data;

namespace SklepZUbraniami.Controllers;

public class ProductController : Controller
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var produkty = _context.Products.ToList();
        return View(produkty);
    }
}