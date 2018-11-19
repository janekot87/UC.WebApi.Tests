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
    public class ReadClassified
    {
        [Theory]
        [InlineData(Data.Guid, Data.Digest, Data.BasicAuth)]
        public void ClassifiedDataTest(string guid, string digest, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/objects/{guid}", Method.GET);

            request
                .AddUrlSegment("guid", guid)
                .AddParameter("digest", digest)
                .AddHeader("Authorization", auth);

            var response = client.Execute<ReadClassifiedModel.RootObject>(request);

            EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<ReadClassifiedModel.RootObject> classifiedDataMainResults;
            var isClassifiedDataValid = GlobalLogic.IsModelValid(response.Data, out classifiedDataMainResults);

            ValidationResultModel<ReadClassifiedModel.Items> classifiedDataItemsResults;
            var areClassifiedDataItemsValid = GlobalLogic.IsModelValid(response.Data.Items, out classifiedDataItemsResults);

            if (!isClassifiedDataValid)
            {
                var message = $"Classified with success: {classifiedDataMainResults.Model.Success} and results: {classifiedDataMainResults.Model.Results}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(classifiedDataMainResults.Results);

                allErrorMessages.Add(message);
            }

            if (!areClassifiedDataItemsValid)
            {
                var message = $"Classified items with guid: {classifiedDataItemsResults.Model.guid}"
                    .RequestInfo(client, request)
                    .WithValidationErrors(classifiedDataItemsResults.Results);

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
