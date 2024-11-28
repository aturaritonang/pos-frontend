using Microsoft.Extensions.Configuration;

namespace XsisPos.Web.Helpers
{
    public class HttpClientHelper
    {
        private readonly IConfiguration _configuration;
        private string _baseUrl;

        public HttpClientHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _baseUrl = _configuration.GetValue<string>("BackEndUrl");
        }

        public HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"{_baseUrl.ToString()}/");
            //dynamic _token = HttpContext.Current.Session["token"];
            //if (_token == null) throw new ArgumentNullException(nameof(_token));
            //MyHttpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", _token.AccessToken));
            return httpClient;
        }
    }
}
