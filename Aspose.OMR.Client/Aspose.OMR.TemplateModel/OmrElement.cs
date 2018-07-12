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
    /// Abstract class representing base omr question 
    /// Contains common properties for all questions
    /// </summary>
    [DataContract]
    [KnownType(typeof(ChoiceBoxElement))]
    [KnownType(typeof(GridElement))]
    [KnownType(typeof(BarcodeElement))]
    public abstract class OmrElement
    {
        /// <summary>
        /// Gets or sets question name
        /// </summary>
        [DataMember(Order = 1, Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets question width
        /// </summary>
        [DataMember(Order = 2, Name = "Width")]
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets question height
        /// </summary>
        [DataMember(Order = 3, Name = "Height")]
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets question top position 
        /// </summary>
        [DataMember(Order = 4, Name = "Top")]
        public double Top { get; set; }

        /// <summary>
        /// Gets or sets question left position
        /// </summary>
        [DataMember(Order = 5, Name = "Left")]
        public double Left { get; set; }
    }
}
