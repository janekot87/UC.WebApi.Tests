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
    public class ContactUs
    {

        [Theory]
        [InlineData(Data.Digest,Data.BasicAuth, Data.ContentType)]
        public void ContactUsTest(string digest, string auth, string contentType)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/user/contact_us?digest={digest}", Method.POST);

            request
                .AddUrlSegment("digest", digest)
                .AddHeader("Content-Type", contentType)
                .AddHeader("Authorization", auth)
                .AddJsonBody(new
                {
                    username = Data.DealerName,
                    message = "Hello World!"
                    
                });


            var response = client.Execute<RegisterRequestModel>(request);

            if (response.StatusCode != HttpStatusCode.OK || response.Data == null || response.Data.Success == false)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request), response.StatusCode, response.Data.Success));
            }

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<RegisterRequestModel> contactUsResults;
            var isRegisterRequestValid = GlobalLogic.IsModelValid(response.Data, out contactUsResults);

            if (!isRegisterRequestValid)
            {
                var message = $"Contact us with success: {contactUsResults.Model.Success}"
                    .RequestInfo(client, request)
                    .WithValidationErrors(contactUsResults.Results);

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
