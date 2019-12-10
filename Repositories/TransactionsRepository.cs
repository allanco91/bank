using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Repositories.Entities;

namespace WebApplication3.Repositories
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly IMongoDatabase mMongoDatabase;
        public TransactionsRepository( IMongoDatabase mongoDatabase )
        {
            mMongoDatabase = mongoDatabase;
        }

        public void Insert( TransactionEntity transaction )
        {
            var collection = GetCollection();
            collection.InsertOne( transaction );
        }

        private IMongoCollection<TransactionEntity> GetCollection()
        {
            return mMongoDatabase.GetCollection<TransactionEntity>( "Items" );
        }

        public List<TransactionEntity> List()
        {
            var collection = GetCollection();
            return collection.Find( a => true ).ToList();
        }

        public TransactionEntity Get( string id )
        {
            var collection = GetCollection();
            return collection.Find( a => a.Id == id ).FirstOrDefault();
        }
    }
}
