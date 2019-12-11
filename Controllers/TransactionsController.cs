using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Repositories.Entities;
using WebApplication3.Repositories;
using WebApplication3.Repositories.Services;
using WebApplication3.Models;
using System.Diagnostics;

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
                return View(obj);
            }

            _transactionsService.AddCredit(obj);
            return RedirectToAction(nameof(Success), new { Message = "Successfully inserted credits", Balance = _transactionsService.Balance(obj.Account) } );
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

            _transactionsService.AddDebit(obj);
            return RedirectToAction(nameof(Success), new { Message = "Successfully debited credits", Balance = _transactionsService.Balance(obj.Account) });
        }

        public IActionResult AccountExtract()
        {
            return View();
        }

        public IActionResult MonthlyReport()
        {
            return View();
        }

        public IActionResult Success(string message, double balance)
        {
            var viewModel = new SuccessViewModel
            {
                Message = message,
                Balance = balance
            };

            return View(viewModel);
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }
    }
}