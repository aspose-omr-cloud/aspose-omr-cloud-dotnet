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
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Utility;
    using Views;

    /// <summary>
    /// View model class for Template Generator view
    /// </summary>
    public class TemplateGeneratorViewModel : ViewModelBase
    {
        /// <summary>
        /// Template generator window
        /// </summary>
        private readonly TemplateGeneratorView view;

        /// <summary>
        /// String containing template description
        /// </summary>
        private string templateDescription;

        /// <summary>
        /// Name of the template
        /// </summary>
        private string templateName;

        /// <summary>
        /// Selected font family
        /// </summary>
        private FontFamily selectedFontFamily;

        /// <summary>
        /// Selected font size
        /// </summary>
        private int selectedFontSize;

        /// <summary>
        /// Cloud storage path where to look for images used in generation
        /// </summary>
        private string extraStoragePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateGeneratorViewModel"/> class
        /// </summary>
        public TemplateGeneratorViewModel()
        {
            // init commands
            this.UploadImagesCommand = new RelayCommand(x => this.UploadImages());
            this.GenerateCommand = new RelayCommand(x => this.OnGenerateCommand());
            this.CloseCommand = new RelayCommand(x => this.OnCloseCommand());
            this.LoadFromFileCommand = new RelayCommand(x => this.LoadDescriptionFromFile());
            this.SaveMarkupFileCommand = new RelayCommand(x => this.SaveDescriptionToFile(), x => !string.IsNullOrEmpty(this.TemplateDescription));

            this.SetDefaultVisuals();

            // init view
            this.view = new TemplateGeneratorView(this);

            // display view
            this.view.ShowDialog();
        }

        public RelayCommand UploadImagesCommand { get; private set; }

        public RelayCommand LoadFromFileCommand { get; private set; }

        public RelayCommand SaveMarkupFileCommand { get; private set; }

        public RelayCommand GenerateCommand { get; private set; }

        public RelayCommand CloseCommand { get; private set; }

        /// <summary>
        /// Gets or sets the template description string
        /// </summary>
        public string TemplateDescription
        {
            get { return this.templateDescription; }
            set
            {
                this.templateDescription = value;
                this.IsDirty = true;
                this.OnPropertyChanged();
                this.SaveMarkupFileCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets the name of the generated template
        /// </summary>
        public string TemplateName
        {
            get { return this.templateName; }
            set
            {
                this.templateName = value;
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
        /// Gets the list of supported Font Families
        /// </summary>
        public List<FontFamily> FontFamilies
        {
            get
            {
                return Fonts.SystemFontFamilies.OrderBy(f => f.Source).ToList();
            }
        }

        /// <summary>
        /// Gets or sets the selected font family
        /// </summary>
        public FontFamily SelectedFontFamily
        {
            get { return this.selectedFontFamily; }
            set
            {
                this.selectedFontFamily = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the list of font sizes
        /// </summary>
        public List<int> FontSizes
        {
            get { return new List<int>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22 }; }
        }

        /// <summary>
        /// Gets or sets the selected font size value
        /// </summary>
        public int SelectedFontSize
        {
            get { return this.selectedFontSize; }
            set
            {
                this.selectedFontSize = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the value indicating whether generation markup has unsaved changes                                                        
        /// </summary>
        public bool IsDirty { get; set; }

        /// <summary>
        /// Gets the produced template generation results
        /// </summary>
        public TemplateGenerationContent GenerationContent { get; private set; }

        /// <summary>
        /// Generation delegate containing generation result
        /// </summary>
        /// <param name="generationResult">Generation result</param>
        public delegate void GenerationDelegate(TemplateGenerationContent generationResult);

        /// <summary>
        /// Fires when generation view is closed
        /// </summary>
        public event GenerationDelegate ViewClosed;

        /// <summary>
        /// Display image upload view
        /// </summary>
        private void UploadImages()
        {
            var imagesUploadViewModel = new ImageUploadViewModel(this.view);
            imagesUploadViewModel.ViewClosed += this.ImagesUploadComplete;
        }

        /// <summary>
        /// Fires when image upload is complete
        /// </summary>
        /// <param name="imagesFolder">Location where images were uploaded</param>
        private void ImagesUploadComplete(string imagesFolder)
        {
            this.ExtraStoragePath = imagesFolder;
        }

        /// <summary>
        /// Applies changes and closes window
        /// </summary>
        private async void OnGenerateCommand()
        {
            if (!this.ValidateValues())
            {
                return;
            }

            if (this.IsDirty)
            {
                MessageBoxResult dialogResult =
                    DialogManager.ShowConfirmDirtyClosingDialog(
                        "Template generation markup has unsaved changes. Do you want to save them?");

                if (dialogResult == MessageBoxResult.Cancel)
                {
                    return;
                }

                if (dialogResult == MessageBoxResult.Yes)
                {
                    this.SaveDescriptionToFile();
                }
            }

            await this.GenerateTemplate();
            this.view.Close();

            if (this.ViewClosed != null)
            {
                this.ViewClosed(this.GenerationContent);
            }
        }

        /// <summary>
        /// Calls template generation
        /// </summary>
        private async Task GenerateTemplate()
        {
            this.view.Hide();
            BusyIndicatorManager.Enable();

            string expectedFileNameExtension = ".txt";

            byte[] templateDescriptionBytes = Encoding.UTF8.GetBytes(this.TemplateDescription);

            if (!Path.HasExtension(this.TemplateName) || 
                (Path.HasExtension(this.TemplateName) && Path.GetExtension(this.TemplateName) != expectedFileNameExtension))
            { 
                this.TemplateName += expectedFileNameExtension;
            }

            try
            {
                this.GenerationContent = await Task.Run(() => CoreApi.GenerateTemplate(
                    this.TemplateName,
                    templateDescriptionBytes,
                    this.ExtraStoragePath,
                    string.Empty));

                this.GenerationContent.Name = this.TemplateName;
            }
            catch (Exception e)
            {
                DialogManager.ShowErrorDialog(e.Message);
            }
            finally
            {
                BusyIndicatorManager.Disable();
            }
        }

        /// <summary>
        /// Set default values for font size, font family and font weight
        /// </summary>
        private void SetDefaultVisuals()
        {
            this.SelectedFontSize = 12;

            // try to find Segoe UI font and set it as default
            FontFamily segoeFont = this.FontFamilies.FirstOrDefault(x => x.Source.Equals("Segoe UI"));
            if (segoeFont != null)
            {
                this.SelectedFontFamily = segoeFont;
                return;
            }

            // if failed with Segoe, try to find Arial, as it should be installed by default on Windows
            FontFamily arialFont = this.FontFamilies.FirstOrDefault(x => x.Source.Equals("Arial"));
            if (arialFont != null)
            {
                this.SelectedFontFamily = arialFont;
                return;
            }

            // if, for some reason, failed with Arial, just choose first from installed list
            this.SelectedFontFamily = this.FontFamilies[0];
        }

        /// <summary>
        /// Loads template description from provided file
        /// </summary>
        private void LoadDescriptionFromFile()
        {
            string imagePath = DialogManager.ShowOpenTextDialog();
            if (imagePath == null)
            {
                return;
            }

            this.TemplateDescription = File.ReadAllText(imagePath);
            this.IsDirty = false;
            if (string.IsNullOrEmpty(this.TemplateName))
            {
                this.TemplateName = Path.GetFileNameWithoutExtension(imagePath);
            }
        }

        /// <summary>
        /// Saves template description to specified location
        /// </summary>
        private void SaveDescriptionToFile()
        {
            string path = DialogManager.ShowSaveTemplateDescriptionDialog();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            try
            {
                File.WriteAllText(path, this.TemplateDescription);
                this.IsDirty = false;
            }
            catch (Exception e)
            {
                DialogManager.ShowErrorDialog(e.Message);
            }
        }

        /// <summary>
        /// Validate input values
        /// </summary>
        /// <returns>False if validation failed, true otherwise</returns>
        private bool ValidateValues()
        {
            if (string.IsNullOrEmpty(this.TemplateDescription))
            {
                DialogManager.ShowErrorDialog("Template Description cannot be empty!");
                return false;
            }

            if (string.IsNullOrEmpty(this.TemplateName))
            {
                DialogManager.ShowErrorDialog("Template Name cannot be empty!");
                return false;
            }

            return true;
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
