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
    public class FBrands
    {

        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth)]
        public void FBrandsTest(string digest, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/brands/foreign?digest={digest}", Method.GET);

            request
                .AddUrlSegment("digest", digest)
                .AddHeader("Authorization", auth);

            var response = client.Execute<FBrandsModel.RootObject>(request);

            EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<FBrandsModel.RootObject> fBrandsMainResults;
            var isFBrandsDataValid = GlobalLogic.IsModelValid(response.Data, out fBrandsMainResults);

            if (!isFBrandsDataValid)
            {
                var message = $"Foreign Brands with success: {fBrandsMainResults.Model.Success} and results: {fBrandsMainResults.Model.Results}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(fBrandsMainResults.Results);

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
