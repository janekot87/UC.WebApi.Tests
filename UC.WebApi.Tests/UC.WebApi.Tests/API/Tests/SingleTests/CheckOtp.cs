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
    public class CheckOtp
    {
                
            [Theory]
            [InlineData(Data.Digest, Data.Phone, Data.Otp, Data.BasicAuth)]
            public void CheckOtpTest(string digest, string phone, int otp, string auth)
            {
                var client = new RestClient(TestConfiguration.API.Location);
                var request = new RestRequest("/user/otp_check", Method.GET);

                request
                    .AddParameter("digest", digest)
                    .AddParameter("phone", phone)
                     .AddParameter("otp", otp)
                    .AddHeader("Authorization", auth);

                var response = client.Execute<CheckOtpModel>(request);

                EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<CheckOtpModel> checkotpResults;
            var isCheckOtpDataValid = GlobalLogic.IsModelValid(response.Data, out checkotpResults);

            if (!isCheckOtpDataValid)
            {
                var message = $"CheckOtp with success: {checkotpResults.Model.Success} and username: {checkotpResults.Model.Username}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(checkotpResults.Results);

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

