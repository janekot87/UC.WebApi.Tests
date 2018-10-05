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
    public class DealerLeads
    {
        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth)]
        public void DealerLeadsTest(string digest, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("lead/list?digest={digest}", Method.GET);

            request

                .AddUrlSegment("digest", digest)
                .AddHeader("Authorization", auth);

            var response = client.Execute<DealerLeadsModel.RootObject>(request);


            if(response.StatusCode != HttpStatusCode.OK || response.Data == null || response.Data.Success == false)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request), response.StatusCode, response.Data.Success));
            }

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<DealerLeadsModel.RootObject> dealerLeadsMainResults;
            var isDealerLeadsDataValid = GlobalLogic.IsModelValid(response.Data, out dealerLeadsMainResults);

            IList<ValidationResultModel<DealerLeadsModel.Item>> DealerLeadsItemResults;
            var areDealerLeadsDataItemsValid = GlobalLogic.IsModelArrayValid(response.Data.Items, out DealerLeadsItemResults);

            if (!isDealerLeadsDataValid)
            {
                var message = $"Dealer leads with success: {dealerLeadsMainResults.Model.Success} and results: {dealerLeadsMainResults.Model.Results}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(dealerLeadsMainResults.Results);

                allErrorMessages.Add(message);
            }

            if (!areDealerLeadsDataItemsValid)
            {
                var message = $"Dealer leads items: {dealerLeadsMainResults.Model.Items}"
                .RequestInfo(client, request)
                .WithValidationErrors(dealerLeadsMainResults.Results);

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

