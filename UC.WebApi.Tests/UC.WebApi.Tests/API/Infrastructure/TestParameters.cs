using System;
using System.Collections.Generic;
using System.Text;

namespace UC.WebApi.Tests.API.Infrastructure
{
    internal static class TestParameters
    {
        private static string GetEnvironmentVariable(string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(variableName));
            }

            return System.Environment.GetEnvironmentVariable(variableName);
        }

        internal static class Api
        {
            public static string Location =>
                GetEnvironmentVariable("UC.Tests.Api.Location");
        }

        public static string Environment =>
            GetEnvironmentVariable("CompatibilityGuide.AutomatedTests.Environment");
    }
}
