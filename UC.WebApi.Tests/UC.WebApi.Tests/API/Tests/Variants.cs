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
    public class Variants
    {
        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth)]
        public void VariantsTest(string digest, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/variants?digest={digest}", Method.GET);

            request
                .AddUrlSegment("digest", digest)
                .AddHeader("Authorization", auth);

            var response = client.Execute<VariantsModel.RootObject>(request);

            if (response.StatusCode != HttpStatusCode.OK || response.Data == null || response.Data.Success == false)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request), response.StatusCode, response.Data.Success));
            }

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<VariantsModel.RootObject> variantsMainResults;
            var isVariantsDataValid = GlobalLogic.IsModelValid(response.Data, out variantsMainResults);

            IList<ValidationResultModel<VariantsModel.Item>> VariantsItemResults;
            var areVariantsDataItemsValid = GlobalLogic.IsModelArrayValid(response.Data.Items, out VariantsItemResults);

            if (!isVariantsDataValid)
            {
                var message = $"Variants with success: {variantsMainResults.Model.Success} and results: {variantsMainResults.Model.Results}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(variantsMainResults.Results);

                allErrorMessages.Add(message);
            }

            if (!areVariantsDataItemsValid)
            {
                foreach (var VariantsItemResult in VariantsItemResults.Where(x => x.Results.Any()))
                {
                    var message = $"\r\nVariants item with Name: '{VariantsItemResult.Model.Name}'\r\n"
                    .RequestInfo(client, request)
                    .WithValidationErrors(VariantsItemResult.Results);

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
    
