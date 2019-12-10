using System.Collections.Generic;
using WebApplication3.Repositories.Entities;

namespace WebApplication3.Repositories
{
    public interface ITransactionsRepository
    {
        TransactionEntity Get( string id );
        void Insert( TransactionEntity transaction );
        List<TransactionEntity> List();
    }
}