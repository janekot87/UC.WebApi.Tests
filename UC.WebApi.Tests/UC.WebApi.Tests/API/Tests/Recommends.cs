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
    public class Recommends
    {
        [Theory]
        [InlineData(Data.Digest, Data.Guid, Data.User_id, Data.BasicAuth)]
        public void RecommendsTest(string digest, string guid, int user_id, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/recommends", Method.GET);


            request

                .AddParameter("digest", digest)
                .AddParameter("guid", guid)
                .AddParameter("user_id", user_id)
                .AddHeader("Authorization", auth);

            var response = client.Execute<RecommendsModel.RootObject>(request);

            if (response.StatusCode != HttpStatusCode.OK || response.Data == null || response.Data.Success == false)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request), response.StatusCode, response.Data.Success));
            }

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<RecommendsModel.RootObject> recommendsMainResults;
            var isRecommendsDataValid = GlobalLogic.IsModelValid(response.Data, out recommendsMainResults);

            IList<ValidationResultModel<RecommendsModel.RecentlyAdded>> RecentlyAddedResults;
            var areRecentlyAddedItemsValid = GlobalLogic.IsModelArrayValid(response.Data.RecentlyAddedItems, out RecentlyAddedResults);

            if (!isRecommendsDataValid)
            {
                var message = $"Recommends with success: {recommendsMainResults.Model.Success}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(recommendsMainResults.Results);

                allErrorMessages.Add(message);
            }

            if (!areRecentlyAddedItemsValid)
            {
                foreach (var RecentlyAddedResult in RecentlyAddedResults.Where(x => x.Results.Any()))
                {
                    var message = $"RecentlyAdded item with Guid: {RecentlyAddedResult.Model.Guid}"
                    .RequestInfo(client, request)
                    .WithValidationErrors(RecentlyAddedResult.Results);

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



        
    
