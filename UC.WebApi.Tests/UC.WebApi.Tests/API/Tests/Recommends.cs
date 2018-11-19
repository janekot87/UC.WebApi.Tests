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

            EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<RecommendsModel.RootObject> recommendsMainResults;
            var isRecommendsDataValid = GlobalLogic.IsModelValid(response.Data, out recommendsMainResults);

            IList<ValidationResultModel<RecommendsModel.Item>> recommendsAndRecentlyAddedResults;
            var areRecommendsAndRecentlyAddedItemsValid = GlobalLogic.IsModelArrayValid(response.Data.RecentlyAdded.Concat(response.Data.Recommendations), out recommendsAndRecentlyAddedResults);

            if (!isRecommendsDataValid)
            {
                var message = $"Recommends with success: {recommendsMainResults.Model.Success}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(recommendsMainResults.Results);

                allErrorMessages.Add(message);
            }

            if (!areRecommendsAndRecentlyAddedItemsValid)
            {
                foreach (var recommendsAndRecentlyAddedResult in recommendsAndRecentlyAddedResults.Where(x => x.Results.Any()))
                {
                    var message = $"RecentlyAdded item with Guid: {recommendsAndRecentlyAddedResult.Model.Guid}"
                    .RequestInfo(client, request)
                    .WithValidationErrors(recommendsAndRecentlyAddedResult.Results);

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



        
    
