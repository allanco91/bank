using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Repositories.Entities;
using WebApplication3.Repositories;
using WebApplication3.Repositories.Services;

namespace WebApplication3.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly TransactionsService _transactionsService;

        public TransactionsController(TransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }
                
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Credit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Credit(TransactionEntity obj)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            _transactionsService.Credit(obj);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Debit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Debit(TransactionEntity obj)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            _transactionsService.Debit(obj);
            return RedirectToAction(nameof(Index));
        }
    }
}