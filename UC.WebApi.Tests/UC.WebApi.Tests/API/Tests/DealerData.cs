using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UC.WebApi.Tests.API.DataStore;
using UC.WebApi.Tests.API.Logic;
using UC.WebApi.Tests.API.Models;
using Xunit;

namespace UC.WebApi.Tests
{
    public class DealerData
    {
        [Theory]
        [InlineData(Data.DealerName, Data.Digest, Data.BasicAuth)]
        public void DealerDataTest(string dealer, string digest, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/index.php/api/v2/users/{dealer_name}", Method.GET);

            request
                .AddUrlSegment("dealer_name", dealer)
                .AddParameter("digest", digest)
                .AddHeader("Authorization", auth);

            var response = client.Execute<DealerDataModel.RootObject>(request);

            //Response.isValid(response);
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

            ValidationResultModel<DealerDataModel.RootObject> dealerDataMainResults;
            var isDealerDataValid = GlobalLogic.IsModelValid(response.Data, out dealerDataMainResults);

            ValidationResultModel<DealerDataModel.Items> dealerDataItemResults;
            var areDealerDataItemsValid = GlobalLogic.IsModelValid(response.Data.Items, out dealerDataItemResults);

            if (!isDealerDataValid)
            {
                var message = $"Dealer with success: {dealerDataMainResults.Model.Success} and results: {dealerDataMainResults.Model.Results}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(dealerDataMainResults.Results);

                allErrorMessages.Add(message);
            }

            if (!areDealerDataItemsValid)
            {
                var message = $"Dealer items with user name: {dealerDataMainResults.Model.Items.User_name}"
                    .RequestInfo(client, request)
                    .WithValidationErrors(dealerDataMainResults.Results);

                allErrorMessages.Add(message);
            }

            if (allErrorMessages.Any())
            {
                var allMessages = string.Join("\r\n\r\n", allErrorMessages);
                throw new Exception(allMessages);
            }
        }


        [Fact]
        public void InvalidDealerNameTest()
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/index.php/api/v2/users/{dealer_name}", Method.GET);

            request
                .AddUrlSegment("dealer_name", "Jane2018_invalid")
                .AddParameter("digest", Data.Digest)
                .AddHeader("Authorization", Data.BasicAuth);

            var response = client.Execute<DealerDataModel.RootObject>(request);

            if (response.StatusCode != HttpStatusCode.OK || response.Data == null)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request), response.StatusCode));
            }

            if (!(response.Content.Contains("false") && response.Content.Contains("Query does not exist")))
            {
                var response1 = client.Execute<ErrorModel.RootObject>(request);
                throw new Exception(AssertMessages.InvalidDealerNameErrorMessage(response1.Data.Description, response1.Data.Error_code, client.BuildUri(request)));
            }
        }


        [Fact]
        public void InvalidDealerDigestTest()
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/index.php/api/v2/users/{dealer_name}", Method.GET);

            request
                .AddUrlSegment("dealer_name", Data.DealerName)
                .AddParameter("digest", "gLqVqvqHTxZk9RHxwtPjkbWtNAbdBpGh_invalid")
                .AddHeader("Authorization", Data.BasicAuth);

            var response = client.Execute<DealerDataModel.RootObject>(request);

            if (response.StatusCode != HttpStatusCode.OK || response.Data == null)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request), response.StatusCode));
            }

            if (!(response.Content.Contains("false") && response.Content.Contains("Auth fail")))
            {
                var response1 = client.Execute<ErrorModel.AuthFail>(request);
                throw new Exception(AssertMessages.InvalidDigestErrorMessage(response1.Data.Success, response1.Data.Message, client.BuildUri(request)));
            }
        }

        [Fact]
        public void InvalidDealerAuthorizationTest()
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/index.php/api/v2/users/{dealer_name}", Method.GET);

            request
                .AddUrlSegment("dealer_name", Data.DealerName)
                .AddParameter("digest", Data.Digest)
                .AddHeader("Authorization", "Basic dXNlZGNhcnNfYXBpOmhBQzMyVFdCUzY_invalid=");

            var response = client.Execute<DealerDataModel.RootObject>(request);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request), response.StatusCode));
            }
        }

    }
}
