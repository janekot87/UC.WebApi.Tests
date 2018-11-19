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
    public class UploadCertificate
    {
        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth, Data.Guid, Data.FileTypePdf)]
       
        public void UploadCertificateTest(string digest, string auth, string guid, string fileType)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/file?digest={digest}", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request
                .AddHeader("Authorization", auth)
                .AddUrlSegment("digest", digest)
                .AddJsonBody(
                new
                {

                    username = Data.DealerName,
                    guid = guid,
                    file =
                        new
                        {
                            type = fileType,
                            name = Data.PdfName,
                            content = Data.FileContentPdf
                        }
                });

            var response = client.Execute<UploadPdfModel>(request);

            EnsureOkResponseStatusCode(response, client, request);


            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<UploadPdfModel> UploadPdfResults;
            var isUploadPdfValid = GlobalLogic.IsModelValid(response.Data, out UploadPdfResults);

            if (!isUploadPdfValid)
            {
                var message = $"\r\nUploaded photo with success: '{UploadPdfResults.Model.Success}' and Url: '{UploadPdfResults.Model.Url}'\r\n"
                    .RequestInfo(client, request)
                    .WithValidationErrors(UploadPdfResults.Results);

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
