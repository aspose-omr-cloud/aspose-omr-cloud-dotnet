/*
 * Copyright (c) 2017 Aspose Pty Ltd. All Rights Reserved.
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
namespace Aspose.OMR.Client.Utility
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;

    /// <summary>
    /// Image preprocessing configuration
    /// </summary>
    [DataContract(Name = "PreprocessingConfiguration")]
    public class PreprocessingConfiguration
    {
        [DataMember]
        public bool IsPreprocessingEnabled { get; set; }

        [DataMember]
        public PreprocessingPreset SelectedPreset { get; set; }

        [DataMember]
        public int JpegCompressionLevel { get; set; }

        [DataMember]
        public int DesiredWidth { get; set; }

        [DataMember]
        public int DesiredHeight { get; set; }

        [DataMember]
        public int ExcludeImagesSize { get; set; }

        /// <summary>
        /// Serialize configuration
        /// </summary>
        /// <param name="config">Config</param>
        /// <returns>Json string</returns>
        public static string Serialize(PreprocessingConfiguration config)
        {
            string result;
            using (MemoryStream jsonStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PreprocessingConfiguration));

                serializer.WriteObject(jsonStream, config);
                result = Encoding.UTF8.GetString(jsonStream.ToArray());
                return result;
            }
        }

        /// <summary>
        /// Deserialize configuration
        /// </summary>
        /// <param name="jsonConfig">Json string</param>
        /// <returns>Deserialized config</returns>
        public static PreprocessingConfiguration Deserialize(string jsonConfig)
        {
            PreprocessingConfiguration config;
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonConfig)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(PreprocessingConfiguration));
                config = ser.ReadObject(ms) as PreprocessingConfiguration;
            }

            return config;
        }
    }
}
