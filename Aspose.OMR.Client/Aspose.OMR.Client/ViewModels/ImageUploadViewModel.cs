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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Controls;
    using Utility;
    using Views;

    public class ImageUploadViewModel : ViewModelBase
    {
        /// <summary>
        /// Max file size in kb that causes no warning for the user
        /// </summary>
        private readonly int maxNoWarningUploadSizeKb = 1024;

        /// <summary>
        /// Template generator window
        /// </summary>
        private readonly ImageUploadView view;

        /// <summary>
        /// Cloud storage path where to look for images used in generation
        /// </summary>
        private string extraStoragePath;

        /// <summary>
        /// Collection of images to upload
        /// </summary>
        private ObservableCollection<FileToUpload> imagesToUpload = new ObservableCollection<FileToUpload>();

        /// <summary>
        /// Images uploaded delegate
        /// </summary>
        /// <param name="imagesFolder">Folder where images were uploaded</param>
        public delegate void ImagesUploadedDelegate(string imagesFolder);

        /// <summary>
        /// Fired when images upload view is closed
        /// </summary>
        public event ImagesUploadedDelegate ViewClosed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageUploadViewModel"/> class
        /// </summary>
        /// <param name="templateGeneratorView">Parent view</param>
        public ImageUploadViewModel(TemplateGeneratorView templateGeneratorView)
        {
            // init view
            this.view = new ImageUploadView(this, templateGeneratorView);

            this.SelectImagesCommand = new RelayCommand(x => this.OnSelectImages());
            this.UploadImagesCommand = new RelayCommand(x => this.OnUploadImages());
            this.CloseCommand = new RelayCommand(x => this.OnCloseCommand());

            this.view.ShowDialog();
        }

        public RelayCommand SelectImagesCommand { get; private set; }
        public RelayCommand UploadImagesCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }

        /// <summary>
        /// Gets or sets the collection of files to upload
        /// </summary>
        public ObservableCollection<FileToUpload> ImagesToUpload
        {
            get { return this.imagesToUpload; }
            set
            {
                this.imagesToUpload = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the cloud storage path to folder where images are located
        /// </summary>
        public string ExtraStoragePath
        {
            get { return this.extraStoragePath; }
            set
            {
                this.extraStoragePath = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Lets user select images to upload and validates selected items
        /// </summary>
        private void OnSelectImages()
        {
            string[] files = DialogManager.ShowOpenMultiselectDialog();
            if (files == null)
            {
                return;
            }

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                FileInfo fi = new FileInfo(file);

                // check for duplicates
                if (this.ImagesToUpload.Any(x => x.Name.Equals(fileName)))
                {
                    DialogManager.ShowErrorDialog("Found duplicate image name - " + fileName +
                                                  ". Uploaded images should have unique names!");
                    continue;
                }

                // get size in kb
                long sizeInKb = fi.Length / 1024;

                if (sizeInKb >= this.maxNoWarningUploadSizeKb)
                {
                    bool uploadAnyway = DialogManager.ShowConfirmDialog(
                        string.Format(
                            "Are You sure You want to select image \"{0}\" that is {1}KB in size? This image seems to be too large for a logo and will burn your Cloud Storage traffic.",
                            fileName,
                            sizeInKb)
                    );

                    // user said no, skip to next item
                    if (!uploadAnyway)
                    {
                        continue;
                    }
                }

                var item = new FileToUpload(fileName, file, sizeInKb);
                this.ImagesToUpload.Add(item);
            }
        }

        /// <summary>
        /// Uploads selected images on cloud storage
        /// </summary>
        private async void OnUploadImages()
        {
            try
            {
                BusyIndicatorManager.Enable();
                ControlHelper.HideAllChildViews();

                Dictionary<string, byte[]> imagesData = new Dictionary<string, byte[]>();
                string folderPath = string.IsNullOrEmpty(this.ExtraStoragePath) ? string.Empty : this.ExtraStoragePath + @"\";

                foreach (FileToUpload item in this.ImagesToUpload)
                {
                    imagesData.Add(string.Concat(folderPath, item.Name), File.ReadAllBytes(item.FullPath));
                }

                await Task.Run(() => CoreApi.StorageUploadImages(imagesData));

                if (this.ViewClosed != null)
                {
                    this.ViewClosed(this.ExtraStoragePath);
                }

                this.view.Close();
            }
            catch (Exception e)
            {
                DialogManager.ShowErrorDialog(e.Message);
            }
            finally
            {
                BusyIndicatorManager.Disable();
                ControlHelper.RestoreHidderChildViews();
            }
        }

        /// <summary>
        /// Closes view
        /// </summary>
        private void OnCloseCommand()
        {
            this.view.Close();
        }
    }
}
