using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PizzeriaApp.Models;
using PizzeriaApp.ViewModels;
using static PizzeriaApp.Controllers.HomeController;

namespace PizzeriaApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderDetailsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public OrderDetailsController(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index()
        {
            var orderDetails = await _context.OrderDetails.Include(o => o.Products).Include(o => o.Size).Where(o => o.Recipe==null).ToListAsync();
            var orderDetailsCustom = _context.OrderDetails.Where(o => o.Recipe != null);

            var customPizzas = new List<CustomOrder>();
            foreach (var order in orderDetailsCustom)
            {
                var selectedItems = JsonConvert.DeserializeObject<SelectedItem[]>(order.Recipe);
                var cartCustomPizza = new CustomOrder
                {
                    OrderDetails = order,
                    cartSelectedItems = new List<CartSelectedItems>
                    {
                        new CartSelectedItems
                        {
                            orderDetails = order,
                            SelectedItems = selectedItems
                        }
                    }
                };
                customPizzas.Add(cartCustomPizza);
            }
            var orderDetailsViewModel = new OrderDetailsViewModel();
            orderDetailsViewModel.orderDetails = orderDetails;
            orderDetailsViewModel.orderDetailsCustom = customPizzas;
            return View(orderDetailsViewModel);
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetails
                .Include(o => o.Products)
                .Include(o => o.Size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderDetails == null)
            {
                return NotFound();
            }

            return View(orderDetails);
        }

        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name");
            ViewData["SizeId"] = new SelectList(_context.Size, "Id", "Name");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductsId,SizeId,Price,Count,Recipe")] OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Id", orderDetails.ProductsId);
            ViewData["SizeId"] = new SelectList(_context.Size, "Id", "Id", orderDetails.SizeId);
            return View(orderDetails);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetails.FindAsync(id);
            if (orderDetails == null)
            {
                return NotFound();
            }
            ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", orderDetails.ProductsId);
            ViewData["SizeId"] = new SelectList(_context.Size, "Id", "Name", orderDetails.SizeId);
            return View(orderDetails);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductsId,SizeId,Price,Count,Recipe")] OrderDetails orderDetails)
        {
            if (id != orderDetails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailsExists(orderDetails.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Id", orderDetails.ProductsId);
            ViewData["SizeId"] = new SelectList(_context.Size, "Id", "Id", orderDetails.SizeId);
            return View(orderDetails);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetails
                .Include(o => o.Products)
                .Include(o => o.Size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderDetails == null)
            {
                return NotFound();
            }

            return View(orderDetails);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderDetails == null)
            {
                return Problem("Entity set 'DatabaseContext.OrderDetails'  is null.");
            }
            var orderDetails = await _context.OrderDetails.FindAsync(id);
            if (orderDetails != null)
            {
                _context.OrderDetails.Remove(orderDetails);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailsExists(int id)
        {
          return _context.OrderDetails.Any(e => e.Id == id);
        }
    }
}
