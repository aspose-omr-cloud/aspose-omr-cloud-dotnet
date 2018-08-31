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
namespace Aspose.OMR.Client.ViewModels
{
    using System.Windows;

    /// <summary>
    /// View model class for Clip Area element
    /// </summary>
    public class ClipAreaViewModel : BaseQuestionViewModel
    {
        /// <summary>
        /// Default value for <see cref="JpegQuality"/> property
        /// </summary>
        private const int DefaultJpegQuiality = 85;

        /// <summary>
        /// Indicates that element is selected
        /// </summary>
        private bool isSelected;
        
        /// <summary>
        /// Jpeg compression quiality
        /// </summary>
        private int jpegQuality;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipAreaViewModel"/> class
        /// </summary>
        /// <param name="name">Name of the clip area</param>
        /// <param name="area">Area of the element</param>
        /// <param name="templateViewModel">The view model of parent template</param>
        public ClipAreaViewModel(string name, Rect area, TemplateViewModel templateViewModel)
        {
            this.InitializeValues(name, area.Top, area.Left, area.Width, area.Height);
            this.ParentTemplate = templateViewModel;
        }

        /// <summary>
        /// Gets or sets the jpeg compression quiality of clipped area image
        /// </summary>
        public int JpegQuality
        {
            get
            {
                return jpegQuality;
            }
            set
            {
                if (value > 0 && value <= 100)
                {
                    jpegQuality = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether item is selected
        /// </summary>
        public override bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets value indicating whether question is valid (unused)
        /// </summary>
        public override bool IsValid
        {
            get { return true; }
        }

        /// <summary>
        /// Initializes question values
        /// </summary>
        /// <param name="name">Question name</param>
        /// <param name="top">Top position on parent canvas</param>
        /// <param name="left">Left position on parent canvas</param>
        /// <param name="width">Question's width</param>
        /// <param name="height">Question's height</param>
        private void InitializeValues(string name, double top, double left, double width, double height)
        {
            this.Name = name;
            this.Top = top;
            this.Left = left;
            this.Width = width;
            this.Height = height;
            this.JpegQuality = DefaultJpegQuiality;
        }

        public override BaseQuestionViewModel CreateCopy()
        {
            ClipAreaViewModel copy = new ClipAreaViewModel(this.Name, new Rect(this.Left, this.Top, this.Width, this.Height), this.ParentTemplate);

            return copy;
        }

        /// <summary>
        /// Shrinks element (unused in clip area)
        /// </summary>
        public override void Shrink()
        {
            return;
        }
    }
}
