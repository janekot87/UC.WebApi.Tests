/*using RestSharp;
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
   public class LeadsCreating
    {
        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth, Data.Email, Data.DealerName, Data.Phone, Data.User_id)]
        public void LeadsCreatingTest(string digest, string auth, string email, string dealerName, string phone, int user_id)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request1 = new RestRequest("/objects?digest={digest}", Method.POST);

            //Create Classified
            request1.RequestFormat = DataFormat.Json;
            request1
                .AddHeader("Authorization", auth)
                .AddUrlSegment("digest", digest)
                .AddJsonBody(
                new
                {
                    data =
                    new
                    {
                        user_id = dealerName,
                        year = "2000",
                        city = "noida",
                        model =
                        new
                        {
                            model = "astonmartin-db9",
                            brand = "astonmartin"
                        },
                        price = "777777",
                        variant_id = "astonmartin-db9-coupe",
                        km_driven = 22222,
                        owners = 1,
                        color = "white",
                        fuel_type = "Petrol",
                        transmission = "Manual",
                        engine = 3333,
                        body_type = "Sedan",
                        have_certificated = "no",
                        classified_phone = "+91-9999999999",
                        address = "Test",
                        status = 1,
                        source = "Cabinet"
                    }
                });

            var response1 = client.Execute<CreateClassifiedModel>(request1);

            EnsureOkResponseStatusCode(response1, client, request1);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<CreateClassifiedModel> createClassifiedResults;
            var isCreateClassifiedValid = GlobalLogic.IsModelValid(response1.Data, out createClassifiedResults);

            if (!isCreateClassifiedValid)
            {
                var message = $"\r\nCreate Classified with success: '{createClassifiedResults.Model.Success}' and Guid: '{createClassifiedResults.Model.Guid}'\r\n"
                    .RequestInfo(client, request1)
                    .WithValidationErrors(createClassifiedResults.Results);

                allErrorMessages.Add(message);
            }

            //Read Classified
            var request2 = new RestRequest("/objects/{guid}", Method.GET);

            request2
                .AddUrlSegment("guid", response1.Data.Guid)
                .AddParameter("digest", digest)
                .AddHeader("Authorization", auth);

            var response2 = client.Execute<ReadClassifiedModel.RootObject>(request2);

            if (response2.StatusCode != HttpStatusCode.OK || response2.Data == null || response2.Data.Success == false)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request2), response2.StatusCode, response2.Content));
            }

                        ValidationResultModel<ReadClassifiedModel.RootObject> classifiedDataMainResults;
            var isClassifiedDataValid = GlobalLogic.IsModelValid(response2.Data, out classifiedDataMainResults);
            if (!isClassifiedDataValid)
            {
                var message = $"Classified with success: {classifiedDataMainResults.Model.Success} and results: {classifiedDataMainResults.Model.Results}."
                    .RequestInfo(client, request2)
                    .WithValidationErrors(classifiedDataMainResults.Results);

                allErrorMessages.Add(message);
            }

            //Create Lead
            var request3 = new RestRequest("/lead/create?digest={digest}", Method.POST);

            request3 
                .AddHeader("Authorization", auth)
                .AddUrlSegment("digest", digest)
                .AddJsonBody(new{

                    classified_id = response2.Data.Items.classified_id,
                    customer_email = email,
                    customer_name = dealerName,
                    customer_phone = phone,
                    dealer_id = user_id
                    
            });

            var response3 = client.Execute<LeadModel.RootObject>(request3);

            EnsureOkResponseStatusCode(response3, client, request3);

            ValidationResultModel<LeadModel.RootObject> createLeadResults;
            var isCreateLeadDataValid = GlobalLogic.IsModelValid(response3.Data, out createLeadResults);

            if (!isCreateLeadDataValid)
            {
                var message = $"\r\nCreate Lead with success: '{createLeadResults.Model.Success}' and Lead id: '{createLeadResults.Model.LeadId}'\r\n"
                    .RequestInfo(client, request3)
                    .WithValidationErrors(createLeadResults.Results);

                allErrorMessages.Add(message);
            }


            //Create Cross-Share Lead
            var request4 = new RestRequest("/lead/cross-share?digest={digest}", Method.POST);

            request4
                .AddHeader("Authorization", auth)
                .AddUrlSegment("digest", digest)
                .AddJsonBody(new
                {

                    lead_id = response3.Data.LeadId

                });

            var response4 = client.Execute<RegisterRequestModel>(request4);

            EnsureOkResponseStatusCode(response4, client, request4);

            ValidationResultModel<RegisterRequestModel> createCShLeadResults;
            var isCreateCShLeadDataValid = GlobalLogic.IsModelValid(response4.Data, out createCShLeadResults);

            if (!isCreateCShLeadDataValid)
            {
                var message = $"\r\nCreate Cross-Share Lead with success: '{createCShLeadResults.Model.Success}\r\n"
                    .RequestInfo(client, request4)
                    .WithValidationErrors(createCShLeadResults.Results);

                allErrorMessages.Add(message);
            }

            //Update Classified
            var request5 = new RestRequest("/objects/{guid}", Method.PUT);
            request5.RequestFormat = DataFormat.Json;
            request5
                .AddUrlSegment("guid", response1.Data.Guid)
                .AddParameter("digest", digest)
                .AddHeader("Authorization", auth)
                .AddJsonBody(
                new
                {
                    data =
                    new
                    {
                        status = 3
                    }
                });

            var response5 = client.Execute<RegisterRequestModel>(request5);

            EnsureOkResponseStatusCode(response5, client, request5);

            ValidationResultModel<RegisterRequestModel> updateClassifiedResults;
            var isupdateClassifiedsDataValid = GlobalLogic.IsModelValid(response5.Data, out updateClassifiedResults);

            if (!isupdateClassifiedsDataValid)
            {
                var message = $"\r\nUpdate Classified with success: '{updateClassifiedResults.Model.Success}\r\n"
                    .RequestInfo(client, request5)
                    .WithValidationErrors(updateClassifiedResults.Results);

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
*/