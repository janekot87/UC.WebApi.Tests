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
    public class UploadImage
    {
        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth, Data.Guid, Data.FileTypeMI)]
        [InlineData(Data.Digest, Data.BasicAuth, Data.Guid, Data.FileTypeNMI)]
        [InlineData(Data.Digest, Data.BasicAuth, Data.Guid, Data.FileTypeUI)]
        [InlineData(Data.Digest, Data.BasicAuth, Data.Guid, Data.FileTypeL)]
        public void UploadMainImageTest (string digest, string auth, string guid, string fileType)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request = new RestRequest("/file?digest={digest}", Method.POST);
            
            request.RequestFormat = DataFormat.Json;
            request
                .AddHeader("Authorization", auth)
                .AddUrlSegment("digest", digest)
                .AddJsonBody(
                new {

                    username = Data.DealerName,
                    guid = guid,
                        file = 
                        new {
                            type = fileType,
                            name = Data.ImageName,
                            content = Data.FileContent
                        }
                        
                   
                });

            var response = client.Execute<UploadImageModel>(request);

            if (response.StatusCode != HttpStatusCode.OK || response.Data == null || response.Data.Success == false)
            {
                throw new Exception(AssertMessages.StatusCodeErrorMessage(client.BuildUri(request), response.StatusCode, response.Data.Success));
            }

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<UploadImageModel> UploadImageResults;
            var isUploadImageValid = GlobalLogic.IsModelValid(response.Data, out UploadImageResults);

            if (!isUploadImageValid)
            {
                var message = $"\r\nUploaded photo with success: '{UploadImageResults.Model.Success}' and Url: '{UploadImageResults.Model.Url}'\r\n"
                    .RequestInfo(client, request)
                    .WithValidationErrors(UploadImageResults.Results);

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
