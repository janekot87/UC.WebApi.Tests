using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UC.WebApi.Tests.API.CreativeMedia;
using UC.WebApi.Tests.API.DataStore;
using UC.WebApi.Tests.API.Logic;
using UC.WebApi.Tests.API.Models;
using Xunit;

namespace UC.WebApi.Tests.API.Tests
{
    public class CreateClassified
    {
        public const string ENTRYPOINT = "objects";
      
        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth)]
        public void CreateClassifiedTest(string digest, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/objects?digest=gLqVqvqHTxZk9RHxwtPjkbWtNAbdBpGh", Method.POST);
            
            //var request = new UCRestRequest(ENTRYPOINT, Method.POST);

            request.RequestFormat = DataFormat.Json;
            request
                .AddHeader("Authorization", auth)
                .AddJsonBody(
                new {
                    data = 
                    new {
                        user_id = "Jane",
                        year = "2000",
                        city = "noida",
                        model = 
                        new {
                            model = "astonmartin-db9",
                            brand = "astonmartin"
                        },
                        price = "777777",
                        variant_id  = "astonmartin-db9-coupe",
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

                
                
               
                
            var response = client.Execute<CreateClassifiedModel>(request);

            if (response.StatusCode != HttpStatusCode.OK || response.Data == null)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request), response.StatusCode));
            }

            //if (!response.Data.Success)
            //{
            //    var response1 = client.Execute<ErrorModel.RootObject>(request);
            //    throw new Exception(AssertMessages.InvalidDealerNameErrorMessage(response1.Data.Description, response1.Data.Error_code, client.BuildUri(request)));
            //}

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<CreateClassifiedModel> createClassifiedResults;
            var isCreateClassifiedValid = GlobalLogic.IsModelValid(response.Data, out createClassifiedResults);

            if (!isCreateClassifiedValid)
            {
                var message = $"Create Classified with success: {createClassifiedResults.Model.Success} and guid: {createClassifiedResults.Model.Guid}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(createClassifiedResults.Results);

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
