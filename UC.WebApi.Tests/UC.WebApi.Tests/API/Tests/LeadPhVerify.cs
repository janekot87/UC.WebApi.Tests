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
        [InlineData(Data.Digest,  Data.BasicAuth, Data.NewPhone)]
        public void PhVerifyTest(string digest, string auth, string newPhone)
        {
            var generator = new Random();
            var client = new RestClient(TestConfiguration.API.Location);
            var request1 = new RestRequest("/sms/verify-phone-v2", Method.GET);
            var requestCounter = 0;
            string newPhoneRandom;
            IRestResponse<PhVerifyModel> response1;
            do
            {
                newPhoneRandom = newPhone + generator.Next(0, 100000).ToString("D5");

                request1
                .AddParameter("digest", digest)
                .AddParameter("phone", newPhoneRandom)
                .AddHeader("Authorization", auth);

                response1 = client.Execute<PhVerifyModel>(request1);
            } while (response1.Data.Success == false && requestCounter++ < 5);

            EnsureOkResponseStatusCode(response1, client, request1);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<PhVerifyModel> phverifyResults;
            var isPhVerifyDataValid = GlobalLogic.IsModelValid(response1.Data, out phverifyResults);

            if (!isPhVerifyDataValid)
            {
                var message = $"PhVerify with success: {phverifyResults.Model.Success} and message: {phverifyResults.Model.Message}."
                    .RequestInfo(client, request1)
                    .WithValidationErrors(phverifyResults.Results);

                allErrorMessages.Add(message);
            }


            /*var request3 = new RestRequest("/sms/verify-phone-v2", Method.GET);

                request3
                .AddParameter("digest", digest)
                .AddParameter("phone", newPhoneRandom)
                .AddParameter("code", response2.Data.Verify_code)
                .AddHeader("Authorization", auth);

            var response3 = client.Execute<RegisterRequestModel>(request3);

            EnsureOkResponseStatusCode(response3, client, request3);

            ValidationResultModel<RegisterRequestModel> codeVerifyResults;
            var isCodeVerifyDataValid = GlobalLogic.IsModelValid(response3.Data, out codeVerifyResults);

            if (!isCodeVerifyDataValid)
            {
                var message = $"CodeVerify with success: {codeVerifyResults.Model.Success}."
                    .RequestInfo(client, request3)
                    .WithValidationErrors(codeVerifyResults.Results);

                allErrorMessages.Add(message);
            }
            */
            

            if (allErrorMessages.Any())
            {
                var allMessages = string.Join("\r\n\r\n", allErrorMessages);
                throw new Exception(allMessages);
            }
        }
    }
}
