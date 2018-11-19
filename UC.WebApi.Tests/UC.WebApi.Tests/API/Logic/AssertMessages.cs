using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using RestSharp;

namespace UC.WebApi.Tests.API.Logic
{
    public static class AssertMessages
    {
        public static string StatusCodeErrorMessage(Uri requestUrl, HttpStatusCode statusCode, string content) => $"\r\nFailed to send request: {requestUrl}\r\nHttpResponseStatusCode: {statusCode}\r\nResponse Content: {content}";
        public static string InvalidDealerNameErrorMessage(string description, int errorCode, Uri requestUrl) => $"Response success is False! Error description: '{description}' Error code: '{errorCode}' for requestUrl: {requestUrl}";
        public static string InvalidDigestErrorMessage(bool success, string message, Uri requestUrl) => $"Response success is False! Success: '{success}' Message: '{message}' for requestUrl: {requestUrl}";

        public static string RequestInfo(this string errorMessage, IRestClient client, IRestRequest request)
        {
            return $"{errorMessage}Request: {client.BuildUri(request)}";
        }

        public static string WithValidationErrors(this string errorMessage, IEnumerable<ValidationResult> validationResults)
        {
            var validationErrors = validationResults.Select(x => x.ErrorMessage);
            return $"{errorMessage}\r\n{string.Join("\r\n", validationErrors)}";
        }
    }
}
