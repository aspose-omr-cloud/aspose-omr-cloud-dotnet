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
namespace Aspose.OMR.Client.ViewModels
{
    /// <summary>
    /// View model logic for bubble
    /// </summary>
    public class BubbleViewModel : ViewModelBase
    {
        /// <summary>
        /// Default font size for displayed bubble Name
        /// </summary>
        private readonly int defaultFontSize = 12;

        /// <summary>
        /// Indicates whether bubble is selected
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Bubble width
        /// </summary>
        private double width;

        /// <summary>
        /// Bubble height
        /// </summary>
        private double height;

        /// <summary>
        /// Bubble top position on parent canvas
        /// </summary>
        private double top;

        /// <summary>
        /// Bubble left position on parent canvas
        /// </summary>
        private double left;

        /// <summary>
        /// Bubble display name
        /// </summary>
        private string name;

        /// <summary>
        /// Indicates whether bubble position state considered valid by Core
        /// </summary>
        private bool isValid;

        /// <summary>
        /// The font size of displayed bubble name
        /// </summary>
        private int fontSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="BubbleViewModel"/> class.
        /// </summary>
        /// <param name="width">Bubble width</param>
        /// <param name="height">Bubble height</param>
        /// <param name="top">Bubble top position</param>
        /// <param name="left">Bubble left position</param>
        public BubbleViewModel(double width, double height, double top, double left)
        {
            this.width = width;
            this.height = height;
            this.top = top;
            this.left = left;

            this.isValid = true;

            this.fontSize = (int)(this.defaultFontSize / TemplateViewModel.ZoomKoefficient);
        }

        /// <summary>
        /// Gets or sets bubble name
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether item is selected
        /// </summary>
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets element width
        /// </summary>
        public double Width
        {
            get { return this.width; }
            set
            {
                this.width = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets element Height
        /// </summary>
        public double Height
        {
            get { return this.height; }
            set
            {
                this.height = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets element top position in canvas
        /// </summary>
        public double Top
        {
            get { return this.top; }
            set
            {
                this.top = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets element left position in canvas
        /// </summary>
        public double Left
        {
            get { return this.left; }
            set
            {
                this.left = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether bubble position is valid
        /// </summary>
        public bool IsValid
        {
            get { return this.isValid; }
            set
            {
                this.isValid = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the font size of displayed bubble name
        /// </summary>
        public int FontSize
        {
            get { return this.fontSize; }
            set
            {
                this.fontSize = value;
                this.OnPropertyChanged();
            }
        }
    }
}
