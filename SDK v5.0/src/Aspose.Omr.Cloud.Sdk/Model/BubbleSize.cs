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
    /// Defines BubbleSize
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
        public enum BubbleSize
    {
        /// <summary>
        /// Enum Extrasmall for value: Extrasmall
        /// </summary>
        [EnumMember(Value = "Extrasmall")]
        Extrasmall = 10,
        /// <summary>
        /// Enum Small for value: Small
        /// </summary>
        [EnumMember(Value = "Small")]
        Small = 20,
        /// <summary>
        /// Enum Normal for value: Normal
        /// </summary>
        [EnumMember(Value = "Normal")]
        Normal = 30,
        /// <summary>
        /// Enum Large for value: Large
        /// </summary>
        [EnumMember(Value = "Large")]
        Large = 40,
        /// <summary>
        /// Enum Extralarge for value: Extralarge
        /// </summary>
        [EnumMember(Value = "Extralarge")]
        Extralarge = 50    }
}
