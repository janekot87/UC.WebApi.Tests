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
    public class FModels
    {

        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth)]
        public void FBrandsTest(string digest, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/models/foreign?digest={digest}", Method.GET);

            request
                .AddUrlSegment("digest", digest)
                .AddHeader("Authorization", auth);

            var response = client.Execute<FModelsScheme.RootObject>(request);

            EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<FModelsScheme.RootObject> fModelsMainResults;
            var isFModelsDataValid = GlobalLogic.IsModelValid(response.Data, out fModelsMainResults);

            if (!isFModelsDataValid)
            {
                var message = $"Foreign Models with success: {fModelsMainResults.Model.Success} and results: {fModelsMainResults.Model.Results}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(fModelsMainResults.Results);

                allErrorMessages.Add(message);
            }

            if (allErrorMessages.Any())
            {
                var allMessages = string.Join("\r\n\r\n", allErrorMessages);
                throw new Exception(allMessages);
            }



        }
    }
}
