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
    public class CheckLoginAndPass
    {

        [Theory]
        [InlineData(Data.Phone, Data.Digest, Data.Otp, Data.Userdata, Data.BasicAuth)]
        public void CheckLoginAndPassTest(string phone, string digest, int otp, string userdata, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/user/check-user", Method.POST);

            request
                .AddParameter("digest", digest)
                .AddParameter("phone", phone)
                 .AddParameter("otp", otp)
                 .AddParameter("userdata", userdata)
                .AddHeader("Authorization", auth);

            var response = client.Execute<CheckLandPModel>(request);

            EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<CheckLandPModel> checklandpResults;
            var isCheckLandPDataValid = GlobalLogic.IsModelValid(response.Data, out checklandpResults);

            if (!isCheckLandPDataValid)
            {
                var message = $"CheckLoginAndPass with success: {checklandpResults.Model.Success} and description: {checklandpResults.Model.Description}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(checklandpResults.Results);

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
