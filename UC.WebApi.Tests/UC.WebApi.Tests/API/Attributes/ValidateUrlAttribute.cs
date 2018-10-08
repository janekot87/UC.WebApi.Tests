using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace UC.WebApi.Tests.API.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]

    public class ValidateUrlAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {

            var requestUrl = value as string ?? throw new InvalidOperationException();

            var httpClientHandler = new HttpClientHandler { AllowAutoRedirect = true };

            var client = new HttpClient(httpClientHandler);

            try
            {
                var response = client.SendAsync(new HttpRequestMessage(HttpMethod.Head, requestUrl)).Result;
                var statusCode = (int)response.StatusCode;
                var contentType = response.Content.Headers.ContentType.MediaType;

                if (statusCode == 200 && (contentType.Equals("image/jpeg") || contentType.Equals("image/gif")))
                {
                    return true;
                }

                ErrorMessage = $"Status code: {statusCode}\r\nContent type: {contentType}";
                return false;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
                return false;
            }
        }
    }
}