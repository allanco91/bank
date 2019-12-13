using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using WebApplication3.Controllers;
using WebApplication3.Repositories;

namespace NUnitTestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {

            var mok = new Mock<ITransactionsRepository>();

            mok.Setup(a => a.BalanceAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(10.0));

            var controller = new TransactionsController(mok.Object);

            var result = controller.Debit(new WebApplication3.Repositories.Entities.TransactionEntity { Value = 50.0 });

            result.Should().BeOfType<OkResult>();

            
        }
    }
}