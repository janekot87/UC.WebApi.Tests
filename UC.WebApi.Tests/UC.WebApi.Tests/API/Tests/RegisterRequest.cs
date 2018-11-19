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
    public class RegisterRequest
    {

        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth, Data.ContentType)]
        public void RegisterRequestValidTest(string digest, string auth, string contentType)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/user/reg_requests?digest={digest}", Method.POST);

            request
                .AddUrlSegment("digest", digest)
                .AddHeader("Content-Type", contentType)
                .AddHeader("Authorization", auth)
                .AddJsonBody(new
                {
                    name = "Testttt11",
                    phone = "+9198765418",
                    city = "New Delhi",
                    dealer_type = "1",
                    source = "Android_app"
                });


            var response = client.Execute<RegisterRequestModel>(request);

            EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<RegisterRequestModel> registerRequestResults;
            var isRegisterRequestValid = GlobalLogic.IsModelValid(response.Data, out registerRequestResults);

            if (!isRegisterRequestValid)
            {
                var message = $"Register Request with success: {registerRequestResults.Model.Success}"
                    .RequestInfo(client, request)
                    .WithValidationErrors(registerRequestResults.Results);

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
