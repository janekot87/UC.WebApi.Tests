using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UC.WebApi.Tests.API.Attributes;
using UC.WebApi.Tests.API.DataStore;
using UC.WebApi.Tests.API.Logic;
using UC.WebApi.Tests.API.Models;
using Xunit;
using static UC.WebApi.Tests.API.Logic.GlobalLogic;


namespace UC.WebApi.Tests.API.Tests
{
    public class PResetEmail
    {
        [SkippableTheory]
        [InlineData(Data.Digest, Data.Email, Data.BasicAuth, Data.ContentType)]
        public void RegisterRequestValidTest(string digest, string email, string auth, string contentType)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("user/pass_mail?digest={digest}", Method.POST);

            request
                .AddUrlSegment("digest", digest)
                .AddHeader("Content-Type", contentType)
                .AddHeader("Authorization", auth)
                .AddJsonBody(new
                {

                    email = email

                });


            var response = client.Execute<RegisterRequestModel>(request);

            EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<RegisterRequestModel> presetEmailResults;
            var isPResetEmailValid = GlobalLogic.IsModelValid(response.Data, out presetEmailResults);

            if (!isPResetEmailValid)
            {
                var message = $"PResetEmail with success: {presetEmailResults.Model.Success}"
                    .RequestInfo(client, request)
                    .WithValidationErrors(presetEmailResults.Results);

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


        




        
    
