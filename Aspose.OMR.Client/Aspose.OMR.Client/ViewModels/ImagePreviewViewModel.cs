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
    using System.Collections.ObjectModel;
    using Utility;

    /// <summary>
    /// View model for image preview within results view
    /// </summary>
    public class ImagePreviewViewModel : ViewModelBase
    {
        /// <summary>
        /// Flag indicating that image is processing by recognition engine
        /// </summary>
        private bool isProcessing;

        /// <summary>
        /// Recognition results for this image
        /// </summary>
        private ObservableCollection<RecognitionResult> recognitionResults;

        /// <summary>
        /// Indicates whether this image was already processed by engine
        /// </summary>
        private bool isProcessed;

        /// <summary>
        /// Recognition status text desribing current recognition stage for the image
        /// </summary>
        private string statusText;

        /// <summary>
        /// Indicates whether recognition for this image can be cancelled
        /// </summary>
        private bool canCancel;

        /// <summary>
        /// Clipped areas for this image
        /// </summary>
        private ObservableCollection<ClippedArea> clippedAreas;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagePreviewViewModel"/> class
        /// </summary>
        /// <param name="pathToImage">Path to image</param>
        /// <param name="title">Image title</param>
        /// <param name="size">Image size in bytes</param>
        public ImagePreviewViewModel(string pathToImage, string title, long size)
        {
            this.PathToImage = pathToImage;
            this.Title = title;

            this.ImageSizeInBytes = size;

            this.WasUploaded = false;
            this.IsProcessing = false;
            this.IsProcessed = false;
            this.CanCancel = true;
        }

        /// <summary>
        /// Gets or sets the image size in bytes
        /// </summary>
        public long ImageSizeInBytes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether image was already uploaded on cloud
        /// </summary>
        public bool WasUploaded { get; set; }

        /// <summary>
        /// Gets or sets the status text describing recognition stage of this item
        /// </summary>
        public string StatusText
        {
            get { return this.statusText; }
            set
            {
                this.statusText = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether recognition can be cancelled for this item
        /// </summary>
        public bool CanCancel
        {
            get { return this.canCancel; }
            set
            {
                this.canCancel = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this item was recognized
        /// </summary>
        public bool IsProcessed
        {
            get { return this.isProcessed; }
            set
            {
                this.isProcessed = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets recognition results
        /// </summary>
        public ObservableCollection<RecognitionResult> RecognitionResults
        {
            get { return this.recognitionResults; }
            set
            {
                this.recognitionResults = value;
                if (value != null)
                {
                    this.IsProcessed = true;
                }

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets clipped areas
        /// </summary>
        public ObservableCollection<ClippedArea> ClippedAreas
        {
            get { return this.clippedAreas; }
            set
            {
                this.clippedAreas = value;
                if (value != null)
                {
                    this.IsProcessed = true;
                }

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets image title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets file path to the image
        /// </summary>
        public string PathToImage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether item is in recognition process
        /// </summary>
        public bool IsProcessing
        {
            get { return this.isProcessing; }
            set
            {
                this.isProcessing = value;
                this.OnPropertyChanged();
            }
        }

        public string ImageFileFormat { get; set; }
    }
}
