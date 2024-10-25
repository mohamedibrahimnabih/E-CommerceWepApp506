using E_Commerce.Models;
using E_Commerce.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace E_Commerce.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository cartRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController( ICartRepository cartRepository, UserManager<ApplicationUser> userManager )
        {
            this.cartRepository = cartRepository;
            this.userManager = userManager;
        }

        public IActionResult AddToCart( int productId, int count)
        {
            var appUser = userManager.GetUserId(User);

            if( appUser == null )
            {
                return RedirectToAction("Login", "Account");
            }
            Cart cart = new Cart()
            {
                Count = count,
                ProductId = productId,
                ApplicationUserId = appUser
            };

            var cartDB = cartRepository.GetOne( expression: e => e.ProductId == productId && e.ApplicationUserId == appUser);

            if (cartDB == null)
                cartRepository.Create(cart);
            else
                cartDB.Count += count;
            
            cartRepository.Commit();

            TempData["success"] = "Add product to cart successfully";

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            var appUser = userManager.GetUserId(User);

            var shoppingCarts = cartRepository.Get([e => e.Product, e=>e.ApplicationUser], e=>e.ApplicationUserId == appUser);

            ViewBag.TotalPrice = shoppingCarts.Sum(e=>e.Product.Price * e.Count);

            return View(shoppingCarts);
        }

        public IActionResult Increment(int productId)
        {
            var appUser = userManager.GetUserId(User);
            var cartDB = cartRepository.GetOne(expression: e => e.ProductId == productId && e.ApplicationUserId == appUser);

            if(cartDB != null)
            {
                cartDB.Count++;
            }
            cartRepository.Commit();

            return RedirectToAction("Index");
        }

        public IActionResult Decremnt(int productId)
        {
            var appUser = userManager.GetUserId(User);
            var cartDB = cartRepository.GetOne(expression: e => e.ProductId == productId && e.ApplicationUserId == appUser);

            if (cartDB != null)
            {
                cartDB.Count--;
                if(cartDB.Count == 0)
                {
                    cartRepository.Delete(cartDB);

                }
            }
            cartRepository.Commit();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int productId)
        {
            var appUser = userManager.GetUserId(User);
            var cartDB = cartRepository.GetOne(expression: e => e.ProductId == productId && e.ApplicationUserId == appUser);

            if (cartDB != null)
            {
                cartRepository.Delete(cartDB);
            }
            cartRepository.Commit();

            return RedirectToAction("Index");
        }

        public IActionResult Pay()
        {
            var appUser = userManager.GetUserId(User);
            var cartDBs = cartRepository.Get(includeProps: [e=>e.Product], expression: e => e.ApplicationUserId == appUser).ToList();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };

            foreach (var item in cartDBs)
            {
                var result = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                        UnitAmount = (long)item.Product.Price * 100,
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(result);
            }

            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }
    }
}
