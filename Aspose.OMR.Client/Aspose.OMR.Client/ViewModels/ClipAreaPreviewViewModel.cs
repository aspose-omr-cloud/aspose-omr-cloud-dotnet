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
    using System.IO;
    using System.Windows.Media.Imaging;
    using Utility;
    using Views;

    /// <summary>
    /// View model for clip area image preview window
    /// </summary>
    public class ClipAreaPreviewViewModel : ViewModelBase
    {
        private BitmapImage bitmapImage;
        private ClipAreaView view;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipAreaPreviewViewModel"/> class
        /// </summary>
        /// <param name="imageData">Clipped area image data</param>
        /// <param name="imageName">Clipped area element name</param>
        public ClipAreaPreviewViewModel(byte[] imageData, string imageName)
        {
            this.ImageBytes = imageData;
            this.ImageName = imageName;
            this.ClipAreaImage = CreateImageFromBytes();

            this.SaveCommand = new RelayCommand(x => this.SaveImage());
            this.CloseCommand = new RelayCommand(x => this.CloseView());

            this.view = new ClipAreaView(this);
            this.view.ShowDialog();
        }

        /// <summary>
        /// Gets the save command
        /// </summary>
        public RelayCommand SaveCommand { get; private set; }

        /// <summary>
        /// Gets the cancel command
        /// </summary>
        public RelayCommand CloseCommand { get; private set; }

        /// <summary>
        /// Gets or sets the clipped area image name
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// Gets or sets the clipped area image bytes
        /// </summary>
        public byte[] ImageBytes { get; set; }

        /// <summary>
        /// Gets or sets the clip area bitmap image 
        /// </summary>
        public BitmapImage ClipAreaImage
        {
            get
            {
                return bitmapImage;
            }
            set
            {
                bitmapImage = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Save image by specified path
        /// </summary>
        private void SaveImage()
        {
            string savePath = DialogManager.ShowSaveClipAreaDialog(this.ImageName);
            if (savePath == null)
            {
                return;
            }

            File.WriteAllBytes(savePath, ImageBytes);
        }

        /// <summary>
        /// Creates bitmap image for UI from image bytes
        /// </summary>
        /// <returns></returns>
        private BitmapImage CreateImageFromBytes()
        {
            if (ImageBytes == null || ImageBytes.Length == 0)
            {
                return null;
            }

            var image = new BitmapImage();
            using (var mem = new MemoryStream(ImageBytes))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }

            image.Freeze();
            return image;
        }

        /// <summary>
        /// Closes view
        /// </summary>
        private void CloseView()
        {
            this.view.Close();
        }
    }
}
