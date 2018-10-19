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
    public class ChPassViaEmail
    {
        [Theory]
        [InlineData(Data.Digest, Data.EmailGuid, Data.Pass, Data.BasicAuth)]
        public void ChPassViaEmailTest(string digest, string emailguid, int pass, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/user/pass_reset/{emailguid}", Method.POST);

            request
                .AddUrlSegment("emailguid", emailguid)
                .AddParameter("digest", digest)
                .AddHeader("Authorization", auth)
                .AddJsonBody(new
                {
                    passpassword = pass,

                });

            var response = client.Execute<RegisterRequestModel>(request);

            if (response.StatusCode != HttpStatusCode.OK || response.Data == null || response.Data.Success == false)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request), response.StatusCode, response.Data.Success));
            }

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<RegisterRequestModel> chpassviaemailResults;
            var isChPassViaEmailDataValid = GlobalLogic.IsModelValid(response.Data, out chpassviaemailResults);

            if (!isChPassViaEmailDataValid)
            {
                var message = $"ChPassViaEmail with success: {chpassviaemailResults.Model.Success}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(chpassviaemailResults.Results);

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
