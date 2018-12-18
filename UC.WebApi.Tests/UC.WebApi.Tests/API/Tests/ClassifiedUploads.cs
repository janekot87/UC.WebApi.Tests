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
    public class ClassifiedUploads
    {
        [Theory]
        [InlineData(Data.Digest, Data.BasicAuth, Data.Email, Data.DealerName, Data.Phone, Data.User_id)]
        public void ClassifiedUploadsTest(string digest, string auth, string email, string dealerName, string phone, int user_id)
        {
            var client = new RestClient(TestConfiguration.API.Location);
            var request1 = new RestRequest("/objects?digest={digest}", Method.POST);

            //Create Classified
            request1.RequestFormat = DataFormat.Json;
            request1
                .AddHeader("Authorization", auth)
                .AddUrlSegment("digest", digest)
                .AddJsonBody(
                new
                {
                    data =
                    new
                    {
                        user_id = dealerName,
                        year = "2000",
                        city = "noida",
                        model =
                        new
                        {
                            model = "astonmartin-db9",
                            brand = "astonmartin"
                        },
                        price = "777777",
                        variant_id = "astonmartin-db9-coupe",
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

            var response1 = client.Execute<CreateClassifiedModel>(request1);

            EnsureOkResponseStatusCode(response1, client, request1);

            List<string> allErrorMessages = new List<string>();

            ValidationResultModel<CreateClassifiedModel> createClassifiedResults;
            var isCreateClassifiedValid = GlobalLogic.IsModelValid(response1.Data, out createClassifiedResults);

            if (!isCreateClassifiedValid)
            {
                var message = $"\r\nCreate Classified with success: '{createClassifiedResults.Model.Success}' and Guid: '{createClassifiedResults.Model.Guid}'\r\n"
                    .RequestInfo(client, request1)
                    .WithValidationErrors(createClassifiedResults.Results);

                allErrorMessages.Add(message);
            }

            //Upload images
            foreach (string fileType in Data.Filetypes)
            {

                var request2 = new RestRequest("/file?digest={digest}", Method.POST);

                request2.RequestFormat = DataFormat.Json;
                request2
                    .AddHeader("Authorization", auth)
                    .AddUrlSegment("digest", digest)
                    .AddJsonBody(
                        new
                        {

                            username = Data.DealerName,
                            guid = response1.Data.Guid,
                            file =
                                new
                                {
                                    type = fileType,
                                    name = Data.ImageName,
                                    content = Data.FileContent
                                }
                        });

                var response2 = client.Execute<UploadImageModel>(request2);

                EnsureOkResponseStatusCode(response2, client, request2);


                ValidationResultModel<UploadImageModel> UploadImageResults;
                var isUploadImageValid = GlobalLogic.IsModelValid(response2.Data, out UploadImageResults);

                if (!isUploadImageValid)
                {
                    var message = $"\r\nUploaded photo with success: '{UploadImageResults.Model.Success}' and Url: '{UploadImageResults.Model.Url}'\r\n"
                        .RequestInfo(client, request2)
                        .WithValidationErrors(UploadImageResults.Results);

                    allErrorMessages.Add(message);
                }
                if (allErrorMessages.Any())
                {
                    var allMessages = string.Join("\r\n\r\n", allErrorMessages);
                    throw new Exception(allMessages);
                }

                
            }
            //Upload Certificate
            var request3 = new RestRequest("/file?digest={digest}", Method.POST);

            request3.RequestFormat = DataFormat.Json;
            request3
                .AddHeader("Authorization", auth)
                .AddUrlSegment("digest", digest)
                .AddJsonBody(
                new
                {

                    username = Data.DealerName,
                    guid = response1.Data.Guid,
                    file =
                        new
                        {
                            type = Data.FileTypePdf,
                            name = Data.PdfName,
                            content = Data.FileContentPdf
                        }
                });

            var response3 = client.Execute<UploadPdfModel>(request3);

            EnsureOkResponseStatusCode(response3, client, request3);
                       

            ValidationResultModel<UploadPdfModel> UploadPdfResults;
            var isUploadPdfValid = GlobalLogic.IsModelValid(response3.Data, out UploadPdfResults);

            if (!isUploadPdfValid)
            {
                var message = $"\r\nUploaded photo with success: '{UploadPdfResults.Model.Success}' and Url: '{UploadPdfResults.Model.Url}'\r\n"
                    .RequestInfo(client, request3)
                    .WithValidationErrors(UploadPdfResults.Results);

                allErrorMessages.Add(message);
            }

            //Update Classified
            var request4 = new RestRequest("/objects/{guid}", Method.PUT);
            request4.RequestFormat = DataFormat.Json;
            request4
                .AddUrlSegment("guid", response1.Data.Guid)
                .AddParameter("digest", digest)
                .AddHeader("Authorization", auth)
                .AddJsonBody(
                new
                {
                    data =
                    new
                    {
                        status = 3
                    }
                });

            var response4 = client.Execute<RegisterRequestModel>(request4);

            EnsureOkResponseStatusCode(response4, client, request4);

            ValidationResultModel<RegisterRequestModel> updateClassifiedResults;
            var isupdateClassifiedsDataValid = GlobalLogic.IsModelValid(response4.Data, out updateClassifiedResults);

            if (!isupdateClassifiedsDataValid)
            {
                var message = $"\r\nUpdate Classified with success: '{updateClassifiedResults.Model.Success}\r\n"
                    .RequestInfo(client, request4)
                    .WithValidationErrors(updateClassifiedResults.Results);

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
