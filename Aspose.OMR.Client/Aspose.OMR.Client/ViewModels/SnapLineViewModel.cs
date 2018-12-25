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
    using System;
    using System.Windows.Shapes;

    /// <summary>
    /// View model for the dynamic snap lines
    /// </summary>
    public class SnapLineViewModel : BaseQuestionViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SnapLineViewModel"/> class.
        /// </summary>
        /// <param name="line">Line to draw</param>
        public SnapLineViewModel(Line line)
        {
            this.X1 = line.X1;
            this.X2 = line.X2;
            this.Y1 = line.Y1;
            this.Y2 = line.Y2;
        }

        /// <summary>
        /// Gets or sets the X1 position of the line
        /// </summary>
        public double X1 { get; set; }

        /// <summary>
        /// Gets or sets the X2 position of the line
        /// </summary>
        public double X2 { get; set; }

        /// <summary>
        /// Gets or sets the Y1 position of the line
        /// </summary>
        public double Y1 { get; set; }

        /// <summary>
        /// Gets or sets the Y2 position of the line
        /// </summary>
        public double Y2 { get; set; }

        /// <summary>
        /// Indicates whether element is selected, not used
        /// </summary>
        public override bool IsSelected { get; set; }

        /// <summary>
        /// Indicates whether element is valid, not used
        /// </summary>
        public override bool IsValid { get; }

        /// <summary>
        /// Creates copy of element, not used
        /// </summary>
        public override BaseQuestionViewModel CreateCopy()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shrinks element, not used
        /// </summary>
        public override void Shrink()
        {
            throw new NotImplementedException();
        }
    }
}
