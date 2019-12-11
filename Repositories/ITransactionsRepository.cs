﻿using System.Collections.Generic;
using System.Linq;
using WebApplication3.Repositories.Entities;

namespace WebApplication3.Repositories
{
    public interface ITransactionsRepository
    {
        TransactionEntity Get(string id);
        void Insert(TransactionEntity transaction);
        void Debit(TransactionEntity transaction);
        List<TransactionEntity> List();
        double Balance(int account);
        List<TransactionEntity> Extract(int account);
        List<IGrouping<int, TransactionEntity>> MonthlyReport(int account, int year);
    }
}