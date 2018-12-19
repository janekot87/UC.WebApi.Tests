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
    public class ResetPasswordViaPhone
    {
        [Theory]
        [InlineData(Data.Digest, Data.Phone, Data.Password, Data.BasicAuth, Data.ContentType)]
        public void ResetPasswordViaPhoneTest(string digest, string phone, string password, string auth, string contentType)
        {
            var client = new RestClient(TestConfiguration.API.Location);

            //Send OTP
            var request1 = new RestRequest("user/otp_set?digest={digest}", Method.POST);

            request1
                .AddUrlSegment("digest", digest)
                .AddHeader("Content-Type", contentType)
                .AddHeader("Authorization", auth)
                .AddJsonBody(new
                {

                    phone = phone

                });


            var response1 = client.Execute<RegisterRequestModel>(request1);

            EnsureOkResponseStatusCode(response1, client, request1);


            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<RegisterRequestModel> sendOtpResults;
            var isSendOtpValid = GlobalLogic.IsModelValid(response1.Data, out sendOtpResults);

            if (!isSendOtpValid)
            {
                var message = $"SendOtp with success: {sendOtpResults.Model.Success}"
                    .RequestInfo(client, request1)
                    .WithValidationErrors(sendOtpResults.Results);

                allErrorMessages.Add(message);
            }

         

           //Get OTP
            var request2 = new RestRequest("user/otp_get?digest={digest}", Method.GET);

            request2
               .AddUrlSegment("digest", digest)
               .AddHeader("Content-Type", contentType)
               .AddHeader("Authorization", auth)
               .AddParameter("phone", phone);



            var response2 = client.Execute<GetOtpModel>(request2);

            EnsureOkResponseStatusCode(response2, client, request2);

            ValidationResultModel<GetOtpModel> getOtpResults;
            var isGetOtpValid = GlobalLogic.IsModelValid(response2.Data, out getOtpResults);

            if (!isGetOtpValid)
            {
                var message = $"GetOtp with success: {getOtpResults.Model.Success} and Otp: {getOtpResults.Model.Otp_code}."
                    .RequestInfo(client, request2)
                    .WithValidationErrors(sendOtpResults.Results);

                allErrorMessages.Add(message);
            }

           //Check OTP
            var request3 = new RestRequest("/user/otp_check", Method.GET);

            request3
                .AddParameter("digest", digest)
                .AddParameter("phone", phone)
                 .AddParameter("otp", response2.Data.Otp_code) 
                .AddHeader("Authorization", auth);

            var response3 = client.Execute<CheckOtpModel>(request3);

            EnsureOkResponseStatusCode(response3, client, request3);

      
            ValidationResultModel<CheckOtpModel> checkOtpResults;
            var isCheckOtpDataValid = GlobalLogic.IsModelValid(response3.Data, out checkOtpResults);

            if (!isCheckOtpDataValid)
            {
                var message = $"CheckOtp with success: {checkOtpResults.Model.Success} and username: {checkOtpResults.Model.Username}."
                    .RequestInfo(client, request3)
                    .WithValidationErrors(checkOtpResults.Results);

                allErrorMessages.Add(message);

            }

           //Change Password
            var request4 = new RestRequest("user/renew_pass?digest={digest}", Method.POST);

            request4
                .AddUrlSegment("digest", digest)
                .AddHeader("Content-Type", contentType)
                .AddHeader("Authorization", auth)
                .AddJsonBody(new
                {

                    phone = phone,
                    otp = response2.Data.Otp_code, 
                    password = password

                });


            var response4 = client.Execute<RegisterRequestModel>(request4);

            EnsureOkResponseStatusCode(response4, client, request4);

                   
            ValidationResultModel<RegisterRequestModel> updatePasswordResults;
            var isUpdatePasswordValid = GlobalLogic.IsModelValid(response4.Data, out updatePasswordResults);

            if (!isUpdatePasswordValid)
            {
                var message = $"UpdatePassword with success: {updatePasswordResults.Model.Success}"
                    .RequestInfo(client, request4)
                    .WithValidationErrors(updatePasswordResults.Results);

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
