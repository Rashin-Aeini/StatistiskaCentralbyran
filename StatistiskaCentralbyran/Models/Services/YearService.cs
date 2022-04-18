using Microsoft.EntityFrameworkCore;
using StatistiskaCentralbyran.Models.Domains;
using StatistiskaCentralbyran.Models.Interfaces;
using StatistiskaCentralbyran.Models.ViewModels.Pagination;
using StatistiskaCentralbyran.Models.ViewModels.Year;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatistiskaCentralbyran.Models.Services
{
    public class YearService : IYearService
    {
        private  IRepository<Year> YearRepository { get; }

        public YearService(IRepository<Year> yearRepository)
        {
            YearRepository = yearRepository;
        }

        public async Task<YearResponse> ListAsync(PaginateRequest entry)
        {
            int offest = entry.Page == 1 ? 0 : ((entry.Page - 1) * entry.Size);

            List<YearViewModel> entries = await YearRepository
                .Read()
                .Select(item => new YearViewModel { Number = item.Number })
                .ToListAsync();

            return new YearResponse
            {
                TotalRecordes = entries.Count,
                TotalPages = (int)Math.Ceiling((decimal)entries.Count / entry.Size),
                PageIndex = entry.Page,
                PageSize = entries.Take(entry.Size).Skip(offest).Count(),
                Recordes = entries.Take(entry.Size).Skip(offest).ToList(),
            };
        }
    }
}
