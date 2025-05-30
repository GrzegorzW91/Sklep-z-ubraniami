using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SklepZUbraniami.Data;
using SklepZUbraniami.Models;
using System.Text.Json;

namespace SklepZUbraniami.Controllers;

public class CartController : Controller
{
    private readonly AppDbContext _context;
    private const string SessionKey = "Cart";

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    private List<CartItem> GetCart()
    {
        var json = HttpContext.Session.GetString(SessionKey);
        return string.IsNullOrEmpty(json)
            ? new List<CartItem>()
            : JsonSerializer.Deserialize<List<CartItem>>(json)!;
    }

    private void SaveCart(List<CartItem> cart)
    {
        var json = JsonSerializer.Serialize(cart);
        HttpContext.Session.SetString(SessionKey, json);
    }

    public IActionResult DodajDoKoszyka(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null || product.Quantity == 0)
            return NotFound();

        var cart = GetCart();
        var item = cart.FirstOrDefault(i => i.ProductId == id);

        if (item != null)
        {
            if (item.Quantity < product.Quantity)
                item.Quantity++;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = 1
            });
        }

        SaveCart(cart);
        return RedirectToAction("Index", "Product");
    }

    public IActionResult UsunZKoszyka(int id)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(i => i.ProductId == id);
        if (item != null)
            cart.Remove(item);

        SaveCart(cart);
        return RedirectToAction("Koszyk");
    }

    public IActionResult Koszyk()
    {
        var cart = GetCart();
        ViewBag.Suma = cart.Sum(i => i.Price * i.Quantity);
        return View(cart);
    }
    [Authorize]
    public IActionResult Finalizuj()
    {
        var cart = GetCart();
        foreach (var item in cart)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product != null && product.Quantity >= item.Quantity)
            {
                product.Quantity -= item.Quantity;
            }
        }

        _context.SaveChanges();
        HttpContext.Session.Remove(SessionKey);
        return View(cart);
    }

   
}