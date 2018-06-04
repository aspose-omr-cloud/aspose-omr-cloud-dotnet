/*
 * Copyright (c) 2018 Aspose Pty Ltd. All Rights Reserved.
 *
 * Licensed under the MIT (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://github.com/aspose-omr-cloud/aspose-omr-cloud-dotnet/blob/master/LICENSE
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Com.Aspose.Omr.Model;
using Com.Aspose.Omr.Client;

namespace Com.Aspose.Omr
{
    public interface IAuthenticator
    {
        void Authenticate(RestClient client, RestRequest request);
    }

    public class RestClient
    {
        private Dictionary<String, String> defaultHeaderMap = new Dictionary<String, String>();
        
        public IAuthenticator Authenticator { get; set; }

        public string BaseUrl { set; get; }
        public RestClient(String baseUrl)
        {
            this.BaseUrl = baseUrl;
        }

        public void addDefaultHeader(string key, string value)
        {
            if (!defaultHeaderMap.ContainsKey(key))
            {
                defaultHeaderMap.Add(key, value);
            }
        }

        public string escapeString(string str)
        {
            return Uri.EscapeDataString(str);
        }

        public static object deserialize(string json, Type type)
        {
            try
            {
                if (json.StartsWith("{") || json.StartsWith("["))
                    return JsonConvert.DeserializeObject(json, type);
                else
                {
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.LoadXml(json);
                    return JsonConvert.SerializeXmlNode(xmlDoc);
                }

            }
            catch (IOException e)
            {
                throw new ApiException(500, e.Message);
            }
            catch (JsonSerializationException jse)
            {
                throw new ApiException(500, jse.Message);
            }
            catch (System.Xml.XmlException xmle)
            {
                throw new ApiException(500, xmle.Message);
            }
        }

        public static object deserialize(byte[] BinaryData, Type type)
        {
            try
            {
                return null;
                //return new ResponseMessage(BinaryData, 200, "Ok");
            }
            catch (IOException e)
            {
                throw new ApiException(500, e.Message);
            }

        }

        public static string serialize(object obj)
        {
            try
            {
                return obj != null
                    ? JsonConvert.SerializeObject(obj, Formatting.Indented,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore})
                    : null;
            }
            catch (Exception e)
            {
                throw new ApiException(500, e.Message);
            }
        }
/*
        public string invokeAPI(string host, string path, string method, Dictionary<String, String> queryParams,
            object body, Dictionary<String, String> headerParams, Dictionary<String, object> formParams)
        {
            return invokeAPIInternal(host, path, method, false, queryParams, body, headerParams, formParams) as string;
        }

        public byte[] invokeBinaryAPI(string host, string path, string method, Dictionary<String, String> queryParams,
            object body, Dictionary<String, String> headerParams, Dictionary<String, object> formParams)
        {
            return invokeAPIInternal(host, path, method, true, queryParams, body, headerParams, formParams) as byte[];
        }
*/
        public RestResponse Execute(RestRequest request)
        {
            string path = request.Path;

            path = Regex.Replace(path, @"{.+?}", "");

            List<string> queryParams = new List<string>();

            if (Authenticator != null)
            {
                Authenticator.Authenticate(this, request);
            }

            byte[] requestBody = null;
            Parameter bodyParameter = null;
            List<object> formParameters = new List<object>();
            foreach (var param in request.Parameters)
                if (param.Type == ParameterType.QueryString || (param.Type == ParameterType.GetOrPost && request.Method != "POST"))
                    queryParams.Add(string.Format("{0}={1}", Uri.EscapeDataString(param.Name), Uri.EscapeDataString(param.Value.ToString())));
                else if (param.Type == ParameterType.GetOrPost)
                    formParameters.Add(param);
                else if (param.Type == ParameterType.RequestBody && bodyParameter == null)
                    bodyParameter = param;

            foreach (var param in request.FileParameters)
            {
                formParameters.Add(param);
            }

            // Need to add / to the basepath if not present, and remove / from path if present to be able
            // Uri recognize relative paths.
            Uri uri = new Uri(new Uri(BaseUrl + (BaseUrl.EndsWith("/") ? "" : "/")), path.StartsWith("/") ? path.TrimStart('/') : path);
            if (queryParams.Count > 0)
            {
                var builder = new UriBuilder(uri);
                builder.Query = string.Join("&", queryParams.ToArray());
                uri = builder.Uri;
            }

            var client = (HttpWebRequest)WebRequest.Create(uri);
            client.Method = request.Method;

            foreach (var headerParam in request.Headers)
            {
                if (0 == string.Compare(headerParam.Key, "accept", true))
                    client.Accept = headerParam.Value;
                else if (0 == string.Compare(headerParam.Key, "content-type", true))
                    client.ContentType = headerParam.Value;
                else
                    client.Headers.Add(headerParam.Key, headerParam.Value);
            }
            if (bodyParameter != null)
            {
                requestBody = Encoding.UTF8.GetBytes(bodyParameter.Value.ToString());
                client.ContentType = bodyParameter.Name;
            } else if (0 == string.Compare(client.ContentType, "application/x-www-form-urlencoded"))
            {
                requestBody = UrlEncodedData(formParameters);
                Trace.WriteLine(Encoding.UTF8.GetString(requestBody));
            } else
            if (formParameters.Count > 0)
            {
                string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
                client.ContentType = "multipart/form-data; boundary=" + formDataBoundary;
                requestBody = MultipartFormData(formParameters, formDataBoundary);
            }
            if (client.Accept == null)
                client.Accept = "application/json";

            switch (request.Method)
            {
                case "GET":
                    break;
                case "POST":
                case "PUT":
                case "DELETE":
                
                    using (Stream requestStream = client.GetRequestStream())
                    {
                        if (requestBody != null)
                        {
                            requestStream.Write(requestBody, 0, requestBody.Length);
                        }
                    }
                    break;
                default:
                    throw new ApiException(500, "unknown method type " + request.Method);
            }

            RestResponse response = new RestResponse();
            try
            {
                var webResponse = (HttpWebResponse)client.GetResponse();
                response.StatusCode = webResponse.StatusCode;
                if (webResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    webResponse.Close();
                    throw new ApiException((int)webResponse.StatusCode, webResponse.StatusDescription);
                }
                using (var memstream = new MemoryStream())
                {
                    CopyTo(webResponse.GetResponseStream(), memstream);
                    response.RawBytes = memstream.ToArray();
                    response.Content = Encoding.UTF8.GetString(response.RawBytes);
                }
                foreach (var header in webResponse.Headers)
                    response.Headers.Add(new Parameter {Name = header.ToString(), Value = webResponse.Headers[header.ToString()] });
            }
            catch (WebException ex)
            {
                var webResponse = ex.Response as HttpWebResponse;
                response.StatusCode = 0;
                if (webResponse != null)
                {
                    response.StatusCode = webResponse.StatusCode;
                    webResponse.Close();
                }
                throw new ApiException((int)response.StatusCode, ex.Message);
            }
            return response;
        }

        private static byte[] UrlEncodedData(List<object> postParameters)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            var builder = new StringBuilder();


            foreach (var param in postParameters)
            {
                if (param is Parameter)
                {
                    var paramInfo = param as Parameter;
                    if (builder.Length > 1)
                       builder.Append("&");
                    builder.AppendFormat("{0}={1}", Uri.EscapeDataString(paramInfo.Name), Uri.EscapeDataString(paramInfo.Value.ToString()));
                }
            }
            formDataStream.Write(Encoding.UTF8.GetBytes(builder.ToString()), 0, Encoding.UTF8.GetByteCount(builder.ToString()));
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

        private static byte[] MultipartFormData(List<object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsCLRF)
                    formDataStream.Write(Encoding.UTF8.GetBytes("\r\n"), 0, Encoding.UTF8.GetByteCount("\r\n"));

                needsCLRF = true;
                if (param is FileParameter)
                {
                    var fileInfo = param as FileParameter;

                    string postData = string.Format(
                        "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n",
                        boundary,
                        fileInfo.Name,
                        fileInfo.ContentType);
                    formDataStream.Write(Encoding.UTF8.GetBytes(postData), 0, Encoding.UTF8.GetByteCount(postData));

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    fileInfo.Writer(formDataStream);
                }
                else if (param is Parameter)
                {
                    var paramInfo = param as Parameter;
                    string postData = string.Format(
                        "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        paramInfo.Name,
                        paramInfo.Value.ToString());
                    formDataStream.Write(Encoding.UTF8.GetBytes(postData), 0, Encoding.UTF8.GetByteCount(postData));
                }
            }
            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(Encoding.UTF8.GetBytes(footer), 0, Encoding.UTF8.GetByteCount(footer));
            

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }
        public static void CopyTo(Stream source, Stream destination, int bufferSize = 10 * 1024)
        {
            int num;
            byte[] buffer = new byte[bufferSize];
            while ((num = source.Read(buffer, 0, buffer.Length)) != 0)
                destination.Write(buffer, 0, num);
        }

        /**
         * Overloaded method for returning the path value
         * For a string value an empty value is returned if the value is null
         * @param value
         * @return
         */
        public String ToPathValue(String value)
        {
            return (value == null) ? "" : value;
        }

        public String ToPathValue(int value)
        {
            return value.ToString();
        }

        public String ToPathValue(int? value)
        {
            return value.ToString();
        }

        public String ToPathValue(float value)
        {
            return value.ToString();
        }

        public String ToPathValue(float? value)
        {
            return value.ToString();
        }

        public String ToPathValue(long value)
        {
            return value.ToString();
        }

        public String ToPathValue(long? value)
        {
            return value.ToString();
        }

        public String ToPathValue(bool value)
        {
            return value.ToString();
        }

        public String ToPathValue(bool? value)
        {
            return value.ToString();
        }

        public String ToPathValue(double value)
        {
            return value.ToString();
        }

        public String ToPathValue(double? value)
        {
            return value.ToString();
        }

        //public String ToPathValue(Com.Aspose.OCR.Model.DateTime value)
        //{
        //    //SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd");
        //    //return format.format(value);
        //    return value.ToString();
        //}


    }


    public enum ParameterType
    {
        Header,
        GetOrPost,
        UrlSegment,
        RequestBody,
        QueryString
    }

    public class Parameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public ParameterType Type { get; set; }
        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Value);
        }

    }
    public class FileParameter
    {
        public static FileParameter Create(string name, byte[] data, string filename, string contentType)
        {
			var length = data.LongLength;
            return new FileParameter
            {
                Writer = s => s.Write(data, 0, data.Length),
                FileName = filename,
                ContentType = contentType,
                ContentLength = length,
                Name = name
            };
        }
        public long ContentLength { get; set; }
        public Action<Stream> Writer { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Name { get; set; }
    }

    public class RestRequest
    {

        public string Path { get; set; }
        public string Method { get; set; }
        public Dictionary<string, string> Headers = new Dictionary<string, string>();
        public List<Parameter> Parameters = new List<Parameter>();
        public List<FileParameter> FileParameters = new List<FileParameter>();

        public RestRequest(string path, string method)
        {
            Path = path;
            Method = method;
        }

        public RestRequest AddHeader(string header, string value)
        {
            Headers.Add(header, value);
            return this;
        }

        public RestRequest AddParameter(string name, object value, ParameterType type = ParameterType.GetOrPost)
        {
            Parameters.Add(new Parameter {Name = name, Value = value, Type = type});
            return this;
        }

        public RestRequest AddFile(string name, Action<Stream> writer, string fileName, string contentType)
        {
            FileParameters.Add(new FileParameter
            {
                Name = name,
                Writer = writer,
                FileName = fileName,
                ContentType = contentType
            });
            return this;
        }
    }

    public class RestResponse
    {
        public System.Net.HttpStatusCode StatusCode;
        public string ErrorMessage;
        public string Content;
        public byte[] RawBytes;
        public List<Parameter> Headers = new List<Parameter>();
    }
}
