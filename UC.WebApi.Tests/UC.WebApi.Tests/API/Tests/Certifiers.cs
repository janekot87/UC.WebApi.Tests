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

            EnsureOkResponseStatusCode(response, client, request);

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
                foreach (var CertifiersItemResult in CertifiersItemResults.Where(x => x.Results.Any()))
                {
                    var message = $"Certifier item with Name: {CertifiersItemResult.Model.Name}"
                    .RequestInfo(client, request)
                    .WithValidationErrors(CertifiersItemResult.Results);

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
