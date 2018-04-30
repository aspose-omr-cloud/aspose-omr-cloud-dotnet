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
namespace Aspose.OMR.TemplateModel
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents finalization data
    /// </summary>
    [DataContract]
    public class FinalizationData
    {
        /// <summary>
        /// Gets or sets warnings recieved from OMR Core
        /// </summary>
        [DataMember(Name = "Warnings")]
        public string[] Warnings { get; set; }

        /// <summary>
        /// Gets or sets recognition result for template image
        /// </summary>
        [DataMember(Name = "RecognitionResult")]
        public string[] AnswerText { get; set; }

        public string Answers { get; set; }
    }
}
