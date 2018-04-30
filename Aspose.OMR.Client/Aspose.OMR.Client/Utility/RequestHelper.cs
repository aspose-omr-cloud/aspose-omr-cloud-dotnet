/*
 * Copyright (c) 2018 Aspose Pty Ltd. All Rights Reserved.
 *
 * Licensed under the MIT (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *       https://github.com/aspose-omr-cloud/aspose-omr-cloud-dotnet/blob/master/LICENSE
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace Aspose.OMR.Client.Utility
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;

    /// <summary>
    /// Helper class for forming requests
    /// </summary>
    public class RequestHelper
    {
        /// <summary>
        /// Serializes CorrectionParameter into json
        /// </summary>
        /// <param name="file">Data to serialize</param>
        /// <returns>Serialized data as byte array</returns>
        internal static byte[] SerializeFilesToJson(CorrectionParameter file)
        {
            using (MemoryStream jsonStream = new MemoryStream())
            {
                var settings = new DataContractJsonSerializerSettings();
                settings.EmitTypeInformation = EmitTypeInformation.Never;
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CorrectionParameter), settings);
                serializer.WriteObject(jsonStream, file);
                byte[] json = jsonStream.ToArray();

                return json;
            }
        }
    }

    /// <summary>
    /// Class representing parameters for Correct Template function
    /// </summary>
    [DataContract]
    public class CorrectionParameter
    {
        [DataMember(Name = "Files")]
        public OmrFile[] Files { get; set; }
    }

    /// <summary>
    /// Class representing OMR file
    /// </summary>
    [DataContract]
    public class OmrFile
    {
        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Size")]
        public int Size { get; set; }

        [DataMember(Name = "Data")]
        public string Data { get; set; }
    }
}
