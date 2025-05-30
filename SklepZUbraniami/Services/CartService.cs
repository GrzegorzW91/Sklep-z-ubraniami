using Microsoft.AspNetCore.Http;
using SklepZUbraniami.Models;
using System.Text.Json;

namespace SklepZUbraniami.Services
{
    public class CartService
    {
        private const string SessionKey = "Cart";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetCartQuantity()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var json = session?.GetString(SessionKey);

            if (string.IsNullOrEmpty(json))
                return 0;

            var items = JsonSerializer.Deserialize<List<CartItem>>(json);
            return items?.Sum(i => i.Quantity) ?? 0;
        }
    }
}