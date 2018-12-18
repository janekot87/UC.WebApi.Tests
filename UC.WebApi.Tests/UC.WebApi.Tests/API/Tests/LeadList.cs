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
    public class LeadList
    {
        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth)]
        public void LeadListTest(string digest, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/lead/list?digest={digest}", Method.GET);

            request
                .AddUrlSegment("digest", digest)
                .AddHeader("Authorization", auth);

            var response = client.Execute<LeadListModel.RootObject>(request);

            EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<LeadListModel.RootObject> leadListMainResults;
            var isLeadListDataValid = GlobalLogic.IsModelValid(response.Data, out leadListMainResults);

            if (!isLeadListDataValid)
            {
                var message = $"LeadList with success: {leadListMainResults.Model.Success} and results: {leadListMainResults.Model.Results}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(leadListMainResults.Results);

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