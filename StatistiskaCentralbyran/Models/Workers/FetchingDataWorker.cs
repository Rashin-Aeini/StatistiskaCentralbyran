using Newtonsoft.Json;
using StatistiskaCentralbyran.Models.Domains;
using StatistiskaCentralbyran.Models.Interfaces;
using StatistiskaCentralbyran.Models.Settings;
using StatistiskaCentralbyran.Models.ViewModels.Centralbyran;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StatistiskaCentralbyran.Models.Workers
{
    public class FetchingDataWorker
    {
        private Centralbyran Centralbyran { get; }

        private Year[] Years { get; }
        private IDictionary<string, Region> Regions { get; }

        #region DefinitionRepositories
        private IRepository<Year> YearRepository { get; }
        private IRepository<Region> RegionRepository { get; }
        private IRepository<Population> PopulationRepository { get; }
        #endregion

        public FetchingDataWorker(
            Centralbyran meta,
            IRepository<Year> yearRepository,
            IRepository<Region> regionRepository,
            IRepository<Population> populationRepository
            )
        {
            Centralbyran = meta;

            Years = new Year[5] {
                new Year { Number = 2016 },
                new Year { Number = 2017 },
                new Year { Number = 2018 },
                new Year { Number = 2019 },
                new Year { Number = 2020 }
            };
            Regions = new Dictionary<string, Region>();

            YearRepository = yearRepository;
            RegionRepository = regionRepository;
            PopulationRepository = populationRepository;
        }

        public async Task ExecuteAsync()
        {
            #region FetchingRegions
            try
            {
                using (HttpClient client = new HttpClient()
                {
                    BaseAddress = new Uri(Centralbyran.Address)
                })
                {
                    using (HttpResponseMessage response = await client.GetAsync(string.Empty))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            CentralbyranResult result = JsonConvert
                                .DeserializeObject<CentralbyranResult>(
                                await response.Content.ReadAsStringAsync()
                                );

                            foreach (CentralbyranVariable variable in result.Variables)
                            {
                                if (string.Compare(variable.Text.ToLower(), nameof(Region).ToLower(), StringComparison.CurrentCultureIgnoreCase) == 0)
                                {
                                    for (int index = 0; index < variable.Values.Length; index++)
                                    {
                                        Regions.Add(
                                            variable.Values[index],
                                            new Region() { Name = variable.ValueTexts[index] }
                                            );
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
            #endregion

            #region SaveDataInDatabase

            for (int index = 0; index < Years.Length; index++)
            {
                Years[index] = await YearRepository.AddAsync(Years[index]);
            }

            foreach (string key in Regions.Keys)
            {
                Regions[key] = await RegionRepository.AddAsync(Regions[key]);
            }

            #endregion

            #region FetchingPopulation
            int counter = 0;

            foreach (Year year in Years)
            {
                foreach (KeyValuePair<string, Region> region in Regions)
                {
                    foreach (string gender in Enum.GetValues(typeof(Gender))
                        .Cast<Gender>()
                        .Select(item => ((int)item).ToString()))
                    {
                        try
                        {
                            using (HttpClient client = new HttpClient()
                            {
                                BaseAddress = new Uri(Centralbyran.Address)
                            })
                            {
                                using (HttpResponseMessage response = await client.PostAsync(
                                    string.Empty,
                                    new StringContent(
                                        JsonConvert.SerializeObject(
                                            new CentralbyranRequest()
                                            {
                                                Query = new CentralbyranQuery[3]
                                                {
                                                new CentralbyranQuery()
                                                {
                                                    Code = "Tid",
                                                    Selection = new CentralbyranSelection()
                                                    {
                                                        Filter = "item",
                                                        Values = new string[1] {year.Number.ToString()}
                                                    }
                                                },
                                                new CentralbyranQuery()
                                                {
                                                    Code = "Region",
                                                    Selection = new CentralbyranSelection()
                                                    {
                                                        Filter = "item",
                                                        Values = new string[1] {region.Key}
                                                    }
                                                },
                                                new CentralbyranQuery()
                                                {
                                                    Code = "Kon",
                                                    Selection = new CentralbyranSelection()
                                                    {
                                                        Filter = "item",
                                                        Values = new string[1] {gender}
                                                    }
                                                }
                                                },
                                                Response = new CentralbyranResponse() { Format = "json" }
                                            }
                                            ),
                                        Encoding.UTF8,
                                        "application/json"
                                        )
                                    ))
                                {
                                    counter++;
                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        Debug.WriteLine(await response.Content.ReadAsStringAsync());

                                        CentralbyranReceive receive = JsonConvert
                                            .DeserializeObject<CentralbyranReceive>(
                                            await response.Content.ReadAsStringAsync()
                                            );

                                        Population entry = new Population()
                                        {
                                            YearId = year.Id,
                                            RegionId = region.Value.Id,
                                            Gender = (Gender)int.Parse(gender),
                                            Count = int.Parse(receive.Data[0].Values[0])
                                        };

                                        await PopulationRepository.AddAsync(entry);

                                    }
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            Debug.WriteLine(exception.Message);
                        }
                    }
                }
            }

            Debug.WriteLine("====================================================");
            Debug.WriteLine(counter);
            Debug.WriteLine("====================================================");
            #endregion
        }
    }
}
