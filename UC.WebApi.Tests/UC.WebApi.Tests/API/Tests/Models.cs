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
    public class Models
    {
        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth)]
        public void ModelsTest(string digest, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/models?digest={digest}", Method.GET);

            request
                .AddUrlSegment("digest", digest)
                .AddHeader("Authorization", auth);

           
            var response = client.Execute<ModelsScheme.RootObject>(request);

            if (response.StatusCode != HttpStatusCode.OK || response.Data == null || response.Data.Success == false)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request), response.StatusCode, response.Data.Success));
            }

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<ModelsScheme.RootObject> modelsMainResults;
            var isModelsDataValid = GlobalLogic.IsModelValid(response.Data, out modelsMainResults);

            IList<ValidationResultModel<ModelsScheme.Item>> ModelsItemResults;
            var areModelsDataItemsValid = GlobalLogic.IsModelArrayValid(response.Data.Items, out ModelsItemResults);

            if (!isModelsDataValid)
            {
                var message = $"Models with success: {modelsMainResults.Model.Success} and results: {modelsMainResults.Model.Results}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(modelsMainResults.Results);

                allErrorMessages.Add(message);
            }
            if (!areModelsDataItemsValid)
            {
                foreach (var ModelsItemResult in ModelsItemResults.Where(x => x.Results.Any()))
                {
                    var message = $"\r\nModels item with Name: '{ModelsItemResult.Model.Name}'\r\n"
                    .RequestInfo(client, request)
                    .WithValidationErrors(ModelsItemResult.Results);

                    allErrorMessages.Add(message);
                }
            }
            if (allErrorMessages.Any())
            {
                var allMessages = string.Join("\r\n\r\n", allErrorMessages);
                throw new Exception(allMessages);
            }
        }


    }
}
