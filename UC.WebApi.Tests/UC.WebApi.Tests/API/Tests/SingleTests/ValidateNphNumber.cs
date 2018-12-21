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
    public class ValidateNphNumber
    {
        [SkippableTheory]
        [InlineData(Data.DealerName, Data.Digest, Data.NewPhone, Data.BasicAuth)]
        public void ValidateNphNumberTest(string dealer, string digest, string newPhone, string auth)
        {
            var generator = new Random();

            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/users/{dealer_name}/phone?digest={digest}", Method.PUT);
            var requestCounter = 0;
            string newPhoneRandom;
            IRestResponse<ValidateNphNumberModel> response;

            do
            {
                newPhoneRandom = newPhone + generator.Next(0, 100000).ToString("D5");
                request
                .AddUrlSegment("dealer_name", dealer)
                .AddUrlSegment("digest", digest)
                .AddParameter("phone", newPhoneRandom)
                .AddHeader("Authorization", auth);

                response = client.Execute<ValidateNphNumberModel>(request);

            } while (response.Data.Success == false && requestCounter++ < 5);

            EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<ValidateNphNumberModel> validatenphnumberResults;
            var isValidateNphNumberDataValid = GlobalLogic.IsModelValid(response.Data, out validatenphnumberResults);

            if (!isValidateNphNumberDataValid)
            {
                var message = $"ValidateNphNumber with success: {validatenphnumberResults.Model.Success} and description: {validatenphnumberResults.Model.Description}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(validatenphnumberResults.Results);

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


        

