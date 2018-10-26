using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UC.WebApi.Tests.API.DataStore;
using UC.WebApi.Tests.API.Logic;
using UC.WebApi.Tests.API.Models;
using Xunit;


namespace UC.WebApi.Tests.API.Tests
{
    public class ChangePhoneNumber
    {
        [Theory]
        [InlineData(Data.DealerName, Data.Digest, Data.NewPhone, Data.Save, Data.BasicAuth)]
        public void ChangePhoneNumberTest(string dealer, string digest, string newphone, int save, string auth)
        {
            Random generator = new Random();          

            var client1 = new RestClient(TestConfiguration.API.Location);
            var request1 = new RestRequest("/users/{dealer_name}/phone?digest={digest}", Method.PUT);
            var requestCounter = 0;
            string newPhoneRandom;
            IRestResponse<ValidateNphNumberModel> response1;

            do
            {
                
                newPhoneRandom = newphone + generator.Next(0, 100000).ToString("D5");

                request1
                .AddUrlSegment("dealer_name", dealer)
                .AddUrlSegment("digest", digest)
                .AddParameter("phone", newPhoneRandom)
                .AddHeader("Authorization", auth);

                 response1 = client1.Execute<ValidateNphNumberModel>(request1);

            } while (response1.Data.Success == false && requestCounter++ < 5);

            if (response1.StatusCode != HttpStatusCode.OK || response1.Data == null || response1.Data.Success == false)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client1.BuildUri(request1), response1.StatusCode, response1.Data.Success));
            }

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<ValidateNphNumberModel> validatenphnumberResults;
            var isValidateNphNumberDataValid = GlobalLogic.IsModelValid(response1.Data, out validatenphnumberResults);

            if (!isValidateNphNumberDataValid)
            {
                var message = $"ValidateNphNumber with success: {validatenphnumberResults.Model.Success} and description: {validatenphnumberResults.Model.Description}."
                    .RequestInfo(client1, request1)
                    .WithValidationErrors(validatenphnumberResults.Results);

                allErrorMessages.Add(message);
            }


            var client2 = new RestClient(TestConfiguration.API.Location);
            var request2 = new RestRequest("/users/{dealer_name}/phone?digest={digest}", Method.PUT);

            request2
                .AddUrlSegment("dealer_name", dealer)
                .AddUrlSegment("digest", digest)
                .AddParameter("phone", newPhoneRandom)
                .AddParameter("save", save)
                .AddHeader("Authorization", auth);

            var response2 = client2.Execute<ValidateNphNumberModel>(request2);

            if (response2.StatusCode != HttpStatusCode.OK || response2.Data == null || response2.Data.Success == false)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client2.BuildUri(request2), response2.StatusCode, response2.Data.Success));
            }

            ValidationResultModel<ValidateNphNumberModel> savenewnumberResults;
            var isSaveNewNumberDataValid = GlobalLogic.IsModelValid(response2.Data, out savenewnumberResults);

            if (!isSaveNewNumberDataValid)
            {
                var message = $"SaveNewNumber with success: {savenewnumberResults.Model.Success} and description: {savenewnumberResults.Model.Description}."
                    .RequestInfo(client2, request2)
                    .WithValidationErrors(savenewnumberResults.Results);

                allErrorMessages.Add(message);
            }

            var client3 = new RestClient(TestConfiguration.API.Location);
            var request3 = new RestRequest("/users/{dealer_name}", Method.GET);

            request3
                .AddUrlSegment("dealer_name", dealer)
                .AddParameter("digest", digest)
                .AddHeader("Authorization", auth);

            var response3 = client3.Execute<DealerDataModel.RootObject>(request3);

            if (!response3.Data.Items.Phone.Equals("91" + newPhoneRandom))
            {
                var message = $"Updated phone number from DealerData: {response3.Data.Items.Phone} doesn't equal the expected value: {newPhoneRandom}.";

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