using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Arda.Intelligence.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Arda.Intelligence.Controllers
{
    [Route("api/[controller]")]
    public class RecommenderController : Controller
    {

        const string connectionStr = "CONNECTION STRING";
        const string apiKey = "ML API KEY";
        const string apiUrl = "ML API URL";


        // POST api/values
        [HttpPost]
        public async Task<IEnumerable<ProfessionalRecommendation>> PostAsync([FromBody]ProfessionalRecommendationArgs args, [FromQuery] int quantity)
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new MLScoreRequest()
                {
                    Inputs = new MLInputs()
                    {
                        mlIPRInput = new MLIPRInput()
                        {
                            ColumnNames = new List<string>() { "activity", "technology" },
                            Values = new List<List<string>>()
                            {
                                new List<string>()
                                {
                                    args.Activity, args.Technology
                                }
                            }
                        }
                    }
                };

                var req = JsonConvert.SerializeObject(scoreRequest);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri(apiUrl);

                var stringContent = new StringContent(req, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    string resultStr = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<MLScoreResult>(resultStr);
                    var professionals = result.Results.mlIPROutput.value.Values[0][0];
                    var intelligence = GetIntelligence(professionals).OrderByDescending(i => i.Afinity).ThenBy(i => i.Perc_Allocation).Take(quantity);
                    return intelligence;
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return null;
                }
            }
        }

        private IEnumerable<ProfessionalRecommendation> GetIntelligence(string professionals)
        {
            var listIntelligence = GetIntelligenceResult(professionals);
            var listAllocation = GetAllocation();
            var listRecommendation = MergeIntelligenceAndAllocation(listIntelligence, listAllocation);

            return listRecommendation;
        }

        private List<IntelligenceResult> GetIntelligenceResult(string professionals)
        {
            var listIntelligence = new List<IntelligenceResult>();

            var professionalAfinity = professionals.Split(' ');

            foreach (var prof in professionalAfinity)
            {
                var temp = prof.Split(':');
                var intelligence = new IntelligenceResult()
                {
                    Professional = temp[0],
                    Afinity = Math.Round(decimal.Parse(temp[1]) * 100, 2)
                };
                listIntelligence.Add(intelligence);
            }

            return listIntelligence;
        }

        private List<AllocationResult> GetAllocation()
        {
            var allocationList = new List<AllocationResult>();
            var command = "SELECT [User],[Hours_Last_7_Days],[Perc_Allocation] FROM AllocationTE;";

            using (var sqlConnection = new SqlConnection(connectionStr))
            {
                var sqlCommand = new SqlCommand(command, sqlConnection);

                sqlConnection.Open();
                using (var sqlReader = sqlCommand.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        var allocation = new AllocationResult()
                        {
                            Professional = sqlReader["User"].ToString(),
                            Hours_Last_7_Days = int.Parse(sqlReader["Hours_Last_7_Days"].ToString()),
                            Perc_Allocation = Decimal.Parse(sqlReader["Perc_Allocation"].ToString())
                        };
                        allocationList.Add(allocation);
                    }
                }
                sqlConnection.Close();
            }

            return allocationList;
        }

        private List<ProfessionalRecommendation> MergeIntelligenceAndAllocation(List<IntelligenceResult> intelligence, List<AllocationResult> allocation)
        {
            var recommendation = (from I in intelligence
                                  join A in allocation on I.Professional equals A.Professional
                                  select new ProfessionalRecommendation
                                  {
                                      Professional = I.Professional,
                                      Afinity = I.Afinity,
                                      Perc_Allocation = A.Perc_Allocation
                                  }).ToList();

            foreach (var result in intelligence)
            {
                if (recommendation.Where(i => i.Professional == result.Professional).Count() == 0)
                {
                    recommendation.Add(new ProfessionalRecommendation()
                    {
                        Professional = result.Professional,
                        Afinity = result.Afinity,
                        Perc_Allocation = decimal.Zero
                    });
                }
            }

            return recommendation;
        }
    }
}