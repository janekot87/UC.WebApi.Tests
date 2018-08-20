using UC.WebApi.Tests.API.Infrastructure;

namespace UC.WebApi.Tests.API.Logic
{
    public static class TestConfiguration
    {
        public static class API
        {
            public static string Location = "http://dev-usedcars-api.autoportal.com/index.php/api/v2";
        }

        public static string Environment => TestParameters.Environment ?? "DEV";
    }
}
