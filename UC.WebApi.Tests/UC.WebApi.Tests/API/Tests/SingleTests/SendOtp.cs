using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using UC.WebApi.Tests.API.Attributes;
using UC.WebApi.Tests.API.DataStore;
using UC.WebApi.Tests.API.Logic;
using UC.WebApi.Tests.API.Models;
using Xunit;

namespace UC.WebApi.Tests.API.Tests.SingleTests
{
    public class SendOtp
    {
        [SkippableTheory]
        [InlineData(Data.Digest, Data.Phone, Data.BasicAuth, Data.ContentType)]
        public void SendOtpTest(string digest, string phone, string auth, string contentType)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("user/otp_set?digest={digest}", Method.POST);

            request
                .AddUrlSegment("digest", digest)
                .AddHeader("Content-Type", contentType)
                .AddHeader("Authorization", auth)
                .AddJsonBody(new
                {

                    phone = phone

                });


            var response = client.Execute<RegisterRequestModel>(request);

            GlobalLogic.EnsureOkResponseStatusCode(response, client, request);


            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<RegisterRequestModel> sendotpResults;
            var isSendOtpValid = GlobalLogic.IsModelValid(response.Data, out sendotpResults);

            if (!isSendOtpValid)
            {
                var message = $"SendOtp with success: {sendotpResults.Model.Success}"
                    .RequestInfo(client, request)
                    .WithValidationErrors(sendotpResults.Results);

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




