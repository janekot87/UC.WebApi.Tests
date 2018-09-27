using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UC.WebApi.Tests.API.DataStore;
using UC.WebApi.Tests.API.Logic;
using UC.WebApi.Tests.API.Models;
using Xunit;

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

          
            if (response.StatusCode != HttpStatusCode.OK || response.Data == null)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request), response.StatusCode));
            }

            if (!response.Data.Success)
            {
                //var response1 = client.Execute<ErrorModel.RootObject>(request);
                //throw new Exception(AssertMessages.InvalidDealerNameErrorMessage(response.Data.Description, response.Data.Error_code, client.BuildUri(request)));
            }

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<ReadClassifiedModel.RootObject> classifiedDataMainResults;
            var isClassifiedDataValid = GlobalLogic.IsModelValid(response.Data, out classifiedDataMainResults);

            ValidationResultModel<ReadClassifiedModel.Items> dealerDataItemResults;
            var areClassifiedDataItemsValid = GlobalLogic.IsModelValid(response.Data.Items, out dealerDataItemResults);

            if (!isClassifiedDataValid)
            {
                var message = $"Classified with success: {classifiedDataMainResults.Model.Success} and results: {classifiedDataMainResults.Model.Results}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(classifiedDataMainResults.Results);

                allErrorMessages.Add(message);
            }

            if (!areClassifiedDataItemsValid)
            {
                var message = $"Classified items with guid: {classifiedDataMainResults.Model.Items.guid}"
                    .RequestInfo(client, request)
                    .WithValidationErrors(classifiedDataMainResults.Results);

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
