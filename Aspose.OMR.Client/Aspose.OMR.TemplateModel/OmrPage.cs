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
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents single omr page
    /// </summary>
    [DataContract]
    public class OmrPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OmrPage"/> class
        /// </summary>
        public OmrPage()
        {
            this.Elements = new List<OmrElement>();
        }

        /// <summary>
        /// Gets or sets page width
        /// </summary>
        [DataMember(Order = 1, Name = "Width")]
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets page height
        /// </summary>
        [DataMember(Order = 2, Name = "Height")]
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets list of elements on page
        /// </summary>
        [DataMember(Order = 3, Name = "Elements")]
        public List<OmrElement> Elements { get; set; }

        /// <summary>
        /// Gets or sets the image data
        /// </summary>
        [DataMember(Order = 4, Name = "ImageData")]
        public string ImageData { get; set; }

        /// <summary>
        /// Gets or sets the image data
        /// </summary>
        [DataMember(Order = 5, Name = "ImageName")]
        public string ImageName { get; set; }

        /// <summary>
        /// Gets or sets the image file format
        /// </summary>
        [DataMember(Order = 5, Name = "ImageFormat")]
        public string ImageFormat { get; set; }

        /// <summary>
        /// Add choice box element on page
        /// </summary>
        /// <param name="name">Element name</param>
        /// <param name="width">Element width</param>
        /// <param name="height">Element height</param>
        /// <param name="top">Element top position</param>
        /// <param name="left">Element left position</param>
        /// <returns>Created choice box</returns>
        public ChoiceBoxElement AddChoiceBoxElement(string name, int width, int height, int top, int left)
        {
            ChoiceBoxElement newElement = new ChoiceBoxElement
            {
                Name = name,
                Width = width,
                Height = height,
                Top = top,
                Left = left
            };

            this.Elements.Add(newElement);

            return newElement;
        }

        public GridElement AddGridElement(string name, int width, int height, int top, int left)
        {
            GridElement newElement = new GridElement
            {
                Name = name,
                Width = width,
                Height = height,
                Top = top,
                Left = left
            };

            this.Elements.Add(newElement);

            return newElement;
        }
    }
}
