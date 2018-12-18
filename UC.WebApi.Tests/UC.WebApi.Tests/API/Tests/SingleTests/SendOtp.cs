﻿using RestSharp;
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
    public class SendOtp
    {
        [Theory]
        [InlineData(Data.Digest, Data.Phone, Data.BasicAuth, Data.ContentType)]
        public void RegisterRequestValidTest(string digest, string phone, string auth, string contentType)
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

            EnsureOkResponseStatusCode(response, client, request);


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


        
