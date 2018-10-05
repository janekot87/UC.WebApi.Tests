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
    public class Certifiers
    {
        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth)]
        public void CertifiersTest(string digest, string auth)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/certifiers?digest={digest}", Method.GET);

            request
                .AddUrlSegment("digest", digest)
                .AddHeader("Authorization", auth);

            var response = client.Execute<CertifiersModel.RootObject>(request);


            if (response.StatusCode != HttpStatusCode.OK || response.Data == null || response.Data.Success == false)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request), response.StatusCode, response.Data.Success));
            }

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<CertifiersModel.RootObject> certifiersMainResults;
            var isCertifiersDataValid = GlobalLogic.IsModelValid(response.Data, out certifiersMainResults);

            IList<ValidationResultModel<CertifiersModel.Item>> CertifiersItemResults;
            var areCertifiersDataItemsValid = GlobalLogic.IsModelArrayValid(response.Data.Items, out CertifiersItemResults);

            if (!isCertifiersDataValid)
            {
                var message = $"Certifiers with success: {certifiersMainResults.Model.Success} and results: {certifiersMainResults.Model.Results}."
                    .RequestInfo(client, request)
                    .WithValidationErrors(certifiersMainResults.Results);

                allErrorMessages.Add(message);
            }

            if (!areCertifiersDataItemsValid)
            {
                var message = $"Certifiers items: {certifiersMainResults.Model.Items}"
                .RequestInfo(client, request)
                .WithValidationErrors(certifiersMainResults.Results);

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
