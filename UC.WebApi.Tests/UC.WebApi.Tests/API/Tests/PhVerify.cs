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

            var response = client.Execute<PhVerifyModel>(request);

            EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<PhVerifyModel> phverifyResults;
            var isPhVerifyDataValid = GlobalLogic.IsModelValid(response.Data, out phverifyResults);

            if (!isPhVerifyDataValid)
            {
                var message = $"PhVerify with success: {phverifyResults.Model.Success} and message: {phverifyResults.Model.Message}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(phverifyResults.Results);

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
