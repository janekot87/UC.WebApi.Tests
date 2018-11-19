using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UC.WebApi.Tests.API.DataStore;
using UC.WebApi.Tests.API.Logic;
using UC.WebApi.Tests.API.Models;
using Xunit;
using static UC.WebApi.Tests.API.Logic.GlobalLogic;

namespace UC.WebApi.Tests.API.Tests
{
    public class Cities
    {
        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth)]
        public void CitiesTest(string digest, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/cities?digest={digest}", Method.GET);

            request
                .AddUrlSegment("digest", digest)
                .AddHeader("Authorization", auth);

            var response = client.Execute<CertifiersModel.RootObject>(request);

            EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<CertifiersModel.RootObject> citiesMainResults;
            var isCitiesDataValid = GlobalLogic.IsModelValid(response.Data, out citiesMainResults);

            IList<ValidationResultModel<CertifiersModel.Item>> CitiesItemResults;
            var areCitiesDataItemsValid = GlobalLogic.IsModelArrayValid(response.Data.Items, out CitiesItemResults);

            if (!isCitiesDataValid)
            {
                var message = $"Cities with success: {citiesMainResults.Model.Success} and results: {citiesMainResults.Model.Results}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(citiesMainResults.Results);

                allErrorMessages.Add(message);
            }

            if (!areCitiesDataItemsValid)
            {
                foreach (var CitiesItemResult in CitiesItemResults.Where(x => x.Results.Any()))
                {
                    var message = $"Cities item with Name: {CitiesItemResult.Model.Name}"
                    .RequestInfo(client, request)
                    .WithValidationErrors(CitiesItemResult.Results);

                    allErrorMessages.Add(message);
                }
            }
            if (allErrorMessages.Any())
            {
                var allMessages = string.Join("\r\n\r\n", allErrorMessages);
                throw new Exception(allMessages);
            }
        }
    }
}


