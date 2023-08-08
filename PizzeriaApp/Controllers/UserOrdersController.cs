using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Models;

namespace PizzeriaApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserOrdersController : Controller
    {
        private readonly DatabaseContext _context;

        public UserOrdersController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: UserOrders
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.UserOrders.Include(u => u.OrderDetails).Include(u => u.ParentOrder).Include(u => u.Status).Include(u => u.User);
            return View(await databaseContext.ToListAsync());
        }

        // GET: UserOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserOrders == null)
            {
                return NotFound();
            }

            var userOrders = await _context.UserOrders
                .Include(u => u.OrderDetails)
                .Include(u => u.ParentOrder)
                .Include(u => u.Status)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userOrders == null)
            {
                return NotFound();
            }

            return View(userOrders);
        }

        // GET: UserOrders/Create
        public IActionResult Create()
        {
            ViewData["OrderDetailsId"] = new SelectList(_context.OrderDetails, "Id", "Id");
            ViewData["ParentOrderId"] = new SelectList(_context.UserOrders, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,OrderDetailsId,StatusId,CreatedAt,ParentOrderId")] UserOrders userOrders)
        {
            userOrders.CreatedAt = DateTime.Now.ToUniversalTime();
            if (ModelState.IsValid)
            {
                _context.Add(userOrders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderDetailsId"] = new SelectList(_context.OrderDetails, "Id", "Id", userOrders.OrderDetailsId);
            ViewData["ParentOrderId"] = new SelectList(_context.UserOrders, "Id", "Id", userOrders.ParentOrderId);
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Name", userOrders.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userOrders.UserId);
            return View(userOrders);
        }

        // GET: UserOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserOrders == null)
            {
                return NotFound();
            }

            var userOrders = await _context.UserOrders.FindAsync(id);
            if (userOrders == null)
            {
                return NotFound();
            }
            ViewData["OrderDetailsId"] = new SelectList(_context.OrderDetails, "Id", "Id", userOrders.OrderDetailsId);
            ViewData["ParentOrderId"] = new SelectList(_context.UserOrders, "Id", "Id", userOrders.ParentOrderId);
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Name", userOrders.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userOrders.UserId);
            return View(userOrders);
        }

        // POST: UserOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,OrderDetailsId,StatusId,CreatedAt,ParentOrderId")] UserOrders userOrders)
        {
            if (id != userOrders.Id)
            {
                return NotFound();
            }
            userOrders.CreatedAt = DateTime.Now.ToUniversalTime();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userOrders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserOrdersExists(userOrders.Id))
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
            ViewData["OrderDetailsId"] = new SelectList(_context.OrderDetails, "Id", "Id", userOrders.OrderDetailsId);
            ViewData["ParentOrderId"] = new SelectList(_context.UserOrders, "Id", "Id", userOrders.ParentOrderId);
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Name", userOrders.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userOrders.UserId);
            return View(userOrders);
        }
        // GET: UserOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserOrders == null)
            {
                return NotFound();
            }

            var userOrders = await _context.UserOrders
                .Include(u => u.OrderDetails)
                .Include(u => u.ParentOrder)
                .Include(u => u.Status)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userOrders == null)
            {
                return NotFound();
            }

            return View(userOrders);
        }

        // POST: UserOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserOrders == null)
            {
                return Problem("Entity set 'DatabaseContext.UserOrders'  is null.");
            }
            var userOrders = await _context.UserOrders.FindAsync(id);
            if (userOrders != null)
            {
                _context.UserOrders.Remove(userOrders);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserOrdersExists(int id)
        {
          return _context.UserOrders.Any(e => e.Id == id);
        }
    }
}
