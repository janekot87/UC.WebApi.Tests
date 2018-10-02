using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace KnowledgeBase.WebApi.AutomatedTests.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]

    public class ValidateUrlAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            var requestUrl = value as string ?? throw new InvalidOperationException();

            var httpClientHandler = new HttpClientHandler { AllowAutoRedirect = false };

            var client = new HttpClient(httpClientHandler);

            try
            {
                var response = client.SendAsync(new HttpRequestMessage(HttpMethod.Head, requestUrl)).Result;
                var statusCode = (int)response.StatusCode;

                if (statusCode >= 200 && statusCode <= 300)
                {
                    return true;
                }

                ErrorMessage = $"Status code: {statusCode}";
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