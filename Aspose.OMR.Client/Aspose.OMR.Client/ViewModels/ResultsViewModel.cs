﻿/*
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
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using Utility;
    using WIA;

    /// <summary>
    /// View model for results view
    /// </summary>
    public class ResultsViewModel : TabViewModel
    {
        /// <summary>
        /// Current results processing workflow stage
        /// </summary>
        private ResultProcessingStages currentStage;

        /// <summary>
        /// Help message containing various useful information displayed at the lower toolbar
        /// </summary>
        private string helpMessage;

        /// <summary>
        /// Id of template which is used for recognition
        /// </summary>
        private readonly string templateId;

        /// <summary>
        /// Image displayed at central area
        /// </summary>
        private BitmapImage mainImage;

        /// <summary>
        /// Visibility of quick access panel at preview window
        /// </summary>
        private Visibility initialPreviewPanelVisibility;

        /// <summary>
        /// Collection of preview images 
        /// </summary>
        private ObservableCollection<ImagePreviewViewModel> previewImages;

        /// <summary>
        /// Selected preview image
        /// </summary>
        private ImagePreviewViewModel selectedPreviewImage;

        /// <summary>
        /// True zoom level without koefficient
        /// </summary>
        private double zoomLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultsViewModel"/> class.
        /// </summary>
        /// <param name="tabName">The tab name string</param>
        /// <param name="templateId">Id of template used for recognition</param>
        /// <param name="isGenerated">Indicates if template was generated</param>
        public ResultsViewModel(string tabName, string templateId, bool isGenerated)
        {
            this.templateId = templateId;
            this.TabName = tabName;

            this.IsGeneratedTemplate = isGenerated;
            this.PreviewImages = new ObservableCollection<ImagePreviewViewModel>();
            this.InitialPreviewPanelVisibility = Visibility.Visible;
            this.PresetShown = false;

            if (PreprocessingConfigurationManager.CheckConfigExists(this.templateId))
            {
                this.SelectedPreprocessingConfiguration =
                    PreprocessingConfiguration.Deserialize(
                        PreprocessingConfigurationManager.GetConfigByKey(this.templateId));
                this.PresetShown = true;
            }

            ZoomKoefficient = 1;
            this.zoomLevel = 1;

            this.InitCommands();

            if (this.PreviewImages.Count == 0)
            {
                this.CurrentStage = ResultProcessingStages.NoImages;
            }
        }

        /// <summary>
        /// Gets or sets the selected image preprocessing configuration
        /// </summary>
        public PreprocessingConfiguration SelectedPreprocessingConfiguration { get; set; }

        /// <summary>
        /// Gets or sets value indicating whether template was generated
        /// </summary>
        public bool IsGeneratedTemplate { get; set; }

        /// <summary>
        /// Gets or sets zoom koefficient for better representation of large images (values from 0 to 1)
        /// </summary>
        public static double ZoomKoefficient { get; set; }

        /// <summary>
        /// Gets current template creation stage
        /// </summary>
        public ResultProcessingStages CurrentStage
        {
            get { return this.currentStage; }
            private set
            {
                this.currentStage = value;

                // update help message
                this.HelpMessage = HelpManager.GetResultsMessageByStage(value);
            }
        }

        /// <summary>
        /// Gets or sets the help message displayed at lower toolbar
        /// </summary>
        public string HelpMessage
        {
            get { return this.helpMessage; }
            set
            {
                this.helpMessage = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of preview images
        /// </summary>
        public ObservableCollection<ImagePreviewViewModel> PreviewImages
        {
            get { return this.previewImages; }
            set
            {
                this.previewImages = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected preview image
        /// </summary>
        public ImagePreviewViewModel SelectedPreviewImage
        {
            get { return this.selectedPreviewImage; }
            set
            {
                this.selectedPreviewImage = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.RecognitionResults));
                this.OnPropertyChanged(nameof(this.ClippedAreas));

                if (value != null)
                {
                    this.UpdateMainImage();
                }
            }
        }

        /// <summary>
        /// Gets or sets main image displayed at central area
        /// </summary>
        public BitmapImage MainImage
        {
            get { return this.mainImage; }
            set
            {
                this.mainImage = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the visibility of quick access panel at preview window
        /// </summary>
        public Visibility InitialPreviewPanelVisibility
        {
            get { return this.initialPreviewPanelVisibility; }
            set
            {
                this.initialPreviewPanelVisibility = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the recognition results for selected image
        /// </summary>
        public ObservableCollection<RecognitionResult> RecognitionResults
        {
            get { return this.SelectedPreviewImage?.RecognitionResults; }
        }

        /// <summary>
        /// Gets the recognition results for selected image
        /// </summary>
        public ObservableCollection<ClippedArea> ClippedAreas
        {
            get { return this.SelectedPreviewImage?.ClippedAreas; }
        }

        /// <summary>
        /// Gets scale used to scale visuals in template view with respect to scaling koefficient
        /// </summary>
        public double PageScale
        {
            get { return this.ZoomLevel * ZoomKoefficient; }
        }

        /// <summary>
        /// Gets the visual string representation of current page scale
        /// </summary>
        public string PageScaleDisplayString
        {
            get { return Math.Round(this.ZoomLevel * 100) + "%"; }
        }

        /// <summary>
        /// Gets or sets true zoom level without koefficient
        /// </summary>
        public double ZoomLevel
        {
            get { return this.zoomLevel; }
            set
            {
                if (this.MainImage != null)
                {
                    this.zoomLevel = value;
                    this.OnPropertyChanged();
                    this.OnPropertyChanged(nameof(this.PageScale));
                    this.OnPropertyChanged(nameof(this.PageScaleDisplayString));
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether presets view has already been shown to the user
        /// </summary>
        public bool PresetShown { get; private set; }

        #region Commands

        public RelayCommand LoadImagesCommand { get; private set; }

        public RelayCommand DropImagesCommand { get; private set; }

        public RelayCommand LoadFolderCommand { get; private set; }

        public RelayCommand DropFolderCommand { get; private set; }

        public RelayCommand RemoveSelectedPreviewCommand { get; private set; }

        public RelayCommand ZoomInCommand { get; private set; }

        public RelayCommand ZoomOutCommand { get; private set; }

        public RelayCommand ZoomOriginalCommand { get; private set; }

        public RelayCommand FitPageWidthCommand { get; private set; }

        public RelayCommand FitPageHeightCommand { get; private set; }

        public RelayCommand RecognizeAllCommand { get; private set; }

        public RelayCommand CancelAllCommand { get; private set; }

        public RelayCommand RecognizeSelectedImageCommand { get; private set; }

        public RelayCommand CancelSelectedCommand { get; private set; }

        public RelayCommand ExportDataCommand { get; private set; }

        public RelayCommand ExportAllCommand { get; private set; }

        public RelayCommand ShowPresetsCommand { get; private set; }

        public RelayCommand ScanImageCommand { get; private set; }

        #endregion

        /// <summary>
        /// Initialize commands
        /// </summary>
        private void InitCommands()
        {
            this.LoadImagesCommand = new RelayCommand(x => this.OnLoadImages());
            this.DropImagesCommand = new RelayCommand(x => this.LoadPreviewImages((string[]) x));

            this.LoadFolderCommand = new RelayCommand(x => this.OnLoadFolder());
            this.DropFolderCommand = new RelayCommand(x => this.LoadImagesFromFolderByPath((string) x));

            base.RemoveElementCommand = new RelayCommand(x => this.OnRemoveSelectedPreview(), x => this.SelectedPreviewImage != null);
            this.RemoveSelectedPreviewCommand = new RelayCommand(x => this.OnRemoveSelectedPreview(), x => this.SelectedPreviewImage != null);

            this.ZoomInCommand = new RelayCommand(x => this.ZoomLevel = Math.Min(this.ZoomLevel + 0.1, 4));
            this.ZoomOutCommand = new RelayCommand(x => this.ZoomLevel = Math.Max(this.ZoomLevel - 0.1, 0.1));
            this.ZoomOriginalCommand = new RelayCommand(x => this.ZoomLevel = 1);
            this.FitPageWidthCommand = new RelayCommand(x => this.OnFitPageWidth((double)x));
            this.FitPageHeightCommand = new RelayCommand(x => this.OnFitPageHeight((Size)x));

            this.RecognizeAllCommand = new RelayCommand(x => this.RecognizeAllImages(), x => this.PreviewImages.Any());
            this.RecognizeSelectedImageCommand =
                new RelayCommand(x => this.RecognizeSelectedImage((ImagePreviewViewModel) x),
                    x => this.SelectedPreviewImage != null);

            this.CancelAllCommand = new RelayCommand(x => this.CancelAllRecognition(), x => this.PreviewImages.Any(y => y.IsProcessing && y.CanCancel));
            this.CancelSelectedCommand = new RelayCommand(x => this.CancelSelectedImageRecognition((ImagePreviewViewModel) x));

            this.ExportDataCommand = new RelayCommand(x => this.OnExportData(), x => this.RecognitionResults != null);
            this.ExportAllCommand = new RelayCommand(x => this.OnExportAllData(), x => this.RecognitionResults != null);

            this.ShowPresetsCommand = new RelayCommand(x => this.OnShowPresets());

            this.ScanImageCommand = new RelayCommand(x => this.OnScanImage());
        }

        /// <summary>
        /// Performs image scanning, saving it to user specified location and loading it to the preview items 
        /// </summary>
        private void OnScanImage()
        {
            ImageFile image;

            try
            {
                CommonDialog dialog = new CommonDialog();

                image = dialog.ShowAcquireImage(
                    WiaDeviceType.ScannerDeviceType,
                    WiaImageIntent.GrayscaleIntent,
                    WiaImageBias.MaximizeQuality,
                    WIA.FormatID.wiaFormatPNG,
                    false,
                    true,
                    false);

                if (image == null)
                {
                    return;
                }

                string savePath = DialogManager.ShowSaveScannedImageDialog(string.Empty);
                image.SaveFile(savePath);

                this.LoadPreviewImage(savePath);
            }
            catch (COMException ex)
            {
                if (ex.ErrorCode == -2145320939)
                {
                    DialogManager.ShowErrorDialog(
                        "Error retrieving a list of scanners. Please make sure your device is connected and turned on.");
                }
                else
                {
                    DialogManager.ShowErrorDialog("COM Exception : " + ex.Message);
                }
            }
            catch (Exception e)
            {
                DialogManager.ShowErrorDialog("Error occured : " + e.Message);
            }
        }

        /// <summary>
        /// Display preset settings and update configuration after view closed
        /// </summary>
        private void OnShowPresets()
        {
            this.PresetShown = true;

            PreprocessingPresetsViewModel viewModel = new PreprocessingPresetsViewModel(this.IsGeneratedTemplate,
                this.SelectedPreprocessingConfiguration);

            if (viewModel.Configuration != null)
            {
                this.SelectedPreprocessingConfiguration = viewModel.Configuration;

                PreprocessingConfigurationManager.AddConfig(this.templateId,
                    PreprocessingConfiguration.Serialize(this.SelectedPreprocessingConfiguration));
            }
        }

        /// <summary>
        /// Updates images at main area according to selected preview image
        /// </summary>
        private void UpdateMainImage()
        {
            string path = this.SelectedPreviewImage.PathToImage;
            this.MainImage = new BitmapImage(new Uri("file://" + path));

            double monitorWidth, monitorHeight;
            ResolutionUtility.GetMonitorResolution(out monitorWidth, out monitorHeight);

            ZoomKoefficient = monitorWidth / this.MainImage.PixelWidth < 1
                ? monitorWidth / this.MainImage.PixelWidth
                : 1;

            this.OnPropertyChanged(nameof(this.PageScale));
        }

        /// <summary>
        /// Adjust zoom level so that image fits page width
        /// </summary>
        /// <param name="viewportWidth">The viewport width</param>
        private void OnFitPageWidth(double viewportWidth)
        {
            // 40 pixels threshold for better visuals
            this.ZoomLevel = (viewportWidth - 40) / (this.MainImage.PixelWidth * ZoomKoefficient);
        }

        /// <summary>
        /// Adjust zoom level so that image fits page height
        /// </summary>
        /// <param name="viewportSize">The viewport size</param>
        private void OnFitPageHeight(Size viewportSize)
        {
            // 20 pixels threshold for better visuals
            this.ZoomLevel = (viewportSize.Height - 20) / (this.MainImage.PixelHeight * ZoomKoefficient);
        }

        /// <summary>
        /// Removes selected image
        /// </summary>
        private void OnRemoveSelectedPreview()
        {
            int index = this.PreviewImages.IndexOf(this.SelectedPreviewImage);
            this.PreviewImages.Remove(this.SelectedPreviewImage);

            if (this.PreviewImages.Count == 0)
            {
                this.MainImage = null;
                this.InitialPreviewPanelVisibility = Visibility.Visible;
                this.CurrentStage = ResultProcessingStages.NoImages;
            }
            else
            {
                // select previous image
                index = index - 1 < 0 ? 0 : index - 1;
                this.SelectedPreviewImage = this.PreviewImages[index];
            }
        }

        /// <summary>
        /// Loads images from specified folder
        /// </summary>
        private void OnLoadFolder()
        {
            string folder = DialogManager.ShowFolderDialog();
            if (folder == null)
            {
                return;
            }

            this.LoadImagesFromFolderByPath(folder);
        }

        private void LoadImagesFromFolderByPath(string path)
        {
            var files = DialogManager.GetImageFilesFromDirectory(path).ToArray();
            this.LoadPreviewImages(files);
        }

        /// <summary>
        /// Loads multiple images from disk
        /// </summary>
        private void OnLoadImages()
        {
            string[] files = DialogManager.ShowOpenMultiselectDialog();
            if (files == null)
            {
                return;
            }

            this.LoadPreviewImages(files);
        }

        /// <summary>
        /// Loads preview images by provided file paths
        /// </summary>
        /// <param name="files">File paths</param>
        private void LoadPreviewImages(string[] files)
        {
            if (files.Length == 0)
            {
                return;
            }

            foreach (string file in files)
            {
                this.LoadPreviewImage(file);
            }

            this.SelectedPreviewImage = this.PreviewImages.First();
        }

        /// <summary>
        /// Loads single preview image from specified path
        /// </summary>
        /// <param name="pathToImage"></param>
        private void LoadPreviewImage(string pathToImage)
        {
            FileInfo fileInfo = new FileInfo(pathToImage);

            if (!ResolutionUtility.CheckImageSize(fileInfo))
            {
                return;
            }

            // load image metadata to check width and height
            BitmapFrame bitmapFrame = BitmapFrame.Create(new Uri(pathToImage), BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
            if (bitmapFrame.PixelWidth < 1200 || bitmapFrame.PixelHeight < 1700)
            {
                    DialogManager.ShowImageSizeWarning(fileInfo.Name);
            }

            var newItem = new ImagePreviewViewModel(pathToImage, Path.GetFileName(pathToImage), fileInfo.Length);
            newItem.ImageFileFormat = fileInfo.Extension;
            this.PreviewImages.Add(newItem);

            this.InitialPreviewPanelVisibility = Visibility.Collapsed;
            this.CurrentStage = ResultProcessingStages.GotImagesToRecognize;
        }

        /// <summary>
        /// Recognize all loaded images
        /// </summary>
        private void RecognizeAllImages()
        {
            if (!this.PresetShown)
            {
                this.OnShowPresets();
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                foreach (ImagePreviewViewModel item in this.PreviewImages)
                {
                    if (item.IsProcessing)
                    {
                        this.RecognizeImageRoutine(item);
                    }
                }
            };
            worker.RunWorkerCompleted += ImagesProcessed;

            foreach (var item in this.PreviewImages)
            {
                item.IsProcessing = true;
                item.StatusText = "Waiting...";
            }

            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Check processed items state after recognition function call
        /// </summary>
        private void ImagesProcessed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.PreviewImages.Any(x => x.RecognitionError))
            {
                this.CurrentStage = ResultProcessingStages.RecognitionError;
            }
            else
            {
                this.CurrentStage = ResultProcessingStages.GotResultsToExport;
            }
        }

        /// <summary>
        /// Cancel recognition for selected image
        /// </summary>
        /// <param name="itemToCancel">Image to cancel recognition for</param>
        private void CancelSelectedImageRecognition(ImagePreviewViewModel itemToCancel)
        {
            if (itemToCancel.CanCancel)
            {
                itemToCancel.IsProcessing = false;
            }
        }

        /// <summary>
        /// Recognize single selected image
        /// </summary>
        /// <param name="imagePreview">Item to recognize</param>
        private void RecognizeSelectedImage(ImagePreviewViewModel imagePreview)
        {
            if (!this.PresetShown)
            {
                this.OnShowPresets();
            }

            var itemToProcess = imagePreview ?? this.SelectedPreviewImage;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                this.RecognizeImageRoutine(itemToProcess);
            };
            worker.RunWorkerCompleted += ImagesProcessed;

            itemToProcess.IsProcessing = true;
            itemToProcess.StatusText = "Waiting...";

            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Performs recognition for provided image
        /// </summary>
        /// <param name="itemToProcess">Image to process</param>
        private void RecognizeImageRoutine(ImagePreviewViewModel itemToProcess)
        {
            // check if image was already processed
            if (itemToProcess.IsProcessed)
            {
                // ask user confirmation
                if (!DialogManager.ShowConfirmDialog(
                    string.Format("Image \"{0}\" was already recognized. Are you sure you want to run recognition again?",
                        itemToProcess.Title)))
                {
                    // if no confirmation, clean up and return
                    itemToProcess.IsProcessing = false;
                    itemToProcess.StatusText = string.Empty;
                    return;
                }
            }

            itemToProcess.CanCancel = false;
            itemToProcess.StatusText = "Preparing for dispatch...";

            if (!File.Exists(itemToProcess.PathToImage))
            {
                DialogManager.ShowErrorDialog("Could not find image " + "\"" + itemToProcess.PathToImage + "\".");
                return;
            }

            byte[] imageData = this.PreprocessImage(itemToProcess.PathToImage, this.SelectedPreprocessingConfiguration);
            string additionalPars = string.Empty;

            itemToProcess.StatusText = "Recognizing...";

            try
            {
                ImageRecognitionResult result =
                    CoreApi.RecognizeImage(itemToProcess.Title,
                        imageData,
                        this.templateId,
                        itemToProcess.WasUploaded,
                        additionalPars);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.ProcessResult(result, itemToProcess);

                    // update item states
                    itemToProcess.WasUploaded = true;
                    itemToProcess.IsProcessing = false;
                    itemToProcess.StatusText = string.Empty;
                    itemToProcess.CanCancel = true;
                    itemToProcess.RecognitionError = false;
                });
            }
            catch (Exception e)
            {
                // clean up in case of exception
                itemToProcess.IsProcessing = false;
                itemToProcess.StatusText = string.Empty;
                itemToProcess.CanCancel = true;
                itemToProcess.RecognitionError = true;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    DialogManager.ShowErrorDialog(e.Message);
                });
            }
        }

        /// <summary>
        /// Preprocess image according to selected configuration
        /// </summary>
        /// <param name="pathToImage">Path to the image</param>
        /// <param name="config">Preprocessing configuration</param>
        /// <returns>Preprocessed image data</returns>
        private byte[] PreprocessImage(string pathToImage, PreprocessingConfiguration config)
        {
            long fileLengthKb = new FileInfo(pathToImage).Length / 1024;

            // if preprocessing enabled and image size is bigger then threshold preprocess
            if (config.IsPreprocessingEnabled && fileLengthKb > config.ExcludeImagesSize)
            {
                byte[] imageData = ImageProcessor.CompressAndResizeImage(pathToImage, config.JpegCompressionLevel,
                    config.DesiredWidth, config.DesiredHeight);

                return imageData;
            }
            // simply pack data without resize
            else
            {
                byte[] imageData = File.ReadAllBytes(pathToImage);
                return imageData;
            }
        }

        /// <summary>
        /// Remove images from recognition queue
        /// </summary>
        private void CancelAllRecognition()
        {
            foreach (ImagePreviewViewModel item in this.PreviewImages)
            {
                if (item.CanCancel)
                {
                    item.IsProcessing = false;
                }
            }
        }

        /// <summary>
        /// Parses and saves recognition result for image
        /// </summary>
        /// <param name="results">Recognition results recieved from omr core</param>
        /// <param name="item">The preview image that was recognized</param>
        private void ProcessResult(ImageRecognitionResult results, ImagePreviewViewModel item)
        {
            ObservableCollection<RecognitionResult> recognitionResults = this.ParseAnswers(results.RecognizedAnswers);
            item.RecognitionResults = recognitionResults;
            item.ClippedAreas = new ObservableCollection<ClippedArea>();

            foreach (var clippedArea in results.ClippedAreas)
            {
                item.ClippedAreas.Add(new ClippedArea(clippedArea.Key, clippedArea.Value));
            }

            this.OnPropertyChanged(nameof(this.RecognitionResults));
            this.OnPropertyChanged(nameof(this.ClippedAreas));
        }

        /// <summary>
        /// Export all data from all recognized images (seprate file for each image)
        /// </summary>
        private void OnExportAllData()
        {
            string path = DialogManager.ShowSaveDataFolderDialog();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            foreach (ImagePreviewViewModel imagePreviewViewModel in this.PreviewImages)
            {
                string savePath = path + @"\" + Path.GetFileNameWithoutExtension(imagePreviewViewModel.Title) + ".csv";
                if (imagePreviewViewModel.IsProcessed && imagePreviewViewModel.RecognitionResults != null)
                {
                    this.ExportFileRoutine(savePath, imagePreviewViewModel.RecognitionResults);
                    this.ExportClipAreas(savePath, imagePreviewViewModel.ClippedAreas);
                }
            }
        }

        /// <summary>
        /// Saves recognition results of selected image to .csv format by specified path
        /// </summary>
        private void OnExportData()
        {
            string imageName = Path.GetFileNameWithoutExtension(this.SelectedPreviewImage.Title);

            string path = DialogManager.ShowSaveDataDialog(imageName);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            this.ExportFileRoutine(path, this.RecognitionResults);
            this.ExportClipAreas(path, this.ClippedAreas);
        }

        private void ExportClipAreas(string path, ObservableCollection<ClippedArea> clippedAreas)
        {
            if (!clippedAreas.Any())
            {
                return;
            }

            try
            {
                var directory = Path.GetDirectoryName(path);
                var title = Path.GetFileNameWithoutExtension(path);
                var directoryInfo = Directory.CreateDirectory(Path.Combine(directory, title) + "ClippedAreas");
                foreach (ClippedArea area in clippedAreas)
                {
                    string fileName = Path.Combine(directoryInfo.FullName, area.AreaName);
                    if (File.Exists(fileName))
                    {
                        bool dialogRes = DialogManager.ShowConfirmDialog("File \"" + fileName + "\" already exists. Do you want to overwrite it?");
                        if (!dialogRes)
                        {
                            continue;
                        }
                    }

                    File.WriteAllBytes(Path.Combine(directoryInfo.FullName, area.AreaName), area.ImageData);
                }
            }
            catch(Exception e)
            {
                DialogManager.ShowErrorDialog("Failed to save clipped areas! " + e.Message);
            }
        }

        /// <summary>
        /// Perform export of provided data to specified path
        /// </summary>
        /// <param name="savePath">Export path</param>
        /// <param name="resultsToExport">Results to export</param>
        private void ExportFileRoutine(string savePath, ObservableCollection<RecognitionResult> resultsToExport)
        {
            StringBuilder csv = new StringBuilder();

            // char to use as separator in CSV file, differs depending on CurrentCulture
            string separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;

            csv.Append("Element Name").Append(separator);
            csv.Append("Value").AppendLine(separator);

            foreach (RecognitionResult result in resultsToExport)
            {
                csv.Append(result.QuestionName);
                csv.Append(separator);
                csv.AppendLine(result.AnswerKey);
            }

            try
            {
                File.WriteAllText(savePath, csv.ToString());
            }
            catch (Exception e)
            {
                DialogManager.ShowErrorDialog(e.Message);
            }
        }
    }
}
