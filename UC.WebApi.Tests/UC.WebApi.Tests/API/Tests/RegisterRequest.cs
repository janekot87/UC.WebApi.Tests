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
    public class RegisterRequest
    {

        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth, Data.ContentType)]
        public void RegisterRequestValidTest(string digest, string auth, string contentType)
        {
            var client = new RestClient("http://dev-usedcars-api.autoportal.com/api.php/api/v2");
            var request = new RestRequest("user/reg_requests", Method.POST);

            //request.RequestFormat = DataFormat.Json;
            request
                .AddParameter("digest", digest)
                .AddHeader("Content-Type", contentType)
                .AddHeader("Authorization", auth)
                .AddJsonBody(new
                {
                    name = "Testttt",
                    phone = "+9198765419",
                    city = "New Delhi",
                    dealer_type = "1",
                    source = "Android_app"
                });


            var response = client.Execute<RegisterRequestModel>(request);

            if (response.StatusCode != HttpStatusCode.OK || response.Data == null)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request), response.StatusCode));
            }

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
