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
    using System.ComponentModel;
    using System.Reflection;
    using Utility;
    using Views;

    /// <summary>
    /// View model for Presets View
    /// </summary>
    public class PreprocessingPresetsViewModel : ViewModelBase
    {
        /// <summary>
        /// Image preprocessing presets view
        /// </summary>
        private readonly PreprocessingPresetsView view;

        /// <summary>
        /// Selected preset
        /// </summary>
        private PreprocessingPreset selectedPreset;

        /// <summary>
        /// Indicates if preprocessing should be enabled
        /// </summary>
        private bool isPreprocessingEnabled;

        /// <summary>
        /// Jpeg quality level
        /// </summary>
        private int compressionLevel;

        /// <summary>
        /// Desired image width
        /// </summary>
        private int imageDesiredWidth;

        /// <summary>
        /// Desired image height
        /// </summary>
        private int imageDesiredHeight;

        /// <summary>
        /// Size of the images that should be excluded from preprocessing
        /// </summary>
        private int excludeImagesSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreprocessingPresetsViewModel"/> class.
        /// </summary>
        /// <param name="isGenerated">Indicates if template was generated</param>
        /// <param name="configuration">Latest used configuration if any</param>
        public PreprocessingPresetsViewModel(bool isGenerated, PreprocessingConfiguration configuration)
        {
            this.ApplyCommand = new RelayCommand(x => this.ApplySettings());
            this.CancelCommand = new RelayCommand(x => this.Cancel());

            this.LoadConfiguration(configuration, isGenerated);

            this.view = new PreprocessingPresetsView(this);
            this.view.ShowDialog();
        }

        /// <summary>
        /// Loads image preprocessing configuration. Sets to defauls in there is no configuration
        /// </summary>
        /// <param name="configuration">Loaded configuration</param>
        /// <param name="isGenerated">Indicates if template was generated</param>
        private void LoadConfiguration(PreprocessingConfiguration configuration, bool isGenerated)
        {
            if (configuration != null)
            {
                this.IsPreprocessingEnabled = configuration.IsPreprocessingEnabled;

                this.compressionLevel = configuration.JpegCompressionLevel;
                this.imageDesiredWidth = configuration.DesiredWidth;
                this.imageDesiredHeight = configuration.DesiredHeight;
                this.excludeImagesSize = configuration.ExcludeImagesSize;

                this.SelectedPreset = configuration.SelectedPreset;
            }
            else
            {
                this.IsPreprocessingEnabled = true;
                this.ExcludeImagesSize = 300;
                this.SelectedPreset = isGenerated ? PreprocessingPreset.AsposeImages : PreprocessingPreset.NormalQuality;
            }
        }

        /// <summary>
        /// Gets or sets the apply command
        /// </summary>
        public RelayCommand ApplyCommand { get; private set; }

        /// <summary>
        /// Gets or sets the cancel command
        /// </summary>
        public RelayCommand CancelCommand { get; private set; }

        /// <summary>
        /// Gets or sets the resulting preprocessing configuration
        /// </summary>
        public PreprocessingConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the selected preset setting
        /// </summary>
        public PreprocessingPreset SelectedPreset
        {
            get { return this.selectedPreset; }
            set
            {
                this.selectedPreset = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(PresetDescription));
                this.UpdateSelectedConfiguration();
            }
        }

        /// <summary>
        /// Gets the preset description string from enum attributes
        /// </summary>
        public string PresetDescription
        {
            get
            {
                FieldInfo fieldInfo = this.SelectedPreset.GetType().GetField(this.SelectedPreset.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    return attributes[0].Description;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is preprocessing should be enabled
        /// </summary>
        public bool IsPreprocessingEnabled
        {
            get { return this.isPreprocessingEnabled; }
            set
            {
                this.isPreprocessingEnabled = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the minimal size of the images that should be excluded from preprocessing
        /// </summary>
        public int ExcludeImagesSize
        {
            get { return this.excludeImagesSize; }
            set
            {
                this.excludeImagesSize = value;
                this.SelectedPreset = PreprocessingPreset.CustomSettings;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the desired width of the image
        /// </summary>
        public int ImageDesiredWidth
        {
            get { return this.imageDesiredWidth; }
            set
            {
                this.imageDesiredWidth = value;
                this.SelectedPreset = PreprocessingPreset.CustomSettings;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the desired height of the image
        /// </summary>
        public int ImageDesiredHeight
        {
            get { return this.imageDesiredHeight; }
            set
            {
                this.imageDesiredHeight = value;
                this.SelectedPreset = PreprocessingPreset.CustomSettings;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the jpeg quality level
        /// </summary>
        public int CompressionLevel
        {
            get { return this.compressionLevel; }
            set
            {
                this.compressionLevel = value; 
                this.SelectedPreset = PreprocessingPreset.CustomSettings;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Updates selected configuration based on selected preset
        /// </summary>
        private void UpdateSelectedConfiguration()
        {
            switch (this.SelectedPreset)
            {
                case PreprocessingPreset.AsposeImages:
                    this.compressionLevel = 65;
                    this.imageDesiredWidth = this.imageDesiredHeight = 2000;
                    this.excludeImagesSize = 300;
                    break;

                case PreprocessingPreset.NormalQuality:
                    this.compressionLevel = 70;
                    this.imageDesiredWidth = this.imageDesiredHeight = 3000;
                    this.excludeImagesSize = 300;
                    break;

                case PreprocessingPreset.GoodQuiality:
                    this.compressionLevel = 55;
                    this.imageDesiredWidth = this.imageDesiredHeight = 2900;
                    this.excludeImagesSize = 300;
                    break;
            }

            this.OnPropertyChanged(nameof(CompressionLevel));
            this.OnPropertyChanged(nameof(ExcludeImagesSize));
            this.OnPropertyChanged(nameof(ImageDesiredHeight));
            this.OnPropertyChanged(nameof(ImageDesiredWidth));
        }

        /// <summary>
        /// Saves settings to preprocessing configuration object and closes view
        /// </summary>
        private void ApplySettings()
        {
            this.Configuration = new PreprocessingConfiguration();

            if (!this.IsPreprocessingEnabled)
            {
                this.Configuration.IsPreprocessingEnabled = false;
                this.view.Close();
                return;
            }

            this.Configuration.IsPreprocessingEnabled = true;
            this.Configuration.SelectedPreset = this.SelectedPreset;

            this.Configuration.JpegCompressionLevel = this.CompressionLevel;
            this.Configuration.DesiredWidth = this.ImageDesiredWidth;
            this.Configuration.DesiredHeight = this.imageDesiredHeight;
            this.Configuration.ExcludeImagesSize = this.ExcludeImagesSize;

            this.view.Close();
        }

        /// <summary>
        /// Cancel changes
        /// </summary>
        private void Cancel()
        {
            this.Configuration = null;
            this.view.Close();
        }
    }
}
