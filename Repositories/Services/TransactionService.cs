using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Repositories;
using WebApplication3.Repositories.Entities;

namespace WebApplication3.Repositories.Services
{
    public class TransactionService
    {

        private readonly ITransactionsRepository _context;

        public TransactionService(ITransactionsRepository context)
        {
            _context = context;
        }

        public void Credito(TransactionEntity obj)
        {
            
            _context.Insert()
        }
    }
}
