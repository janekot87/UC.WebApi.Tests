using RestSharp;

namespace UC.WebApi.Tests.API.CreativeMedia
{
    public class UCRestRequest
    {
        public  const string baseUrl = "/index.php/api/v2/";
        
        public RestRequest UCRestRequestMake(string entrypoint, Method method)
        {
            return new RestRequest(baseUrl + entrypoint, method);
        }
        
    }
}