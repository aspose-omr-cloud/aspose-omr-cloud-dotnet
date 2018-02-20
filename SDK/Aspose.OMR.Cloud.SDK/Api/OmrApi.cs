/*
 * Copyright (c) 2018 Aspose Pty Ltd. All Rights Reserved.
 *
 * Licensed under the MIT (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *       https://github.com/asposecloud/Aspose.OMR-Cloud/blob/master/LICENSE
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Aspose.OMR.Cloud.SDK.Model;

namespace Aspose.OMR.Cloud.SDK.Api
{
    public class OmrApi
    {
        string basePath;
        private readonly ApiInvoker apiInvoker = ApiInvoker.GetInstance();

        public OmrApi(String apiKey, String appSid, String basePath)
        {
            this.apiInvoker.apiKey = apiKey;
            this.apiInvoker.appSid = appSid;
            this.basePath = basePath;
        }

        public ApiInvoker getInvoker()
        {
            return this.apiInvoker;
        }

        // Sets the endpoint base url for the services being accessed
        public void setBasePath(string basePath)
        {
            this.basePath = basePath;
        }

        // Gets the endpoint base url for the services being accessed
        public String getBasePath()
        {
            return this.basePath;
        }

        /// <summary>
        /// PostRunOmrTask
        /// </summary>
        /// <param name="name"></param>
        /// <param name="actionName"></param>
        /// <param name="functionParams"></param>
        /// <param name="additionalParams"></param>
        /// <param name="storage"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public OMRResponse PostRunOmrTask(string name, string actionName, OMRFunctionParam functionParam,
            string storage, string folder)
        {
            // create path and map variables
            var ResourcePath =
                "/omr/{name}/runOmrTask/?appSid={appSid}&amp;actionName={actionName}&amp;storage={storage}&amp;folder={folder}"
                    .Replace("{format}", "json");
            ResourcePath = Regex.Replace(ResourcePath, "\\*", "")
                .Replace("&amp;", "&")
                .Replace("/?", "?")
                .Replace("toFormat={toFormat}", "format={format}");

            // query params
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, object>();

            // verify required params are set
            if (name == null)
            {
                throw new ApiException(400, "missing required params");
            }
            if (name == null)
            {
                ResourcePath = Regex.Replace(ResourcePath, @"([&?])name=", "");
            }
            else
            {
                ResourcePath = ResourcePath.Replace("{" + "name" + "}", this.apiInvoker.ToPathValue(name));
            }
            if (actionName == null)
            {
                ResourcePath = Regex.Replace(ResourcePath, @"([&?])actionName=", "");
            }
            else
            {
                ResourcePath = ResourcePath.Replace("{" + "actionName" + "}", this.apiInvoker.ToPathValue(actionName));
            }
            if (storage == null)
            {
                ResourcePath = Regex.Replace(ResourcePath, @"([&?])storage=", "");
            }
            else
            {
                ResourcePath = ResourcePath.Replace("{" + "storage" + "}", this.apiInvoker.ToPathValue(storage));
            }
            if (folder == null)
            {
                ResourcePath = Regex.Replace(ResourcePath, @"([&?])folder=", "");
            }
            else
            {
                ResourcePath = ResourcePath.Replace("{" + "folder" + "}", this.apiInvoker.ToPathValue(folder));
            }
            try
            {
                if (typeof(OMRResponse) == typeof(ResponseMessage))
                {
                    var response = this.apiInvoker.invokeBinaryAPI(this.basePath, ResourcePath, "POST", queryParams,
                        functionParam, headerParams, formParams);
                    return (OMRResponse) ApiInvoker.deserialize(response, typeof(OMRResponse));
                }
                else
                {
                    var response = this.apiInvoker.invokeAPI(this.basePath, ResourcePath, "POST", queryParams, functionParam,
                        headerParams, formParams);
                    if (response != null)
                    {
                        return (OMRResponse) ApiInvoker.deserialize(response, typeof(OMRResponse));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (ApiException ex)
            {
                if (ex.ErrorCode == 404)
                {
                    return null;
                }
                else
                {
                    throw ex;
                }
            }
        }

    }
}
