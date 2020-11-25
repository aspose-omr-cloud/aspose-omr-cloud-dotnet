// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Aspose" file="OmrApi.cs">
//   Copyright (c) 2020 Aspose.Omr for Cloud
// </copyright>
// <summary>
//   Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Aspose.Omr.Cloud.Sdk
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Aspose.Omr.Cloud.Sdk.Model;
    using Aspose.Omr.Cloud.Sdk.Model.Requests;
    using Aspose.Omr.Cloud.Sdk.RequestHandlers;

    /// <summary>
    /// Aspose.Omr for Cloud API.
    /// </summary>
    public class OmrApi
    {        
        private readonly ApiInvoker apiInvoker;
        private readonly Configuration configuration;
        private string appKey;
        private string appSid;
        private string basepath;

        /// <summary>
        /// Initializes a new instance of the <see cref="OmrApi"/> class.
        /// </summary>
        /// <param name="apiKey">
        /// The api Key.
        /// </param>
        /// <param name="appSid">
        /// The app Sid.
        /// </param>
        public OmrApi(string apiKey, string appSid)
            : this(new Configuration { AppKey = apiKey, AppSid = appSid })
        {
        }

        public OmrApi(string jwtToken)
            : this(new Configuration { JwtToken = jwtToken, ApiVersion = ApiVersion.V3, AuthType = AuthType.JWT })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OmrApi"/> class.
        /// </summary>    
        /// <param name="configuration">Configuration settings</param>
        public OmrApi(Configuration configuration)
        {
            this.configuration = configuration;
            
            var requestHandlers = new List<IRequestHandler>();
            switch (this.configuration.AuthType)
            {
                case AuthType.JWT:
                    requestHandlers.Add(new JwtRequestHandler(this.configuration));
                    break;
                default:
                    throw new ApiException(1000, "Authorization method is not supported for OMR API. USE AuthType.JWT");
            }

            requestHandlers.Add(new DebugLogRequestHandler(this.configuration));
            requestHandlers.Add(new ApiExceptionRequestHandler());
            this.apiInvoker = new ApiInvoker(requestHandlers);
        }


        /// <summary>
        /// Run specific OMR task  
        /// </summary>
        /// <param name="request">Request. <see cref="PostRunOmrTaskRequest" /></param> 
        /// <returns><see cref="OmrResponse"/></returns>            
        public OmrResponse PostRunOmrTask(PostRunOmrTaskRequest request)
        {
            // verify the required parameter 'name' is set
            if (request.name == null) 
            {
                throw new ApiException(400, "Missing required parameter 'name' when calling PostRunOmrTask");
            }

            // verify the required parameter 'actionName' is set
            if (request.actionName == null) 
            {
                throw new ApiException(400, "Missing required parameter 'actionName' when calling PostRunOmrTask");
            }

            // verify the required parameter 'param' is set
            if (request.param == null) 
            {
                throw new ApiException(400, "Missing required parameter 'param' when calling PostRunOmrTask");
            }

            // create path and map variables
            var resourcePath = this.configuration.GetApiRootUrl() + "/omr/{name}/runOmrTask";
            resourcePath = Regex
                        .Replace(resourcePath, "\\*", string.Empty)
                        .Replace("&amp;", "&")
                        .Replace("/?", "?");
            resourcePath = UrlHelper.AddPathParameter(resourcePath, "name", request.name);
            resourcePath = UrlHelper.AddQueryParameterToUrl(resourcePath, "actionName", request.actionName);
            resourcePath = UrlHelper.AddQueryParameterToUrl(resourcePath, "storage", request.storage);
            resourcePath = UrlHelper.AddQueryParameterToUrl(resourcePath, "folder", request.folder);
            var postBody = SerializationHelper.Serialize(request.param); // http body (model) parameter
            try 
            {                               
                var response = this.apiInvoker.InvokeApi(
                    resourcePath, 
                    "POST", 
                    postBody, 
                    null, 
                    null);
                if (response != null)
                {
                    return (OmrResponse)SerializationHelper.Deserialize(response, typeof(OmrResponse));
                }
                    
                return null;
            } 
            catch (ApiException ex) 
            {
                if (ex.ErrorCode == 404) 
                {
                    return null;
                }
                
                throw;                
            }
        }
    }
}