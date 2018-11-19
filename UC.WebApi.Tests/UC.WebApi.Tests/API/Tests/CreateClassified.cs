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
    public class CreateClassified
    {      
        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth)]
        public void CreateClassifiedTest(string digest, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/objects?digest={digest}", Method.POST);
            
            request.RequestFormat = DataFormat.Json;
            request
                .AddHeader("Authorization", auth)
                .AddUrlSegment("digest", digest)
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

            EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<CreateClassifiedModel> createClassifiedResults;
            var isCreateClassifiedValid = GlobalLogic.IsModelValid(response.Data, out createClassifiedResults);

            if (!isCreateClassifiedValid)
            {
                var message = $"\r\nCreate Classified with success: '{createClassifiedResults.Model.Success}' and Guid: '{createClassifiedResults.Model.Guid}'\r\n"
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
