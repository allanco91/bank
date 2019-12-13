using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using WebApplication3.Controllers;
using WebApplication3.Repositories;
using WebApplication3.Repositories.Entities;

namespace NUnitTestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestCredit()
        {
            var mok = new Mock<ITransactionsRepository>();

            var controller = new TransactionsController(mok.Object);

            var result = await controller.Credit(new TransactionEntity
            {
                Account = 1,
                Value = 50.0,
                IsDebit = false,
                Date = DateTime.Now
            });

            result.Should().BeRedirectAction(nameof(controller.Success));
        }

        [Test]
        public async Task TestCreditIsCredit()
        {
            var mok = new Mock<ITransactionsRepository>();

            var controller = new TransactionsController(mok.Object);


            var result = await controller.Credit(new TransactionEntity
            {
                Account = 1,
                Value = 50.0,
                IsDebit = true,
                Date = DateTime.Now
            });

            result.Should().BeRedirectAction(nameof(controller.Error));
        }

        [Test]
        public async Task TestDebit()
        {
            var mok = new Mock<ITransactionsRepository>();

            mok.Setup(a => a.FindByAccountAsync(It.IsAny<int>())).Returns(Task.FromResult(
                new TransactionEntity
                {
                    Id = "test",
                    Account = 1,
                    Date = DateTime.Now,
                    IsDebit = true,
                    Value = 0.0
                }));

            mok.Setup(a => a.BalanceAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(60.0));

            var controller = new TransactionsController(mok.Object);

            var result = await controller.Debit(new TransactionEntity
            {
                Account = 1,
                Value = 50.0,
                IsDebit = true,
                Date = DateTime.Now
            });

            result.Should().BeRedirectAction(nameof(controller.Success));
        }

        [Test]
        public async Task TestDebitNoAccount()
        {
            var mok = new Mock<ITransactionsRepository>();

            var controller = new TransactionsController(mok.Object);

            var result = await controller.Debit(new TransactionEntity
            {
                Account = 1,
                Value = 50.0,
                IsDebit = true,
                Date = DateTime.Now
            });

            result.Should().BeRedirectAction(nameof(controller.Error));
        }

        [Test]
        public async Task TestDebitSmallerBalance()
        {
            var mok = new Mock<ITransactionsRepository>();

            mok.Setup(a => a.FindByAccountAsync(It.IsAny<int>())).Returns(Task.FromResult(
                new TransactionEntity
                {
                    Id = "test",
                    Account = 1,
                    Date = DateTime.Now,
                    IsDebit = true,
                    Value = 0.0
                }));

            mok.Setup(a => a.BalanceAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(10.0));

            var controller = new TransactionsController(mok.Object);

            var result = await controller.Debit(new TransactionEntity
            {
                Account = 1,
                Value = 50.0,
                IsDebit = true,
                Date = DateTime.Now
            });

            result.Should().BeRedirectAction(nameof(controller.Error));
        }

        [Test]
        public async Task TestDebitIsNotDebit()
        {
            var mok = new Mock<ITransactionsRepository>();

            mok.Setup(a => a.FindByAccountAsync(It.IsAny<int>())).Returns(Task.FromResult(
                new TransactionEntity
                {
                    Id = "test",
                    Account = 1,
                    Date = DateTime.Now,
                    IsDebit = true,
                    Value = 0.0
                }));

            mok.Setup(a => a.BalanceAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(60.0));

            var controller = new TransactionsController(mok.Object);

            var result = await controller.Debit(new TransactionEntity
            {
                Account = 1,
                Value = 50.0,
                IsDebit = false,
                Date = DateTime.Now
            });

            result.Should().BeRedirectAction(nameof(controller.Error));
        }

        [Test]
        public async Task TestAccountExtract()
        {
            var mok = new Mock<ITransactionsRepository>();

            var controller = new TransactionsController(mok.Object);

            mok.Setup(a => a.FindByAccountAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(
                        new TransactionEntity
                        {
                            Id = "test",
                            Account = 1,
                            Date = DateTime.Now,
                            IsDebit = true,
                            Value = 0.0
                        }));

            var result = await controller.AccountExtract(1);

            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task TestAccountExtractNoAccount()
        {
            var mok = new Mock<ITransactionsRepository>();

            var controller = new TransactionsController(mok.Object);

            var result = await controller.AccountExtract(1);

            result.Should().BeRedirectAction(nameof(controller.Error));
        }

        [Test]
        public async Task TestAccountExtractNullAccount()
        {
            var mok = new Mock<ITransactionsRepository>();

            var controller = new TransactionsController(mok.Object);

            var result = await controller.AccountExtract(null);

            result.Should().BeRedirectAction(nameof(controller.Error));
        }

        [Test]
        public async Task TestMonthlyReport()
        {
            var mok = new Mock<ITransactionsRepository>();

            var controller = new TransactionsController(mok.Object);

            mok.Setup(a => a.FindByAccountAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(
                        new TransactionEntity
                        {
                            Id = "test",
                            Account = 1,
                            Date = DateTime.Now,
                            IsDebit = true,
                            Value = 0.0
                        }));

            var result = await controller.MonthlyReport(1, 1);

            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task TestMonthlyReportNoAccount()
        {
            var mok = new Mock<ITransactionsRepository>();

            var controller = new TransactionsController(mok.Object);

            var result = await controller.MonthlyReport(1, 1);

            result.Should().BeRedirectAction(nameof(controller.Error));
        }

        [Test]
        public async Task TestMonthlyReportNullAccount()
        {
            var mok = new Mock<ITransactionsRepository>();

            var controller = new TransactionsController(mok.Object);

            var result = await controller.MonthlyReport(null, 1);

            result.Should().BeRedirectAction(nameof(controller.Error));
        }

        [Test]
        public async Task TestMonthlyReportNullYear()
        {
            var mok = new Mock<ITransactionsRepository>();

            mok.Setup(a => a.FindByAccountAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(
                    new TransactionEntity
                    {
                        Id = "test",
                        Account = 1,
                        Date = DateTime.Now,
                        IsDebit = true,
                        Value = 0.0
                    }));

            var controller = new TransactionsController(mok.Object);

            var result = await controller.MonthlyReport(1, null);

            result.Should().BeRedirectAction(nameof(controller.Error));
        }

        [Test]
        public async Task TestMonthlyReportNullAccountAndYear()
        {
            var mok = new Mock<ITransactionsRepository>();

            var controller = new TransactionsController(mok.Object);

            var result = await controller.MonthlyReport(null, null);

            result.Should().BeRedirectAction(nameof(controller.Error));
        }
    }
}