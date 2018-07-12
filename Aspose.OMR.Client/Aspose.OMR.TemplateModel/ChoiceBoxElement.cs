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
    /// Represents ChoiceBox question
    /// </summary>
    [DataContract(Name = "ChoiceBox", Namespace = "")]
    public class ChoiceBoxElement : OmrElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiceBoxElement"/> class
        /// </summary>
        public ChoiceBoxElement()
        {
            this.Bubbles = new List<OmrBubble>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether multiple selection is allowed
        /// </summary>
        [DataMember(Name = "Multiselect")]
        public bool MultipleSelectionAllowed { get; set; }

        [DataMember(Name = "BubbleWidth")]
        public double BubbleWidth { get; set; }

        [DataMember(Name = "BubbleHeight")]
        public double BubbleHeight { get; set; }

        public Orientations Orientation { get; set; }

        /// <summary>
        /// Gets string representation of orientation property
        /// </summary>
        [DataMember(Name = "Orientation")]
        public string OrientationString
        {
            get { return this.Orientation.ToString(); }
            private set { }
        }

        /// <summary>
        /// Gets or sets a value indicating whether bubbles aligned horizontally
        /// </summary>
        [DataMember(Name = "AlignedHorizontally")]
        public bool IsAlignedHorizontal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether bubbles aligned vertically
        /// </summary>
        [DataMember(Name = "AlignedVertically")]
        public bool IsAlignedVertical { get; set; }

        /// <summary>
        /// Gets or sets bubbles collection
        /// </summary>
        [DataMember(Name = "Bubbles")]
        public List<OmrBubble> Bubbles { get; set; }

        /// <summary>
        /// Adds bubble inside question
        /// </summary>
        /// <param name="name">Bubble name</param>
        /// <param name="width">Bubble width</param>
        /// <param name="height">Bubble height</param>
        /// <param name="top">Bubble top position</param>
        /// <param name="left">Bubble left position</param>
        /// <param name="isValid">Bubble valid flag</param>
        public void AddBubble(string name, int width, int height, int top, int left, bool isValid)
        {
            OmrBubble newBubble = new OmrBubble
            {
                Value = name,
                Top = top,
                Left = left,
                IsValid = isValid
            };

            this.Bubbles.Add(newBubble);

            this.BubbleWidth = width;
            this.BubbleHeight = height;
        }
    }
}
