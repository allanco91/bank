using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Repositories.Entities;

namespace WebApplication3.Repositories.Services
{
    public class TransactionsService
    {

        private readonly ITransactionsRepository _repo;

        public TransactionsService(ITransactionsRepository repository)
        {
            _repo = repository;
        }

        public void AddCredit(TransactionEntity obj)
        {
            _repo.Insert(obj);
        }

        public void AddDebit(TransactionEntity obj)
        {
            _repo.Insert(obj);
        }

        public double CreditBalance(int account)
        {
            var transactions = from t in _repo.List() select t;
            return transactions.Where(t => t.Account == account && !t.IsDebit).Sum(t => t.Value);
        }

        public double DebitBalance(int account)
        {
            var transactions = from t in _repo.List() select t;
            return transactions.Where(t => t.Account == account && t.IsDebit).Sum(t => t.Value);
        }

        public double Balance(int account)
        {   
            return CreditBalance(account) - DebitBalance(account);
        }

        public List<TransactionEntity> AccountExtract(int account)
        {
            var transactions = from t in _repo.List() select t;

            return transactions.Where(t => t.Account == account).OrderBy(t => t.Date).ToList();
        }

        public List<IGrouping<int, TransactionEntity>> MonthlyReport(int account, int year)
        {
            var transactions = from t in _repo.List() select t;

            transactions.Where(t => t.Account == account);
            transactions.Where(t => t.Date.Year == year);

            return transactions.OrderBy(t => t.Date).GroupBy(t => t.Date.Month).ToList();
        }
    }
}
