using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Infrastructure;
namespace SportsStore.Models {
    public class SessionCart : Cart {
        public static Cart GetCart(IServiceProvider services)
        {
            var httpContext = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext;
            if (httpContext != null)
            {
                ISession session =
                    httpContext?.Session;
                SessionCart cart = session?.GetJson<SessionCart>("Cart")
                                   ?? new SessionCart();
                cart.Session = session;
                return cart;
            }
            return null;
        }
        [JsonIgnore]
        public ISession Session { get; set; }
        public override void AddItem(Product product, int quantity) {
            base.AddItem(product, quantity);
            Session.SetJson("Cart", this);
        }
        public override void RemoveLine(Product product) {
            base.RemoveLine(product);Session.SetJson("Cart", this);
        }
        public override void Clear() {
            base.Clear();
            Session.Remove("Cart");
        }
    }
}