﻿using RestSharp;
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
    public class Brands
    {
        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth)]
        public void BrandsTest(string digest, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/brands?digest={digest}", Method.GET);

            request
                .AddUrlSegment("digest", digest)
                .AddHeader("Authorization", auth);

            var response = client.Execute<BrandsModel.RootObject>(request);

            EnsureOkResponseStatusCode(response, client, request);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<BrandsModel.RootObject> brandsMainResults;
            var isBrandsDataValid = GlobalLogic.IsModelValid(response.Data, out brandsMainResults);

            IList<ValidationResultModel<BrandsModel.Item>> BrandsItemResults;
            var areBrandsDataItemsValid = GlobalLogic.IsModelArrayValid(response.Data.Items, out BrandsItemResults);

            if (!isBrandsDataValid)
            {
                var message = $"Brands with success: {brandsMainResults.Model.Success} and results: {brandsMainResults.Model.Results}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(brandsMainResults.Results);

                allErrorMessages.Add(message);
            }

            if (!areBrandsDataItemsValid)
            {
                foreach (var BrandsItemResult in BrandsItemResults.Where(x => x.Results.Any()))
                {
                    var message = $"\r\nBrands item with Name: '{BrandsItemResult.Model.Name}'\r\n"
                    .RequestInfo(client, request)
                    .WithValidationErrors(BrandsItemResult.Results);

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




        
    