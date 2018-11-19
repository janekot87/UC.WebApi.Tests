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

            EnsureOkResponseStatusCode(response, client, request);

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
                foreach (var DealerLeadsItemResult in DealerLeadsItemResults.Where(x => x.Results.Any()))
                {
                    var message = $"Dealer lead item with Classified_id: {DealerLeadsItemResult.Model.Classified_id}"
                    .RequestInfo(client, request)
                    .WithValidationErrors(DealerLeadsItemResult.Results);

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

