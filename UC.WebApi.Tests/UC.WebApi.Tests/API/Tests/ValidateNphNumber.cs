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
    public class ValidateNphNumber
    {
        [Theory]
        [InlineData(Data.DealerName, Data.Digest, Data.NewPhone, Data.BasicAuth)]
        public void ValidateNphNumberTest(string dealer, string digest, string newphone, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/users/{dealer_name}/phone?digest={digest}", Method.PUT);

            request
                .AddUrlSegment("dealer_name", dealer)
                .AddUrlSegment("digest", digest)
                .AddParameter("phone", newphone)
                .AddHeader("Authorization", auth);

            var response = client.Execute<ValidateNphNumberModel>(request);

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


        

