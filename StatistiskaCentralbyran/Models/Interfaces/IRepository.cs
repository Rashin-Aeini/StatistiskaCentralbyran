using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StatistiskaCentralbyran.Models.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Read();
        Task<T> ReadAsync(int id);
        Task<T> AddAsync(T entry);
        Task<bool> EditAsync(T entry);
        Task<bool> RemoveAsync(T entry);
        Task<bool> ExistAsync(Expression<Func<T, bool>> perdicate);
    }
}
