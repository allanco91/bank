using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Repositories.Entities;
using WebApplication3.Repositories;
using WebApplication3.Models;
using System.Diagnostics;
using WebApplication3.Models.ViewModels;
using WebApplication3.Repositories.Exceptions;

namespace WebApplication3.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransactionsRepository _transactionsRepository;

        public TransactionsController(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
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
        public async Task<IActionResult> Credit(TransactionEntity obj)
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            await _transactionsRepository.InsertAsync(obj);
            return RedirectToAction(nameof(Success), new { Message = "Successfully inserted credits", obj.Value, Balance = await _transactionsRepository.BalanceAsync(obj.Account) });
        }

        public IActionResult Debit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Debit(TransactionEntity obj)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                if (await _transactionsRepository.FindByAccountAsync(obj.Account) == null)
                {
                    throw new NotFoundException("Account not found");
                }
                if (!obj.IsDebit)
                {
                    throw new TransactionException("Operation must be Debit");
                }
                if (await _transactionsRepository.BalanceAsync(obj.Account) < obj.Value)
                {
                    throw new TransactionException("Balance must be greater than amount to be debited");
                }

                await _transactionsRepository.InsertAsync(obj);
                return RedirectToAction(nameof(Success), new { Message = "Successfully debited credits", obj.Value, Balance = await _transactionsRepository.BalanceAsync(obj.Account) });
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { e.Message });
            }

        }

        public IActionResult IndexAccountExtract()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountExtract(int? account)
        {
            if (!account.HasValue)
                return RedirectToAction(nameof(Error), new { message = "Account not provided" });

            ViewData["account"] = account;
            var model = await _transactionsRepository.ExtractAsync(account);
            return View(model);
        }

        public IActionResult IndexMonthlyReport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MonthlyReport(int? account, int? year)
        {
            if (!account.HasValue)
                return RedirectToAction(nameof(Error), new { message = "Account not provided" });

            if (!year.HasValue)
                return RedirectToAction(nameof(Error), new { message = "Year not provided" });

            ViewData["account"] = account;
            ViewData["year"] = year;
            var model = await _transactionsRepository.MonthlyReportAsync(account, year);
            return View(model);
        }

        public IActionResult Success(string message, double value, double balance)
        {
            var viewModel = new SuccessViewModel
            {
                Message = message,
                Value = value,
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