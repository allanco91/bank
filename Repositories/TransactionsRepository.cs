using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models.ViewModels;
using WebApplication3.Repositories.Entities;
using WebApplication3.Repositories.Exceptions;

namespace WebApplication3.Repositories
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly IMongoDatabase mMongoDatabase;
        public TransactionsRepository(IMongoDatabase mongoDatabase)
        {
            mMongoDatabase = mongoDatabase;
        }

        public void Insert(TransactionEntity transaction)
        {
            var collection = GetCollection();
            collection.InsertOne(transaction);
        }

        public void Debit(TransactionEntity transaction)
        {
            if (FindByAccount(transaction.Account) == null)
            {
                throw new NotFoundException("Account not found");
            }
            if (!transaction.IsDebit)
            {
                throw new TransactionException("Operation must be Debit");
            }
            if (Balance(transaction.Account) < transaction.Value)
            {
                throw new TransactionException("Balance must be greater than amount to be debited");
            }

            Insert(transaction);
        }

        private IMongoCollection<TransactionEntity> GetCollection()
        {
            return mMongoDatabase.GetCollection<TransactionEntity>("Items");
        }

        public List<TransactionEntity> List()
        {
            var collection = GetCollection();
            return collection.Find(a => true).ToList();
        }

        public TransactionEntity Get(string id)
        {
            var collection = GetCollection();
            return collection.Find(a => a.Id == id).FirstOrDefault();
        }

        private List<TransactionEntity> ListWithNegativeValues()
        {
            var transactions = from t in List() select t;

            return transactions.Select(t => new TransactionEntity
            {
                Id = t.Id,
                Account = t.Account,
                Date = t.Date,
                IsDebit = t.IsDebit,
                Value = t.IsDebit ? t.Value * -1 : t.Value
            }).ToList();
        }

        private TransactionEntity FindByAccount(int account)
        {
            var collection = GetCollection();
            return collection.Find(a => a.Account == account).FirstOrDefault();
        }

        public double Balance(int account)
        {
            var transactions = from t in ListWithNegativeValues() select t;

            return transactions.Where(t => t.Account == account).Sum(t => t.Value);
        }

        public List<TransactionEntity> Extract(int account)
        {
            var transactions = from t in ListWithNegativeValues() select t;

            return transactions.Where(t => t.Account == account).ToList();
        }

        public List<MonthlyReportViewModel> MonthlyReport(int account, int year)
        {
            var transactions = from t in ListWithNegativeValues() select t;

            return transactions.Where(t => t.Account == account && t.Date.Year == year)
                .GroupBy(g => new { g.Account, Date = new DateTime(g.Date.Year, g.Date.Month, 1) })
                .OrderBy(t => t.Key.Date)
                .Select(t => new MonthlyReportViewModel
                {
                    Account = t.Key.Account,
                    Date = t.Key.Date,
                    Credit = t.Where(x => !x.IsDebit).Sum(x => x.Value),
                    Debit = t.Where(x => x.IsDebit).Sum(x => x.Value),
                    Balance = t.Sum(x => x.Value)
                }).ToList();
        }
    }
}
