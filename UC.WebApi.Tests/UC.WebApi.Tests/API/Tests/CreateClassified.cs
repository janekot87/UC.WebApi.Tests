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
                .AddJsonBody(new {user_id = "Jane", year = "2000", city = "noida", model = "astonmartin-db9", price = "666666"});

                
                
                
                //.AddJsonBody(new { user_id = "Jane" })
                //.AddJsonBody(new { year = "2000" })
                //.AddJsonBody(new { city = "noida" })
                //.AddJsonBody(new { model = "astonmartin-db9" })
                //.AddJsonBody(new { variant_id = "stonmartin-db9-coupe1" })
                //.AddJsonBody(new { km_driven = "22000" })
                //.AddJsonBody(new { owners = "1" })
                //.AddJsonBody(new { color = "white" })
                //.AddJsonBody(new { fuel_type = "Petrol" })
                //.AddJsonBody(new { transmission = "Manual" })
                //.AddJsonBody(new { engine = "3333" })
                //.AddJsonBody(new { body_type = "Sedan" })
                //.AddJsonBody(new { have_certificated = "no" })
                //.AddJsonBody(new { price = "222222" })
                //.AddJsonBody(new { classified_phone = "+91-9999999999" })
                //.AddJsonBody(new { address = "khgkh" })
                //.AddJsonBody(new { status = 1 })
                //.AddJsonBody(new { images = Data.image })
                //.AddJsonBody(new { brand = "astonmartin" })
                //.AddJsonBody(new { source = "Cabinet" });

                
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
