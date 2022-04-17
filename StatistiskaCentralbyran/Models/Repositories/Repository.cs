using Microsoft.EntityFrameworkCore;
using StatistiskaCentralbyran.Models.Data;
using StatistiskaCentralbyran.Models.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StatistiskaCentralbyran.Models.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected ApplicationDatabase Database { get; }
        protected DbSet<T> Table { get; }

        public Repository(ApplicationDatabase database)
        {
            Database = database;

            Table = Database.Set<T>();

            if(Table == null)
            {
                throw new Exception();
            }
        }

        public async Task<T> AddAsync(T entry)
        {
            Table.Add(entry);

            await Database.SaveChangesAsync();

            return entry;
        }

        public async Task<bool> EditAsync(T entry)
        {
            Table.Update(entry);

            return await Database.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>> perdicate)
        {
            return await Read().AnyAsync(perdicate);
        }

        public IQueryable<T> Read()
        {
            return Table;
        }

        public async Task<T> ReadAsync(int id)
        {
            return await Table.FindAsync(id);
        }

        public async Task<bool> RemoveAsync(T entry)
        {
            Table.Remove(entry);

            return await Database.SaveChangesAsync() > 0;
        }
    }
}
