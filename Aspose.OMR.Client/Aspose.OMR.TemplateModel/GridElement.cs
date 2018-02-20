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
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract(Name = "Grid", Namespace = "")]
    public class GridElement : OmrElement
    {
        public Orientations Orientation { get; set; }

        [DataMember(Name = "Orientation")]
        public string OrientationString
        {
            get { return this.Orientation.ToString(); }
            private set { }
        }

        [DataMember(Name = "Elements")]
        public List<OmrElement> ChoiceBoxes { get; set; }

        public GridElement()
        {
            this.ChoiceBoxes = new List<OmrElement>();
        }

        public ChoiceBoxElement AddChoiceBox(string name, int width, int height, int top, int left)
        {
            ChoiceBoxElement newElement = new ChoiceBoxElement
            {
                Name = name,
                Width = width,
                Height = height,
                Top = top,
                Left = left
            };

            this.ChoiceBoxes.Add(newElement);
            return newElement;
        }
    }
}
