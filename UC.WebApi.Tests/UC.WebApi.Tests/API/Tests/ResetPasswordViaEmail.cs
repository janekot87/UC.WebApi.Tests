using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using UC.WebApi.Tests.API.DataStore;
using UC.WebApi.Tests.API.Logic;
using UC.WebApi.Tests.API.Models;
using Xunit;
using static UC.WebApi.Tests.API.Logic.GlobalLogic;
namespace UC.WebApi.Tests.API.Tests
{
    public class ResetPasswordViaEmail
    {
        [Theory]
        [InlineData(Data.Digest, Data.Email, Data.Password, Data.BasicAuth, Data.ContentType)]
        public void ResetPasswordViaEmailTest(string digest, string email, string password, string auth,string contentType)
        {
            var client = new RestClient(TestConfiguration.API.Location);

            //Send Password Reset Email
            var request1 = new RestRequest("user/pass_mail?digest={digest}", Method.POST);

            request1
                .AddUrlSegment("digest", digest)
                .AddHeader("Content-Type", contentType)
                .AddHeader("Authorization", auth)
                .AddJsonBody(new
                {

                    email = email

                });


            var response1 = client.Execute<RegisterRequestModel>(request1);

            EnsureOkResponseStatusCode(response1, client, request1);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<RegisterRequestModel> presetEmailResults;
            var isPResetEmailValid = GlobalLogic.IsModelValid(response1.Data, out presetEmailResults);

            if (!isPResetEmailValid)
            {
                var message = $"PResetEmail with success: {presetEmailResults.Model.Success}"
                    .RequestInfo(client, request1)
                    .WithValidationErrors(presetEmailResults.Results);

                allErrorMessages.Add(message);
            }

            //Get Guid
            var request2 = new RestRequest("user/get_pass_reset?digest={digest}", Method.GET);

            request2
                .AddUrlSegment("digest", digest)
                .AddHeader("Content-Type", contentType)
                .AddHeader("Authorization", auth)
                .AddParameter("email", email);



            var response2 = client.Execute<GetGuidModel>(request2);

            EnsureOkResponseStatusCode(response2, client, request2);

            ValidationResultModel<GetGuidModel> getEmailResults;
            var isGetEmailValid = GlobalLogic.IsModelValid(response2.Data, out getEmailResults);

            if (!isGetEmailValid)
            {
                var message = $"GetEmail with success: {getEmailResults.Model.Success} and Guid: {getEmailResults.Model.Guid}."
                    .RequestInfo(client, request2)
                    .WithValidationErrors(getEmailResults.Results);

                allErrorMessages.Add(message);
            }

            //Check Guid
            var request3 = new RestRequest("/user/pass_reset/{guid}", Method.GET);

            request3
                .AddUrlSegment("guid", response2.Data.Guid)
                .AddParameter("digest", digest)
                .AddHeader("Authorization", auth);

            var response3 = client.Execute<RegisterRequestModel>(request3);

            EnsureOkResponseStatusCode(response3, client, request3);

           
            ValidationResultModel<RegisterRequestModel> checkguidResults;
            var isCheckOtpDataValid = GlobalLogic.IsModelValid(response3.Data, out checkguidResults);

            if (!isCheckOtpDataValid)
            {
                var message = $"CheckGuid with success: {checkguidResults.Model.Success}."
                    .RequestInfo(client, request3)
                    .WithValidationErrors(checkguidResults.Results);

                allErrorMessages.Add(message);
            }

            //Change Password
            var request4 = new RestRequest("/user/pass_reset/{guid}?digest={digest}", Method.POST);

            request4
                .AddUrlSegment("guid", response2.Data.Guid)
                .AddUrlSegment("digest", digest)
                .AddHeader("Authorization", auth)
                .AddJsonBody(new
                {
                    password = password

                });

            var response4 = client.Execute<RegisterRequestModel>(request4);

            EnsureOkResponseStatusCode(response4, client, request4);

           
            ValidationResultModel<RegisterRequestModel> chPassViaEmailResults;
            var isChPassViaEmailDataValid = GlobalLogic.IsModelValid(response4.Data, out chPassViaEmailResults);

            if (!isChPassViaEmailDataValid)
            {
                var message = $"ChPassViaEmail with success: {chPassViaEmailResults.Model.Success}."
                    .RequestInfo(client, request4)
                    .WithValidationErrors(chPassViaEmailResults.Results);

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
