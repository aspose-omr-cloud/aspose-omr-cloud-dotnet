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
namespace Aspose.OMR.TemplateModel
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents single omr bubble
    /// </summary>
    [DataContract]
    public class OmrBubble
    {
        /// <summary>
        /// Gets or sets bubble answer value, i.e. mapping value
        /// </summary>
        [DataMember(Order = 1, Name = "Value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets bubble absolute top position
        /// </summary>
        [DataMember(Order = 2, Name = "Top")]
        public double Top { get; set; }

        /// <summary>
        /// Gets or sets bubble absolute left position
        /// </summary>
        [DataMember(Order = 3, Name = "Left")]
        public double Left { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether bubble is in valid position
        /// </summary>
        [DataMember(Order = 4, Name = "IsValid")]
        public bool IsValid { get; set; }
    }
}
