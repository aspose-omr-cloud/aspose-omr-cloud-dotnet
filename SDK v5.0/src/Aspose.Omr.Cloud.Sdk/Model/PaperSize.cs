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
    /// Defines PaperSize
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
        public enum PaperSize
    {
        /// <summary>
        /// Enum Letter for value: Letter
        /// </summary>
        [EnumMember(Value = "Letter")]
        Letter = 1,
        /// <summary>
        /// Enum A4 for value: A4
        /// </summary>
        [EnumMember(Value = "A4")]
        A4 = 2,
        /// <summary>
        /// Enum Legal for value: Legal
        /// </summary>
        [EnumMember(Value = "Legal")]
        Legal = 3    }
}
