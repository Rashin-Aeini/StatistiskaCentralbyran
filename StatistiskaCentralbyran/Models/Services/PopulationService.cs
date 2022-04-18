using Microsoft.EntityFrameworkCore;
using StatistiskaCentralbyran.Models.Domains;
using StatistiskaCentralbyran.Models.Interfaces;
using StatistiskaCentralbyran.Models.ViewModels.Population;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatistiskaCentralbyran.Models.Services
{
    public class PopulationService : IPopulationService
    {
        private IRepository<Population> PopulationRepository { get; }
        private IRepository<Year> YearRepository { get; }
        private IRepository<Region> RegionRepository { get; }

        public PopulationService(
            IRepository<Population> populationRepository, 
            IRepository<Year> yearRepository, 
            IRepository<Region> regionRepository
            )
        {
            PopulationRepository = populationRepository;
            YearRepository = yearRepository;
            RegionRepository = regionRepository;
        }

        public async Task<PopulationResponse> YearAsync(PopulationRequest entry)
        {
            if (string.IsNullOrEmpty(entry.Value))
            {
                entry.Value = await YearRepository.Read()
                    .Select(item => item.Number.ToString())
                    .FirstOrDefaultAsync();
            }

            int value = int.Parse(entry.Value);

            if(!await YearRepository.ExistAsync(item => item.Number == value))
            {
                throw new System.Data.InvalidConstraintException();
            }

            Year year = await YearRepository
                .Read()
                .Where(item => item.Number == value)
                .FirstOrDefaultAsync();

            List<Population> entries =  await PopulationRepository
                .Read()
                .Where(item => item.YearId == year.Id)
                .Include(item => item.Year)
                .Include(item => item.Region)
                .ToListAsync();

            List<PopulationViewModel> populations = entries.GroupBy(item => item.RegionId)
                .Select(group => new PopulationViewModel 
                { 
                    Region = group.First().Region.Name,
                    Year = group.First().Year.Number.ToString(),
                    Count = group.Sum(pop => pop.Count), 
                })
                .ToList();

            int offest = entry.Page == 1 ? 0 : ((entry.Page - 1) * entry.Size);

            return new PopulationResponse()
            {
                TotalRecordes = populations.Count,
                TotalPages = (int)Math.Ceiling((decimal)populations.Count / entry.Size),
                PageIndex = entry.Page,
                PageSize = populations.Take(entry.Size).Skip(offest).Count(),
                Recordes = populations.Take(entry.Size).Skip(offest).ToList(),
            };
        }

        public async Task<PopulationResponse> RegionAsync(PopulationRequest entry)
        {
            if (string.IsNullOrEmpty(entry.Value))
            {
                entry.Value = await RegionRepository.Read()
                    .Select(item => item.Name)
                    .FirstOrDefaultAsync();
            }

            if (!await RegionRepository.ExistAsync(item => item.Name == entry.Value))
            {
                throw new System.Data.InvalidConstraintException();
            }

            Region region = await RegionRepository
                .Read()
                .Where(item => item.Name == entry.Value)
                .FirstOrDefaultAsync();

            List<Population> entries = await PopulationRepository
                .Read()
                .Where(item => item.RegionId == region.Id)
                .Include(item => item.Year)
                .Include(item => item.Region)
                .ToListAsync();

            List<PopulationViewModel> populations = entries.GroupBy(item => item.YearId)
                .Select(group => new PopulationViewModel
                {
                    Region = group.First().Region.Name,
                    Year = group.First().Year.Number.ToString(),
                    Count = group.Sum(pop => pop.Count),
                })
                .ToList();

            int offest = entry.Page == 1 ? 0 : ((entry.Page - 1) * entry.Size);

            return new PopulationResponse()
            {
                TotalRecordes = populations.Count,
                TotalPages = (int)Math.Ceiling((decimal)populations.Count / entry.Size),
                PageIndex = entry.Page,
                PageSize = populations.Take(entry.Size).Skip(offest).Count(),
                Recordes = populations.Take(entry.Size).Skip(offest).ToList(),
            };
        }

        public async Task<PopulationResponse> GenderAsync(PopulationRequest entry)
        {
            if (string.IsNullOrEmpty(entry.Value))
            {
                entry.Value = Enum.GetNames(typeof(Gender)).FirstOrDefault();
            }

            if (!Enum.GetNames(typeof(Gender)).Any(item => item == entry.Value))
            {
                throw new System.Data.InvalidConstraintException();
            }

            Gender value = Enum.Parse<Gender>(entry.Value);

            List<Population> entries = await PopulationRepository
                .Read()
                .Where(item => item.Gender == value)
                .Include(item => item.Year)
                .Include(item => item.Region)
                .ToListAsync();

            List<PopulationViewModel> populations = entries
                .Select(item => new PopulationViewModel
                {
                    Region = item.Region.Name,
                    Year = item.Year.Number.ToString(),
                    Gender = value.ToString(),
                    Count = item.Count,
                })
                .ToList();

            int offest = entry.Page == 1 ? 0 : ((entry.Page - 1) * entry.Size);

            return new PopulationResponse()
            {
                TotalRecordes = populations.Count,
                TotalPages = (int)Math.Ceiling((decimal)populations.Count / entry.Size),
                PageIndex = entry.Page,
                PageSize = populations.Take(entry.Size).Skip(offest).Count(),
                Recordes = populations.Take(entry.Size).Skip(offest).ToList(),
            };
        }
    }
}
