﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoneyTracker1.Models;
using MoneyTracker1.Data;
using System.Security.Claims;
using MoneyTracker1.Models.Enums;

namespace MoneyTracker1.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
       [Authorize]
        public async Task<IActionResult> Index(int? categoryId)
        {
            // Получаем список всех категорий для фильтра
            ViewBag.Categories = await _context.Categories.ToListAsync();                      

            // Основной запрос с фильтрацией
            IQueryable<Transaction> query = _context.Transactions.Include(t => t.Category);                

            // фильтр по категории если он задан
            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(t => t.CategoryId == categoryId.Value);
                ViewBag.SelectedCategoryId = categoryId.Value;
            }

            return View(await query.OrderByDescending(t => t.Date).ToListAsync());
        }     

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,Date,Description,CategoryId,TransactionType")] TransactionCreateViewModel transaction)
        {
            if (ModelState.IsValid)
            {
                var newTransaction = new Transaction
                {
                    Amount = transaction.Amount,
                    Date = DateTime.SpecifyKind(transaction.Date, DateTimeKind.Utc),                    
                    Description = transaction.Description,
                    CategoryId = transaction.CategoryId,
                    TransactionType = transaction.TransactionType,
                };

                _context.Add(newTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Console.WriteLine("Ошибка валадиции: ");
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        Console.WriteLine($"{state.Key}: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
                ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", transaction.CategoryId);
                return View(transaction);
            }
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,Date,Description,CategoryId,TransactionType")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }



        //Анализ расходов диаграмма
        [HttpGet]
        [Authorize]
        public IActionResult ExpensesChart()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetExpensesChartData()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var data = await _context.Transactions
                .Where(t => t.TransactionType == TransactionType.Expense)
                .GroupBy(t => t.Category.Name)
                .Select(g => new
                {
                    Category = g.Key,
                    Amount = g.Sum(t => t.Amount)
                })
                .ToListAsync();

            return Json(data);
        }
    }
}
