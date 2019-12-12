using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication3.Models.ViewModels;
using WebApplication3.Repositories.Entities;

namespace WebApplication3.Repositories
{
    public interface ITransactionsRepository
    {
        Task<TransactionEntity> GetAsync(string id);
        Task InsertAsync(TransactionEntity transaction);
        Task DebitAsync(TransactionEntity transaction);
        Task<List<TransactionEntity>> ListAsync();
        Task<double> BalanceAsync(int account);
        Task<List<TransactionEntity>> ExtractAsync(int? account);
        Task<List<MonthlyReportViewModel>> MonthlyReportAsync(int? account, int? year);
    }
}