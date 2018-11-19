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
    public class Feedback
    {

        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth, Data.ContentType)]
        public void FeedbackTest(string digest, string auth, string contentType)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/user/feedback?digest={digest}", Method.POST);

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

            EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<RegisterRequestModel> feedbackResults;
            var isFeedbackValid = GlobalLogic.IsModelValid(response.Data, out feedbackResults);

            if (!isFeedbackValid)
            {
                var message = $"Feedback with success: {feedbackResults.Model.Success}"
                    .RequestInfo(client, request)
                    .WithValidationErrors(feedbackResults.Results);

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
