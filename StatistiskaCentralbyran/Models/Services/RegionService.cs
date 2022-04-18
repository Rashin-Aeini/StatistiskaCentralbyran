using Microsoft.EntityFrameworkCore;
using StatistiskaCentralbyran.Models.Domains;
using StatistiskaCentralbyran.Models.Interfaces;
using StatistiskaCentralbyran.Models.ViewModels.Pagination;
using StatistiskaCentralbyran.Models.ViewModels.Region;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatistiskaCentralbyran.Models.Services
{
    public class RegionService : IRegionService
    {
        private IRepository<Region> RegionRepository { get; }

        public RegionService(IRepository<Region> regionRepository)
        {
            RegionRepository = regionRepository;
        }

        public async Task<RegionResponse> ListAsync(PaginateRequest entry)
        {
            int offest = entry.Page == 1 ? 0 : ((entry.Page - 1) * entry.Size);

            List<RegionViewModel> entries = await RegionRepository
                .Read()
                .Select(item => new RegionViewModel { Name = item.Name })
                .ToListAsync();

            return new RegionResponse
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
