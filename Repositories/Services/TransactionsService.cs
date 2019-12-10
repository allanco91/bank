using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Repositories;
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

        public void Credit(TransactionEntity obj)
        {
            _repo.Insert(obj);
        }

        public void Debit(TransactionEntity obj)
        {
            _repo.Insert(obj);
        }

        public List<TransactionEntity> AccountExtract(int account)
        {
            var transactions = from t in _repo.List() select t;

            return transactions.Where(t => t.Account == account).OrderBy(t => t.Id).ToList();
        }
    }
}
