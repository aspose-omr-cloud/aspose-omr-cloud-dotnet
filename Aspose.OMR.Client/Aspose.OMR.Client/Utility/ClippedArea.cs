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
namespace Aspose.OMR.Client.Utility
{
    using ViewModels;

    /// <summary>
    /// Represents clipped area info for single question
    /// </summary>
    public class ClippedArea
    {
        /// <summary>
        /// Gets or sets the name of the item
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// Gets or sets the image data
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClippedArea"/> class
        /// </summary>
        /// <param name="name">Name of the resulting clipped area</param>
        /// <param name="imageData">Image data</param>
        public ClippedArea(string name, byte[] imageData)
        {
            this.AreaName = name;
            this.ImageData = imageData;

            PreviewClipAreaCommand = new RelayCommand(x => OnPreviewClipArea());

        }

        /// <summary>
        /// Gets the command for displaying clip area preview
        /// </summary>
        public RelayCommand PreviewClipAreaCommand { get; private set; }

        /// <summary>
        /// Initiate clip area preview vm and display image
        /// </summary>
        private void OnPreviewClipArea()
        {
            ClipAreaPreviewViewModel vm = new ClipAreaPreviewViewModel(this.ImageData, this.AreaName);
        }
    }
}
