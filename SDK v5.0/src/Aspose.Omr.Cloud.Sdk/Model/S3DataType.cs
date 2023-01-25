/*
 * Copyright (C) 2023 Aspose Pty Ltd. All Rights Reserved.
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


/* 
 * Aspose OMR Cloud V5 API
 *
 * Aspose OMR Cloud V5 API
 *
 * OpenAPI spec version: v5
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Aspose.Omr.Cloud.Sdk.Model
{
    /// <summary>
    /// Defines S3DataType
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
        public enum S3DataType
    {
        /// <summary>
        /// Enum Png for value: Png
        /// </summary>
        [EnumMember(Value = "Png")]
        Png = 1,
        /// <summary>
        /// Enum Pdf for value: Pdf
        /// </summary>
        [EnumMember(Value = "Pdf")]
        Pdf = 2,
        /// <summary>
        /// Enum Csv for value: Csv
        /// </summary>
        [EnumMember(Value = "Csv")]
        Csv = 3,
        /// <summary>
        /// Enum Json for value: Json
        /// </summary>
        [EnumMember(Value = "Json")]
        Json = 4,
        /// <summary>
        /// Enum Txt for value: Txt
        /// </summary>
        [EnumMember(Value = "Txt")]
        Txt = 5,
        /// <summary>
        /// Enum Internal for value: Internal
        /// </summary>
        [EnumMember(Value = "Internal")]
        Internal = 6    }
}