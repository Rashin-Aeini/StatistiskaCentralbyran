using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using StatistiskaCentralbyran.Models.Domains;
using StatistiskaCentralbyran.Models.Interfaces;
using StatistiskaCentralbyran.Models.Settings;
using StatistiskaCentralbyran.Models.ViewModels.Centralbyran;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        private IWebHostEnvironment Environment { get; }

        #region DefinitionRepositories
        private IRepository<Year> YearRepository { get; }
        private IRepository<Region> RegionRepository { get; }
        private IRepository<Population> PopulationRepository { get; }
        #endregion

        public FetchingDataWorker(
            Centralbyran meta,
            IRepository<Year> yearRepository,
            IRepository<Region> regionRepository,
            IRepository<Population> populationRepository,
            IWebHostEnvironment environment
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

            Environment = environment;
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
                                                        Values = Years.Select(item => item.Number.ToString()).ToArray()
                                                    }
                                                },
                                                new CentralbyranQuery()
                                                {
                                                    Code = "Region",
                                                    Selection = new CentralbyranSelection()
                                                    {
                                                        Filter = "item",
                                                        Values = Regions.Keys.ToArray()
                                                    }
                                                },
                                                new CentralbyranQuery()
                                                {
                                                    Code = "Kon",
                                                    Selection = new CentralbyranSelection()
                                                    {
                                                        Filter = "item",
                                                        Values = Enum.GetValues(typeof(Gender)).Cast<Gender>().Select(item => ((int)item).ToString()).ToArray()
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


                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Debug.WriteLine(await response.Content.ReadAsStringAsync());

                            CentralbyranReceive receive = JsonConvert
                                .DeserializeObject<CentralbyranReceive>(
                                await response.Content.ReadAsStringAsync()
                                );

                            List<string> sequence = new List<string>();

                            foreach (CentralbyranColumn column in receive.Columns)
                            {
                                switch (column.Code)
                                {
                                    case "Region":
                                        sequence.Add(nameof(Region));
                                        break;
                                    case "Tid":
                                        sequence.Add(nameof(Year));
                                        break;
                                    case "Kon":
                                        sequence.Add(nameof(Gender));
                                        break;
                                    default:
                                        break;
                                }
                            }

                            foreach (CentralbyranData data in receive.Data)
                            {
                                (Year year, Region region, Gender gender) = (null, null, 0);
                                int index = 0;
                                foreach (string item in sequence)
                                {
                                    switch (item)
                                    {
                                        case nameof(Region):
                                            region = Regions[data.Key[index]];
                                            break;
                                        case nameof(Year):
                                            year = Years.FirstOrDefault(y => y.Number == int.Parse(data.Key[index]));
                                            break;
                                        case nameof(Gender):
                                            gender = (Gender)int.Parse(data.Key[index]);
                                            break;
                                        default:
                                            break;
                                    }
                                    index++;
                                }

                                Population entry = new Population()
                                {
                                    YearId = year.Id,
                                    RegionId = region.Id,
                                    Gender = gender,
                                    Count = int.Parse(data.Values[0])
                                };

                                await PopulationRepository.AddAsync(entry);
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
        }
    }
}
