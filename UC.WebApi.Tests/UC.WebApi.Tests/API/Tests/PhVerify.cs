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
    public class PhVerify
    {
        [Theory]
        [InlineData(Data.Digest, Data.Phone, Data.BasicAuth)]
        public void PhVerifyTest(string digest, string phone, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/sms/verify-phone-v2", Method.GET);

            request
                .AddParameter("digest", digest)
                .AddParameter("phone", phone)
                .AddHeader("Authorization", auth);

            var response = client.Execute<PhVerifyModel.RootObject>(request);

            if (response.StatusCode != HttpStatusCode.OK || response.Data == null || response.Data.Success == false)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request), response.StatusCode, response.Data.Success));
            }

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<PhVerifyModel.RootObject> phverifyMainResults;
            var isPhVerifyDataValid = GlobalLogic.IsModelValid(response.Data, out phverifyMainResults);

            if (!isPhVerifyDataValid)
            {
                var message = $"PhVerify with success: {phverifyMainResults.Model.Success} and message: {phverifyMainResults.Model.Message}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(phverifyMainResults.Results);

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
