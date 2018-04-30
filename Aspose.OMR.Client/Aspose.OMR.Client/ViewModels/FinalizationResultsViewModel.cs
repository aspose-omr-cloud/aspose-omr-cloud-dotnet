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
    using System.Collections.ObjectModel;
    using System.Windows.Media.Imaging;
    using System.Windows;
    using Utility;
    using Views;

    /// <summary>
    /// View model for recognition results recieved during finalization
    /// </summary>
    public class FinalizationResultsViewModel : ViewModelBase
    {
        /// <summary>
        /// View displayed to the user
        /// </summary>
        private readonly FinalizationResultsView view;

        /// <summary>
        /// Template image that was used during finalization
        /// </summary>
        private BitmapImage mainImage;

        /// <summary>
        /// True zoom level without koefficient
        /// </summary>
        private double zoomLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="FinalizationResultsViewModel"/> class
        /// </summary>
        /// <param name="recognitionResults">Recieved recognition results</param>
        /// <param name="templateImage">Template image</param>
        public FinalizationResultsViewModel(ObservableCollection<RecognitionResult> recognitionResults, BitmapImage templateImage)
        {
            this.RecognitionResults = recognitionResults;
            this.GoToTemplateCreationCommand = new RelayCommand(x => this.OnGoToTemplateCreationCommand());
            this.GoToRecognitionCommand = new RelayCommand(x => this.OnGoToRecognition());

            this.ZoomInCommand = new RelayCommand(x => this.ZoomLevel = Math.Min(this.ZoomLevel + 0.1, 4));
            this.ZoomOutCommand = new RelayCommand(x => this.ZoomLevel = Math.Max(this.ZoomLevel - 0.1, 0.1));
            this.ZoomOriginalCommand = new RelayCommand(x => this.ZoomLevel = 1);
            this.FitPageWidthCommand = new RelayCommand(x => this.OnFitPageWidth((double)x));
            this.FitPageHeightCommand = new RelayCommand(x => this.OnFitPageHeight((Size)x));

            ZoomKoefficient = 1;
            this.zoomLevel = 1;

            this.MainImage = templateImage;
            this.view = new FinalizationResultsView(this);
            this.view.ShowDialog();
        }

        /// <summary>
        /// Gets or sets the recognition results
        /// </summary>
        public ObservableCollection<RecognitionResult> RecognitionResults { get; set; }

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
        /// Gets or sets zoom koefficient for better representation of large images (values from 0 to 1)
        /// </summary>
        public static double ZoomKoefficient { get; set; }

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
        /// Gets a value indicating whether user decided to accpet finalization results and to go recognition stage
        /// </summary>
        public bool ProceedToRecognition { get; private set; }

        /// <summary>
        /// Gets or sets the GoToTemplateCreationCommand command
        /// </summary>
        public RelayCommand GoToTemplateCreationCommand { get; set; }

        /// <summary>
        /// Gets or sets the GoToRecognition command
        /// </summary>
        public RelayCommand GoToRecognitionCommand { get; set; }

        public RelayCommand ZoomInCommand { get; private set; }

        public RelayCommand ZoomOutCommand { get; private set; }

        public RelayCommand ZoomOriginalCommand { get; private set; }

        public RelayCommand FitPageWidthCommand { get; private set; }

        public RelayCommand FitPageHeightCommand { get; private set; }

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
        /// Closes window and gets back to working with template
        /// </summary>
        private void OnGoToTemplateCreationCommand()
        {
            this.ProceedToRecognition = false;
            this.view.Close();
        }

        /// <summary>
        /// Closes window and gets to recognition
        /// </summary>
        private void OnGoToRecognition()
        {
            this.ProceedToRecognition = true;
            this.view.Close();
        }
    }
}
