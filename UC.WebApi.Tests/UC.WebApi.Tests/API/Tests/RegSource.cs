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
    public class RegSource
    {
        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth)]
        public void RegSourceTest(string digest, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/user/reg_request_source?digest={digest}", Method.GET);

            request
                .AddUrlSegment("digest", digest)
                .AddHeader("Authorization", auth);

            var response = client.Execute<RegSourceModel.RootObject>(request);

            EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<RegSourceModel.RootObject> regSourceMainResults;
            var isRegSourceDataValid = GlobalLogic.IsModelValid(response.Data, out regSourceMainResults);

            IList<ValidationResultModel<RegSourceModel.Item>> RegSourceItemResults;
            var areRegSourceDataItemsValid = GlobalLogic.IsModelArrayValid(response.Data.Items, out RegSourceItemResults);

            if (!isRegSourceDataValid)
            {
                var message = $"Registration source with success: {regSourceMainResults.Model.Success} and results: {regSourceMainResults.Model.Results}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(regSourceMainResults.Results);

                allErrorMessages.Add(message);
            }

            if (!areRegSourceDataItemsValid)
            {
                foreach (var RegSourceItemResult in RegSourceItemResults.Where(x => x.Results.Any()))
                {
                    var message = $"Source item with Name: {RegSourceItemResult.Model.Name}"
                    .RequestInfo(client, request)
                    .WithValidationErrors(RegSourceItemResult.Results);

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
