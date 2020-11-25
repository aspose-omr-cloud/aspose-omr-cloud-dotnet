using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Aspose.Omr.Cloud.Sdk.RequestHandlers
{
    internal class JwtRequestHandler : IRequestHandler
    {
        private readonly Configuration configuration;
        private readonly ApiInvoker apiInvoker;

        private string accessToken;

        public JwtRequestHandler(Configuration configuration)
        {
            this.configuration = configuration;

            var requestHandlers = new List<IRequestHandler>();
            requestHandlers.Add(new DebugLogRequestHandler(this.configuration));
            requestHandlers.Add(new ApiExceptionRequestHandler());
            this.apiInvoker = new ApiInvoker(requestHandlers);
        }

        public string ProcessUrl(string url)
        {
            if (string.IsNullOrEmpty(this.accessToken))
                RequestToken();
            return url;
        }

        public void BeforeSend(WebRequest request, Stream streamToSend)
        {
            request.Headers.Add("Authorization", "Bearer " + this.accessToken);
        }

        public void ProcessResponse(HttpWebResponse response, Stream resultStream)
        {
        }

        private void RequestToken()
        {
            var requestUrl = this.configuration.ApiBaseUrl + "/connect/token";

            var postData = "grant_type=client_credentials";
            postData += "&client_id=" + this.configuration.AppSid;
            postData += "&client_secret=" + this.configuration.AppKey;

            var responseString = this.apiInvoker.InvokeApi(
                requestUrl,
                "POST",
                postData,
                contentType: "application/x-www-form-urlencoded");

            var result = (GetAccessTokenResult)SerializationHelper.Deserialize(responseString, typeof(GetAccessTokenResult));

            this.accessToken = result.AccessToken;
        }

        private class GetAccessTokenResult
        {
            [JsonProperty(PropertyName = "access_token")]
            public string AccessToken { get; set; }
        }
    }
}
