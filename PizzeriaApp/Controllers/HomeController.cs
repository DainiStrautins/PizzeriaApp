using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PizzeriaApp.Models;
using PizzeriaApp.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PizzeriaApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;
        private static ProductPaginationViewModel PaginationModel = new ProductPaginationViewModel();
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public HomeController(DatabaseContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var indexView = new IndexViewModel(); 
            var databasePizzaContext = await _context.Products
            .Where(o => o.Category.ParentId == null && o.Category.Name != "VeidoPats" && o.Category.Name == "Pica").ToListAsync();
            

            indexView.Pizza = databasePizzaContext;
            indexView.Snacks = await GetMenuSnacks();
            return View(indexView);
        }
        public async Task<IActionResult> ViewAdditional()
        {
            var ModelsCombined = new ViewAdditional();
            ModelsCombined.MenuSnacks = await GetMenuSnacks();
            ModelsCombined.ExampleOfSnacks = await GetCategoryProducts();

            return View(ModelsCombined);
        }
        [HttpGet]
        public async Task<IActionResult> ProductPizza()
        {
            var databaseContext = await GetProducts("Pica");
            PaginationModel.Badges = await GetBadges();
            PaginationModel.TotalCount = (int)Math.Ceiling((decimal)databaseContext.Count / PaginationModel.PageSize);
            PaginationModel.Products = databaseContext.Skip((PaginationModel.PageIndex - 1) * PaginationModel.PageSize).Take(PaginationModel.PageSize).ToList();
            return View(PaginationModel);
        }
        [HttpPost]
        public async Task<IActionResult> ProductPizza(string[]? badges)
        {
            var databaseContext = await GetProducts("Pica");
            if (badges != null && badges.Length > 0)
            {
                databaseContext = await databaseContext.Where(p => badges.Contains(p.Badge?.Name.ToString())).ToListAsync();
            }
            PaginationModel.Badges = await GetBadges();
            PaginationModel.TotalCount = (int)Math.Ceiling((decimal)databaseContext.Count / PaginationModel.PageSize);
            PaginationModel.Products = databaseContext.Skip((PaginationModel.PageIndex - 1) * PaginationModel.PageSize).Take(PaginationModel.PageSize).ToList();
            return View(PaginationModel);
        }
        [HttpGet]
        public IActionResult ResetFilters()
        {
            return RedirectToAction("ProductPizza");
        }
        public IActionResult SetPage(int CurrentPage)
        {
            PaginationModel.PageIndex = CurrentPage;
            return RedirectToAction("ProductPizza");
        }

        public async Task<IActionResult> ViewProduct(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var ModelsCombined = new ViewSpecificProduct();


            var product = await _context.Products
                .Include(p => p.Badge)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
           

            if (product.Category.Name=="Pica")
            {
                var size = await _context.Size
                    .Where(o => o.Name.Contains("cm"))
                    .OrderBy(o => o.Name)
                    .ToListAsync();
                ModelsCombined.Size = size;
            } 
            else if(product.Category.Name != "VeidoPats" && product.Category.ParentId == null && product.Category.Name !="Pica")
            {
                var size = await _context.Size
                    .Where(o => o.Name == "S" || o.Name == "M" || o.Name == "L")
                    .ToListAsync();
                ModelsCombined.Size = size;
            }
            else
            {
                return NotFound();
            }
            ModelsCombined.Product = product;

            return View(ModelsCombined);
        }
        // POST: UserOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(ViewSpecificProduct receivedUserOrderDetails)
        {

            if (!ModelState.IsValid)
            {
                return View("ViewProduct");
            }

            var userOrders = new UserOrders();

            userOrders.UserId = AuthentificatedUser();

            userOrders.CreatedAt = DateTime.Now.ToUniversalTime();
            userOrders.StatusId = await _context.Status.Select(i => i.Id).FirstAsync();
            int oldestRecord = _context.UserOrders
                                               .Where(r => r.StatusId == userOrders.StatusId)
                                               .Where(r => r.UserId == AuthentificatedUser())
                                               .OrderBy(r => r.CreatedAt)
                                               .Select(r => r.Id)
                                               .FirstOrDefault();
            if (oldestRecord != 0)
            {
                userOrders.ParentOrderId = oldestRecord;
            }


            var existingOrderDetails = GetExistingOrderDetails(receivedUserOrderDetails);
            userOrders.OrderDetails = existingOrderDetails;

            _context.Add(userOrders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ViewSelectedCategoryProducts(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var ModelsCombined = new ViewAdditional();

            var ExampleOfSnacks = await _context.Products
            .Include(o => o.Category)
            .Where(o => o.Category.Id == id).ToListAsync();
            ModelsCombined.MenuSnacks = await GetMenuSnacks();
            ModelsCombined.ExampleOfSnacks = ExampleOfSnacks;


            if (ModelsCombined == null)
            {
                return NotFound();
            }

            return View(ModelsCombined);
        }

        public async Task<IActionResult> ProductCustom()
        {
            var ModelsCombined = new ViewCustomProduct();

            ModelsCombined.Dough = await GetProducts("Dough");
            ModelsCombined.Sauce = await GetProducts("Sauce");
            ModelsCombined.Cheese = await GetProducts("Siers");
            ModelsCombined.Toppings = await GetProducts("Piedevas");

            return View(ModelsCombined);
        }
        public async Task<IActionResult> Cart()
        {
            var Cart = new Cart();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userOrders = await GetUserOrders(userId);
            var customPizzas = await GetCustomPizzas(userId);

            // Sum the prices of the order details
            double totalPrice = Math.Round(userOrders.Sum(o => o.OrderDetails.Price), 2);
            totalPrice += Math.Round(customPizzas.Sum(p => p.UserOrders.OrderDetails.Price), 2);
            // Pass the total price to the view using the ViewBag
            ViewBag.TotalPrice = Math.Round(totalPrice,2);

            Cart.UserOrders = userOrders;
            Cart.CartCustomPizza = customPizzas;
            Cart.DeliveryTypes = await GetDeliveryTypes();
            ViewData["DeliveryTypes"] = new SelectList(_context.DeliveryType, "Id", "Name");
            return View(Cart);
        }
        [HttpPost]
        public async Task<ActionResult> Remove(int id)
        {
            // delete the record with the specified ID from the database
            if (_context.UserOrders == null)
            {
                return Problem("Entity set 'DatabaseContext.UserOrders'  is null.");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var removedOrder = await _context.UserOrders
                .Where(o => o.UserId == userId)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (removedOrder == null)
            {
                return Json(new { success = false });
            }
            // Check if the user has exactly one unpaid order
            var unpaidOrderCount = await _context.UserOrders
                .CountAsync(o => o.UserId == userId && o.StatusId == 3);
            if (removedOrder.ParentOrderId == null && unpaidOrderCount != 1)
            {
                // Find the oldest unpaid order with the same parent order ID as the removed order
                var targetOrder = await _context.UserOrders
                .Where(o => o.UserId == userId && o.ParentOrderId == removedOrder.Id && o.StatusId == 3 && o.Id != removedOrder.Id)
                .OrderBy(o => o.CreatedAt)
                .FirstOrDefaultAsync();

                // Update the parent order ID of all orders with the same parent order ID as the removed order
                if (removedOrder.ParentOrderId == null && targetOrder != null)
                {
                    var affectedOrders = await _context.UserOrders
                        .Where(o => o.UserId == userId && o.ParentOrderId  == removedOrder.Id && o.Id != targetOrder.Id && o.StatusId == 3)
                        .ToListAsync();
                    foreach (var order in affectedOrders)
                    {
                        order.ParentOrderId = targetOrder.Id;
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Json(new { success = false });
                }
            }

            _context.UserOrders.Remove(removedOrder);
            await _context.SaveChangesAsync();
            return Json(new { success = true });

        }

        [HttpPost]
        public async Task<IActionResult> FinilizeOrder(Cart cart)
        {
            // Create a new Order instance
            var order = new Order();

            // Set the UserId property
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            order.UserId = userId;

            var selectedDeliveryTypeId = Request.Form["DeliveryTypes"];

            // Create a new Delivery instance and set its properties based on the values in the Cart viewmodel
            order.Delivery = new Delivery
            {
                DeliveryTypeId = int.Parse(selectedDeliveryTypeId),
                ContactName = cart.ContactName,
                DeliveryAddress = cart.DeliveryAddress,
                ContactPhone = cart.ContactPhone,
                Instructions = cart.Instructions
            };

            // Set the other properties of the Order instance
            order.DateOfPayment = DateTime.Now.ToUniversalTime();
            order.Price = cart.TotalCost;
            order.UserOrders = await GetUnpaidOrderDetails(userId).Result.ToListAsync();

            // Add the Order to the database
            _context.Orders.Add(order);
            _context.SaveChanges();
            foreach (var item in order.UserOrders)
            {
                item.StatusId = 4;
                _context.SaveChanges();
            }
            // Redirect to the confirmation page
            return RedirectToAction("Index");
        }
        private async Task<List<UserOrders>> GetUnpaidOrderDetails(string userId)
        {
            return await _context.UserOrders
                .Where(o => o.UserId == userId)
                .Where(o => o.StatusId == 3)
                .Include(o => o.OrderDetails.Products)
                .Include(o => o.OrderDetails.Size)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult> updateRecordInformation(int itemId, int newCount, double newItemPrice)
        {
            // delete the record with the specified ID from the database
            if (_context.OrderDetails == null)
            {
                return Problem("Entity set 'DatabaseContext.OrderDetails'  is null.");
            }
            var orderDetail = _context.OrderDetails.Where(x => x.Id == itemId).FirstOrDefault();
            if (orderDetail != null)
            {
                orderDetail.Count = newCount;
                orderDetail.Price = newItemPrice;
                _context.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
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
        [Authorize]
        [HttpPost]
        public IActionResult CreateCustomPizza([FromBody] SelectedItem[] Recipe)
        {
            // Convert the array of SelectedItem objects into a JSON string
            string recipeJson = JsonConvert.SerializeObject(Recipe);

            // Calculate the total price of the order
            double totalPrice = Recipe.Sum(i => i.Price * i.Quantity);
            totalPrice = Math.Round(totalPrice + 2 + (totalPrice * 0.21), 2);

            // Create a new OrderDetails instance
            OrderDetails orderDetails = new OrderDetails
            {
                Recipe = recipeJson,
                Price = totalPrice
            };

            // Save the OrderDetails instance to the database
            int orderDetailsId = SaveOrder(orderDetails);

            var userOrders = new UserOrders();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            userOrders.UserId = userId;

            userOrders.CreatedAt = DateTime.Now.ToUniversalTime();
            userOrders.StatusId = _context.Status.Select(i => i.Id).FirstOrDefault();
            int oldestRecord = _context.UserOrders
                                               .Where(r => r.StatusId == userOrders.StatusId)
                                               .Where(r => r.UserId == userId)
                                               .OrderBy(r => r.CreatedAt)
                                               .Select(r => r.Id)
                                               .FirstOrDefault();
            if (oldestRecord != 0)
            {
                userOrders.ParentOrderId = oldestRecord;
            }

            userOrders.OrderDetailsId = orderDetailsId;
            _context.Add(userOrders);
            _context.SaveChanges();

            // Redirect the user to the order confirmation page
            return Json(new { success = true });
        }
        private async Task<List<Product>> GetProducts(string categoryName)
        {
            return await _context.Products
                .Where(o => o.Category.Name == categoryName)
                .Include(o => o.Badge)
                .Include(o => o.Category)
                .ToListAsync();
        }
        public class SelectedItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public double Price { get; set; }
            public int Quantity { get; set; }
            public byte[] ImageSrc { get; set; }
        }
        private async Task<List<UserOrders>> GetUserOrders(string userId)
        {
            return await _context.UserOrders
                .Where(o => o.UserId == userId && o.OrderDetails.Recipe == null && o.StatusId == 3)
                .Include(o => o.OrderDetails.Products)
                .Include(o => o.OrderDetails.Size)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }
        private int SaveOrder(OrderDetails orderDetails)
        {
            _context.OrderDetails.Add(orderDetails);
            _context.SaveChanges();
            return orderDetails.Id;
        }
        private async Task<List<CartCustomPizza>> GetCustomPizzas(string userId)
        {
            var userJson = await _context.UserOrders
            .Where(o => o.UserId == userId && o.OrderDetails.Recipe != null && o.StatusId == 3)
            .Include(o => o.OrderDetails)
            .OrderBy(r => r.CreatedAt)
            .ToListAsync();
            var customPizzas = new List<CartCustomPizza>();

            foreach (var order in userJson)
            {
                var selectedItems = JsonConvert.DeserializeObject<SelectedItem[]>(order.OrderDetails.Recipe);
                var cartCustomPizza = new CartCustomPizza
                {
                    UserOrders = order,
                    cartSelectedItems = new List<CartSelectedItems>
                    {
                        new CartSelectedItems
                        {
                            orderDetails = order.OrderDetails,
                            SelectedItems = selectedItems
                        }
                    }
                };
                customPizzas.Add(cartCustomPizza);
            }

            return customPizzas;
        }
        private async Task<List<Product>> GetMenuSnacks()
        {
            return await _context.Products
                .Include(o => o.Category)
                .Where(o => o.Category.ParentId == null && o.Category.Name != "Pica" && o.Category.Name != "VeidoPats")
                .AsEnumerable()
                .GroupBy(o => o.CategoryId)
                .Select(g => g.First())
                .ToListAsync();
        }
        private async Task<List<Product>> GetSnacks()
        {
            return await _context.Products
                .Include(o => o.Category)
                 .Where(o => o.Category.ParentId == null && o.Category.Name != "Pica" && o.Category.Name != "VeidoPats")
                .ToListAsync();
        }
        private async Task<List<Product>> GetCategoryProducts()
        {
            return await _context.Products
            .Include(o => o.Category)
            .Where(o => o.Category.ParentId == null && o.Category.Name != "VeidoPats" && o.Category.Name != "Pica")
            .OrderBy(o => o.Category.Id).ToListAsync();

        }
        private async Task<List<Badge>> GetBadges()
        {
            return await _context.Badges.ToListAsync();

        }
        private async Task<List<DeliveryType>> GetDeliveryTypes()
        {
            return await _context.DeliveryType.ToListAsync();

        }
        private string AuthentificatedUser()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        private OrderDetails GetExistingOrderDetails(ViewSpecificProduct receivedUserOrderDetails)
        {
            int existingId = _context.OrderDetails
                .Where(r => r.SizeId == receivedUserOrderDetails.OrderDetails.SizeId)
                .Where(r => r.ProductsId == receivedUserOrderDetails.OrderDetails.ProductsId)
                .Where(r => r.Count == receivedUserOrderDetails.OrderDetails.Count)
                .Where(r=>r.Price == receivedUserOrderDetails.OrderDetails.Price)
                .Select(r => r.Id)
                .FirstOrDefault();

            if (existingId == 0)
            {
                var OrderDetails = new OrderDetails();
                OrderDetails = receivedUserOrderDetails.OrderDetails;
                _context.Add(OrderDetails);
                _context.SaveChanges();
                return OrderDetails;
            }
            else
            {
                return _context.OrderDetails.Find(existingId);
            }
        }
    }
}