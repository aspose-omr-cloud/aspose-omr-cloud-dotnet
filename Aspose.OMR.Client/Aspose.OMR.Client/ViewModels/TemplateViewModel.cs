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
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using TemplateModel;
    using UndoRedo;
    using Utility;

    /// <summary>
    /// View model for the template view
    /// </summary>
    public class TemplateViewModel : TabViewModel
    {
        #region Fields
        
        /// <summary>
        /// Buffer to store copied elements
        /// </summary>
        private readonly ObservableCollection<BaseQuestionViewModel> copiedQuestionsBuffer;

        /// <summary>
        /// Current page elements
        /// </summary>
        private ObservableCollection<BaseQuestionViewModel> pageQuestions;

        /// <summary>
        /// Selected elements
        /// </summary>
        private ObservableCollection<BaseQuestionViewModel> selectedElements;

        /// <summary>
        /// List of warnings recieved from omr core after template finalization
        /// </summary>
        private ObservableCollection<string> warnings;

        /// <summary>
        /// Template image displayed to the user
        /// </summary>
        private BitmapImage templateImage;

        /// <summary>
        /// Indicates that user is currently in adding chocie box mode
        /// </summary>
        private bool isAddingChoiceBox;

        /// <summary>
        /// Indicates that user is currently in adding grid mode
        /// </summary>
        private bool isAddingGrid;

        /// <summary>
        /// Indicates that user is currently in adding barcode mode
        /// </summary>
        private bool isAddingBarcode;

        /// <summary>
        /// Indicates that template correction has been done
        /// </summary>
        private bool correctionComplete;

        /// <summary>
        /// Indicates that template finalization was complete with no warnings
        /// </summary>
        private bool finalizationComplete;

        /// <summary>
        /// True zoom level without koefficient
        /// </summary>
        private double zoomLevel;

        /// <summary>
        /// The template id used by omr core
        /// </summary>
        private string templateId;

        /// <summary>
        /// Indicates visibility of start panel with quick access commands
        /// </summary>
        private Visibility startPanelVisibility;

        /// <summary>
        /// Page width in pixels
        /// </summary>
        private double pageWidth;

        /// <summary>
        /// Page height in pixels
        /// </summary>
        private double pageHeight;

        /// <summary>
        /// Help message containing various useful information displayed at lower toolbar
        /// </summary>
        private string helpMessage;

        /// <summary>
        /// Current template creation workflow stage
        /// </summary>
        private TemplateCreationStages currentStage;

        /// <summary>
        /// Indicates whether to display properties panel
        /// </summary>
        private bool showPropertiesPanel = true;

        /// <summary>
        /// Width of the properties column
        /// </summary>
        private double propertiesPanelWidth = 220;

        /// <summary>
        /// Remembered properties width (that might be set by the user) to restore panel in right size
        /// </summary>
        private double savedPropertiesWidth;

        /// <summary>
        /// Template Name
        /// </summary>
        private string templateName;

        /// <summary>
        /// Indicated whether template validation can be done
        /// </summary>
        private bool canValidate;

        /// <summary>
        /// Indicated whether template validation has failed
        /// </summary>
        private bool validationFailed;

        /// <summary>
        /// Minimum width on the properties pane in closed state.
        /// </summary>
        private const double PropertiesPanelClosedWidth = 24d;

        /// <summary>
        /// Static zoom koefficient value for better representation of large images (values from 0 to 1)
        /// </summary>
        private static double zoomKoefficient = 1;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateViewModel"/> class.
        /// </summary>
        /// <param name="finalizationComplete">Flag indicating that template is finalized</param>
        /// <param name="templateId">Template identifier string</param>
        public TemplateViewModel(bool finalizationComplete, string templateId)
        {
            this.Warnings = new ObservableCollection<string>();
            this.pageQuestions = new ObservableCollection<BaseQuestionViewModel>();
            this.selectedElements = new ObservableCollection<BaseQuestionViewModel>();
            this.copiedQuestionsBuffer = new ObservableCollection<BaseQuestionViewModel>();

            this.zoomLevel = 1;

            this.InitCommands();

            this.TemplateId = templateId;
            this.FinalizationComplete = finalizationComplete;

            this.CorrectionComplete = !string.IsNullOrEmpty(this.TemplateId);
            this.CanValidate = !this.FinalizationComplete;
            this.IsDirty = false;

            this.UpdateTemplateCreationStage();
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether template was generated
        /// </summary>
        public bool IsGeneratedTemplate { get; set; }

        /// <summary>
        /// Gets a value indicating whether template validation failed
        /// </summary>
        public bool ValidationFailed
        {
            get { return this.validationFailed; }
            private set
            {
                this.validationFailed = value;
                this.UpdateTemplateCreationStage();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether template correction is complete with no errors
        /// </summary>
        public bool CorrectionComplete
        {
            get { return this.correctionComplete; }
            set
            {
                if (this.correctionComplete != value)
                {
                    this.IsDirty = true;
                }

                this.correctionComplete = value;
                this.OnPropertyChanged();
                this.UpdateTemplateCreationStage();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether template finalization is complete with no warnings
        /// </summary>
        public bool FinalizationComplete
        {
            get { return this.finalizationComplete; }
            set
            {
                if (this.finalizationComplete != value)
                {
                    this.IsDirty = true;
                }

                this.finalizationComplete = value;
                this.OnPropertyChanged();
                this.UpdateTemplateCreationStage();
            }
        }

        /// <summary>
        /// Gets or sets the path that template was loaded from
        /// </summary>
        public string LoadedPath { get; set; }

        /// <summary>
        /// Gets or sets the width of the properties panel
        /// </summary>
        public double PropertiesPanelWidth
        {
            get { return this.propertiesPanelWidth; }
            set
            {
                this.propertiesPanelWidth = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether properties panel should be displayed
        /// </summary>
        public bool ShowPropertiesPanel
        {
            get { return this.showPropertiesPanel; }
            set
            {
                this.showPropertiesPanel = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets zoom koefficient for better representation of large images (values from 0 to 1)
        /// </summary>
        public static double ZoomKoefficient
        {
            get
            {
                return zoomKoefficient;
            }
            set
            {
                zoomKoefficient = value;
            }
        }

        /// <summary>
        /// Updates workflow stage depending on various conditions
        /// </summary>
        private void UpdateTemplateCreationStage()
        {
            if (this.TemplateImage == null)
            {
                this.CurrentStage = TemplateCreationStages.NoImage;
            }
            else if (this.PageQuestions.Count == 0)
            {
                this.CurrentStage = TemplateCreationStages.NoElements;
            }
            else if (this.PageQuestions.Count < 3)
            {
                this.CurrentStage = TemplateCreationStages.WorkWithElements;
            }
            else if (string.IsNullOrEmpty(this.TemplateId))
            {
                this.CurrentStage = TemplateCreationStages.NoValidation;
            }
            else if (this.ValidationFailed || (!string.IsNullOrEmpty(this.TemplateId) && !this.FinalizationComplete))
            {
                this.CurrentStage = TemplateCreationStages.ValidatedWithErrors;
            }
            else if (this.FinalizationComplete)
            {
                this.CurrentStage = TemplateCreationStages.ValidatedWithNoErrors;
            }
        }

        /// <summary>
        /// Gets current template creation stage
        /// </summary>
        public TemplateCreationStages CurrentStage
        {
            get { return this.currentStage; }
            private set
            {
                this.currentStage = value;

                // update help message
                this.HelpMessage = HelpManager.GetMessageByStage(value);
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
        /// Gets or sets the template id used by omr core
        /// </summary>
        public string TemplateId
        {
            get { return this.templateId; }
            set
            {
                this.templateId = value;
                this.UpdateTemplateCreationStage();
            }
        }

        /// <summary>
        /// Gets or sets the selected elements
        /// </summary>
        public ObservableCollection<BaseQuestionViewModel> SelectedElements
        {
            get { return this.selectedElements; }
            set
            {
                this.selectedElements = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the page elements
        /// </summary>
        public ObservableCollection<BaseQuestionViewModel> PageQuestions
        {
            get { return this.pageQuestions; }
            private set
            {
                this.pageQuestions = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether adding choice box mode is enabled
        /// </summary>
        public bool IsAddingChoiceBox
        {
            get { return this.isAddingChoiceBox; }
            set
            {
                if (value)
                {
                    this.ResetAddingElementModes();
                }

                this.isAddingChoiceBox = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether adding grid mode is enabled
        /// </summary>
        public bool IsAddingGrid
        {
            get { return this.isAddingGrid; }
            set
            {
                if (value)
                {
                    this.ResetAddingElementModes();
                }

                this.isAddingGrid = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether adding grid mode is enabled
        /// </summary>
        public bool IsAddingBarcode
        {
            get { return this.isAddingBarcode; }
            set
            {
                if (value)
                {
                    this.ResetAddingElementModes();
                }

                this.isAddingBarcode = value;
                this.OnPropertyChanged();
            }
        }

        private void ResetAddingElementModes()
        {
            this.IsAddingChoiceBox = false;
            this.IsAddingGrid = false;
            this.IsAddingBarcode = false;
        }

        /// <summary>
        /// Gets the data context for properties view
        /// </summary>
        public BaseQuestionViewModel PropertiesContext
        {
            get { return this.SelectedElements.Count == 1 ? this.SelectedElements[0] : null; }
        }

        /// <summary>
        /// Gets the visual string representation of current page scale
        /// </summary>
        public string PageScaleDisplayString
        {
            get { return Math.Round(this.ZoomLevel * 100) + "%"; }
        }

        /// <summary>
        /// Gets scale used to scale visuals in template view with respect to scaling koefficient
        /// </summary>
        public double PageScale
        {
            get { return this.ZoomLevel * ZoomKoefficient; }
        }

        /// <summary>
        /// Gets the list of warnings recieved after template finalization
        /// </summary>
        public ObservableCollection<string> Warnings
        {
            get { return this.warnings; }
            private set
            {
                this.warnings = value;
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
                if (this.TemplateImage != null)
                {
                    this.zoomLevel = value;
                    this.OnPropertyChanged();
                    this.OnPropertyChanged(nameof(this.PageScale));
                    this.OnPropertyChanged(nameof(this.PageScaleDisplayString));
                }
            }
        }

        /// <summary>
        /// Gets or sets the template page image displayed to the user
        /// </summary>
        public BitmapImage TemplateImage
        {
            get { return this.templateImage; }
            set
            {
                this.templateImage = value;
                this.WasUploaded = false;

                if (this.templateImage != null)
                {
                    this.StartPanelVisibility = Visibility.Collapsed;
                    this.PageWidth = this.templateImage.PixelWidth;
                    this.PageHeight = this.templateImage.PixelHeight;
                }
                else
                {
                    this.StartPanelVisibility = Visibility.Visible;
                }

                this.UpdateTemplateCreationStage();
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether template image was already uploaded on cloud
        /// </summary>
        public bool WasUploaded { get; set; }

        /// <summary>
        /// Gets or sets page width
        /// </summary>
        public double PageWidth
        {
            get { return this.pageWidth; }
            set
            {
                this.pageWidth = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets page height
        /// </summary>
        public double PageHeight
        {
            get { return this.pageHeight; }
            set
            {
                this.pageHeight = value; 
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets visibility of start panel
        /// </summary>
        public Visibility StartPanelVisibility
        {
            get { return this.startPanelVisibility; }
            set
            {
                this.startPanelVisibility = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the template image size in bytes
        /// </summary>
        public long ImageSizeInBytes { get; private set; }

        /// <summary>
        /// Gets or sets the template image name
        /// </summary>
        public string TemplateImageName { get; set; }

        /// <summary>
        /// Gets or sets the template name
        /// </summary>
        public string TemplateName
        {
            get { return this.templateName; }
            set
            {
                this.templateName = value;
                this.TabName = value;
            }
        }

        /// <summary>
        /// Gets or sets image file format
        /// </summary>
        public string ImageFileFormat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether template can be validated
        /// </summary>
        public bool CanValidate
        {
            get
            {
                if (this.PageQuestions.Count == 0)
                {
                    return false;
                }

                return this.canValidate;
            }
            set
            {
                this.canValidate = value;
                this.OnPropertyChanged();
                this.ValidateTemplateCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Finalization approved delegate
        /// </summary>
        public delegate void FinalizationApprovedDelegate();

        /// <summary>
        /// Fires when finalization results are approved by user
        /// </summary>
        public event FinalizationApprovedDelegate FinalizationApproved;

        #region Commands

        public RelayCommand LoadTemplateImageCommand { get; private set; }

        public RelayCommand DropPageImageCommand { get; private set; }

        public RelayCommand SelectAllElementsCommand { get; private set; }

        public RelayCommand ValidateTemplateCommand { get; private set; }

        public RelayCommand CopyElementsCommand { get; private set; }

        public RelayCommand PasteElementsCommand { get; private set; }

        public RelayCommand FitPageWidthCommand { get; private set; }

        public RelayCommand FitPageHeightCommand { get; private set; }

        public RelayCommand ZoomInCommand { get; private set; }

        public RelayCommand ZoomOutCommand { get; private set; }

        public RelayCommand ZoomOriginalCommand { get; private set; }

        public RelayCommand UndoCommand { get; private set; }

        public RelayCommand RedoCommand { get; private set; }

        public RelayCommand AlignLeftCommand { get; private set; }

        public RelayCommand AlignRightCommand { get; private set; }

        public RelayCommand AlignTopCommand { get; private set; }

        public RelayCommand AlignBottomCommand { get; private set; }

        public RelayCommand ApplyFormattingCommand { get; private set; }

        public RelayCommand ShrinkElementCommand { get; private set; }

        public RelayCommand MoveElementsHorizontal { get; private set; }

        public RelayCommand MoveElementsVertical { get; private set; }

        public RelayCommand RenameGroupCommand { get; private set; }

        public RelayCommand ShowPropertiesCommand { get; private set; }

        public RelayCommand HidePropertiesCommand { get; private set; }

        #endregion

        #endregion

        /// <summary>
        /// Unselect all elements
        /// </summary>
        public void ClearSelection()
        {
            foreach (BaseQuestionViewModel element in this.PageQuestions)
            {
                element.IsSelected = false;
            }
        }

        /// <summary>
        /// Adds group of questions to the template
        /// </summary>
        /// <param name="questions">Questions to add</param>
        public void AddQuestions(IEnumerable<BaseQuestionViewModel> questions)
        {
            foreach (BaseQuestionViewModel question in questions)
            {
                this.OnAddQuestion(question, false);
            }
        }

        /// <summary>
        /// Add new element via selection rectangle
        /// </summary>
        /// <param name="area">Element area</param>
        public void AddQuestion(Rect area)
        {
            string nextName = NamingManager.GetNextAvailableElementName(this.PageQuestions);

            BaseQuestionViewModel newQuestion = null;

            if (this.IsAddingChoiceBox)
            {
                newQuestion = new ChoiceBoxViewModel(nextName, area, this, null);
                this.IsAddingChoiceBox = false;
            }
            else if(this.IsAddingGrid)
            {
                newQuestion = new GridViewModel(nextName, area, this);
                this.IsAddingGrid = false;
            }
            else if (this.IsAddingBarcode)
            {
                newQuestion = new BarcodeViewModel(nextName, area, this);
                this.IsAddingBarcode = false;
            }

            this.OnAddQuestion(newQuestion, true);
            newQuestion.IsSelected = true;

            ActionTracker.TrackAction(new AddElementsAction(new[] { newQuestion }, this.PageQuestions));
        }

        /// <summary>
        /// Selects all elements
        /// </summary>
        private void OnSelectAllElements()
        {
            this.ClearSelection();
            foreach (BaseQuestionViewModel element in this.PageQuestions)
            {
                element.IsSelected = true;
            }
        }

        /// <summary>
        /// Loads main template image and calculates zoom koefficient
        /// </summary>
        private void OnLoadTemplateImage()
        {
            string imagePath = DialogManager.ShowOpenImageDialog();
            if (imagePath == null)
            {
                return;
            }

            this.LoadTemplateImageFromFile(imagePath);
        }

        /// <summary>
        /// Loads template image located by specified path
        /// </summary>
        /// <param name="path">Path to image</param>
        public bool LoadTemplateImageFromFile(string path)
        {
            double monitorWidth, monitorHeight;
            ResolutionUtility.GetMonitorResolution(out monitorWidth, out monitorHeight);

            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                DialogManager.ShowErrorDialog("Failed to load template image by path: " + path);
                return false;
            }

            this.ImageSizeInBytes = fileInfo.Length;
            this.TemplateImageName = fileInfo.Name;
            this.ImageFileFormat = fileInfo.Extension;

            if (!ResolutionUtility.CheckImageSize(fileInfo))
            {
                return false;
            }

            this.TemplateImage = this.LoadImageNoLock(path);

            if (this.TemplateImage.PixelWidth < 1200 || this.TemplateImage.PixelHeight < 1700)
            {
                DialogManager.ShowImageSizeWarning();
            }

            ZoomKoefficient = monitorWidth / this.TemplateImage.PixelWidth < 1
                ? monitorWidth / this.TemplateImage.PixelWidth
                : 1;

            this.OnPropertyChanged(nameof(this.PageScale));
            this.UpdateTemplateCreationStage();

            if (this.PageQuestions.Any())
            {
                this.IsDirty = true;
                this.CanValidate = true;
            }

            return true;
        }

        /// <summary>
        /// Load image without lock
        /// </summary>
        /// <param name="path">Path to the image</param>
        /// <returns>Loaded bitmap image</returns>
        private BitmapImage LoadImageNoLock(string path)
        {
            var bitmap = new BitmapImage();
            var stream = File.OpenRead(path);

            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.EndInit();

            stream.Close();
            stream.Dispose();

            return bitmap;
        }

        /// <summary>
        /// Removes selected elements
        /// </summary>
        private void OnRemoveElement()
        {
            BaseQuestionViewModel[] removedElements = new BaseQuestionViewModel[this.SelectedElements.Count];
            int index = 0;

            foreach (BaseQuestionViewModel selectedItem in this.SelectedElements)
            {
                this.PageQuestions.Remove(selectedItem);
                removedElements[index++] = selectedItem;
            }

            this.SelectedElements.Clear();

            ActionTracker.TrackAction(new RemoveElementsAction(removedElements, this.PageQuestions));

            this.OnPropertyChanged(nameof(this.PropertiesContext));
            this.IsDirty = true;
            this.CanValidate = true;
        }

        /// <summary>
        /// Add element to page elements collection
        /// </summary>
        /// <param name="question">Element to add</param>
        /// <param name="markAsDirty">Mark template as dirty after adding question</param>
        private void OnAddQuestion(BaseQuestionViewModel question, bool markAsDirty)
        {
            // subscribe to property changed to track selection changes
            question.PropertyChanged += this.ElementPropertyChanged;
            this.PageQuestions.Add(question);

            if (markAsDirty)
            {
                this.UpdateTemplateCreationStage();
                this.IsDirty = true;
                this.CanValidate = true;
            }
        }

        /// <summary>
        /// Handles questions selection logic
        /// </summary>
        /// <param name="sender">Question with changed property</param>
        /// <param name="e">The event args</param>
        private void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                BaseQuestionViewModel element = (BaseQuestionViewModel) sender;

                if (element.IsSelected)
                {
                    this.SelectedElements.Add(element);
                }
                else
                {
                    this.SelectedElements.Remove(element);
                }

                this.OnPropertyChanged(nameof(this.PropertiesContext));
            }
            else if (e.PropertyName == nameof(ChoiceBoxViewModel.SelectedMapping) ||
                     e.PropertyName == nameof(BaseQuestionViewModel.Name) ||
                     e.PropertyName == nameof(ChoiceBoxViewModel.BubblesCount) ||
                     e.PropertyName == nameof(GridViewModel.SelectedMapping) ||
                     e.PropertyName == nameof(GridViewModel.OptionsCount) ||
                     e.PropertyName == nameof(GridViewModel.SectionsCount) ||
                     e.PropertyName == nameof(GridViewModel.Width) ||
                     e.PropertyName == nameof(GridViewModel.Height) ||
                     e.PropertyName == nameof(GridViewModel.Top) ||
                     e.PropertyName == nameof(GridViewModel.Left))
            {
                this.IsDirty = true;
                this.CanValidate = true;
            }
        }

        /// <summary>
        /// Initialize commands
        /// </summary>
        private void InitCommands()
        {
            this.RemoveElementCommand = new RelayCommand(x => this.OnRemoveElement(), x => this.SelectedElements.Any());

            this.LoadTemplateImageCommand = new RelayCommand(x => this.OnLoadTemplateImage());
            this.DropPageImageCommand = new RelayCommand(x => this.LoadTemplateImageFromFile((string) x));
            this.SelectAllElementsCommand = new RelayCommand(x => this.OnSelectAllElements(), x => this.PageQuestions.Any());

            this.ValidateTemplateCommand = new RelayCommand(x => this.OnValidateTemplate(), x => this.CanValidate);

            this.CopyElementsCommand = new RelayCommand(x => this.OnCopyQuestions(), x => this.SelectedElements.Any());

            this.PasteElementsCommand = new RelayCommand(x =>
            {
                if (x == null)
                    x = new Point(30, 30);
                this.OnPasteQuestions((Point) x);
            }, x => this.copiedQuestionsBuffer.Any());

            this.FitPageWidthCommand = new RelayCommand(x => this.OnFitPageWidth((double)x));
            this.FitPageHeightCommand = new RelayCommand(x => this.OnFitPageHeight((Size)x));
            this.ZoomInCommand = new RelayCommand(x => this.ZoomLevel = Math.Min(this.ZoomLevel + 0.1, 4));
            this.ZoomOutCommand = new RelayCommand(x => this.ZoomLevel = Math.Max(this.ZoomLevel - 0.1, 0.1));
            this.ZoomOriginalCommand = new RelayCommand(x => this.ZoomLevel = 1);

            this.UndoCommand = new RelayCommand(o => ActionTracker.Undo(1), o => ActionTracker.CanUndo());
            this.RedoCommand = new RelayCommand(o => ActionTracker.Redo(1), o => ActionTracker.CanRedo());

            this.AlignBottomCommand = new RelayCommand(x => AlignmentHelper.AlignBottom(this.SelectedElements), x => this.SelectedElements.Count > 1);
            this.AlignTopCommand = new RelayCommand(x => AlignmentHelper.AlignTop(this.SelectedElements), x => this.SelectedElements.Count > 1);
            this.AlignRightCommand = new RelayCommand(x => AlignmentHelper.AlignRight(this.SelectedElements), x => this.SelectedElements.Count > 1);
            this.AlignLeftCommand = new RelayCommand(x => AlignmentHelper.AlignLeft(this.SelectedElements), x => this.SelectedElements.Count > 1);

            this.ApplyFormattingCommand = new RelayCommand(x => this.OnApplyFormatting(), x => this.SelectedElements.Count == 1);
            this.ShrinkElementCommand = new RelayCommand(x => this.OnShrinkQuestionCommand(), x => this.CanShrink());

            this.MoveElementsHorizontal = new RelayCommand(x => this.OnMoveElementsHorizontal((double)x), x => this.SelectedElements.Count > 0);
            this.MoveElementsVertical = new RelayCommand(x => this.OnMoveElementsVertical((double)x), x => this.SelectedElements.Count > 0);

            this.RenameGroupCommand = new RelayCommand(x => new GroupRenameViewModel(this.SelectedElements), x => this.SelectedElements.Count > 1);

            this.ShowPropertiesCommand = new RelayCommand(x =>
            {
                this.ShowPropertiesPanel = true;
                this.PropertiesPanelWidth = this.savedPropertiesWidth;
            });
            this.HidePropertiesCommand = new RelayCommand(x =>
            {
                this.ShowPropertiesPanel = false;
                this.savedPropertiesWidth = this.PropertiesPanelWidth;
                this.PropertiesPanelWidth = PropertiesPanelClosedWidth;
            });
        }

        /// <summary>
        /// Determines whether shrink command can be executed
        /// </summary>
        /// <returns>Bool value indicating whether shrink command can be executed</returns>
        private bool CanShrink()
        {
            if(this.SelectedElements.Count > 0 && !this.SelectedElements.All(x=> x is BarcodeViewModel))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Shrink selected elements
        /// </summary>
        private void OnShrinkQuestionCommand()
        {
            List<BaseQuestionViewModel> copiesBefore = new List<BaseQuestionViewModel>();
            List<BaseQuestionViewModel> copiesAfter = new List<BaseQuestionViewModel>();

            foreach (BaseQuestionViewModel element in this.SelectedElements)
            {
                copiesBefore.Add(element.CreateCopy());
                element.Shrink();
                copiesAfter.Add(element.CreateCopy());
            }

            ActionTracker.TrackShrink(this.SelectedElements.ToList(), copiesBefore, copiesAfter);
        }

        /// <summary>
        /// Move question horizontally
        /// </summary>
        /// <param name="distance">Distance to move elements</param>
        private void OnMoveElementsHorizontal(double distance)
        {
            foreach (BaseQuestionViewModel element in this.SelectedElements)
            {
                element.Left += distance;
            }
        }

        /// <summary>
        /// Move question vertically
        /// </summary>
        /// <param name="distance">Distance to move elements</param>
        private void OnMoveElementsVertical(double distance)
        {
            foreach (BaseQuestionViewModel element in this.SelectedElements)
            {
                element.Top += distance;
            }
        }

        /// <summary>
        /// Apply selected question's style to all other questions
        /// </summary>
        private void OnApplyFormatting()
        {
            List<BaseQuestionViewModel> copiesBefore = new List<BaseQuestionViewModel>();

            BaseQuestionViewModel ethalon = this.SelectedElements[0];
            var itemsToChange = new List<BaseQuestionViewModel>();

            if (ethalon is ChoiceBoxViewModel)
            {
                itemsToChange = this.PageQuestions.Where(x => x is ChoiceBoxViewModel && x != ethalon).ToList();

                for (int i = 0; i < itemsToChange.Count; i++)
                {
                    copiesBefore.Add(itemsToChange[i].CreateCopy());
                    ((ChoiceBoxViewModel)itemsToChange[i]).ApplyStyle((ChoiceBoxViewModel)ethalon);
                }
            }
            else if (ethalon is GridViewModel)
            {
                itemsToChange = this.PageQuestions.Where(x => x is GridViewModel && x != ethalon).ToList();

                for (int i = 0; i < itemsToChange.Count; i++)
                {
                    copiesBefore.Add(itemsToChange[i].CreateCopy());
                    ((GridViewModel)itemsToChange[i]).ApplyStyle((GridViewModel)ethalon);
                }
            }
            else if (ethalon is BarcodeViewModel)
            {
                itemsToChange = this.PageQuestions.Where(x => x is BarcodeViewModel && x != ethalon).ToList();
                for (int i = 0; i < itemsToChange.Count; i++)
                {
                    copiesBefore.Add(itemsToChange[i].CreateCopy());
                    ((BarcodeViewModel)itemsToChange[i]).ApplyStyle((BarcodeViewModel)ethalon);
                }
            }

            //ActionTracker.TrackApplyFormatting(itemsToChange, copiesBefore, ethalon);
        }

        /// <summary>
        /// Validates template by perfoming correction and finalization
        /// </summary>
        private async void OnValidateTemplate()
        {
            this.FinalizationComplete = false;
            this.CorrectionComplete = false;

            await this.RunCorrection();

            if (this.PageQuestions.Any(x => x.IsValid == false))
            {
                CorrectionErrorsViewModel vm = new CorrectionErrorsViewModel();
                if (!vm.ForceValidation)
                {
                    return;
                }

                // if force validation, mark correction as completed and continue
                this.CorrectionComplete = true;
            }

            if (this.CorrectionComplete)
            {
                await this.FinilizeTemplateAsync();
            }

            // if validation is fully complete, disable validation until any changes to template are made
            if (this.FinalizationComplete)
            {
                this.CanValidate = false;
            }
        }

        /// <summary>
        /// Perform async template correction and check results
        /// </summary>
        /// <returns></returns>
        private async Task RunCorrection()
        {
            await this.CorrectTemplateAsync();

            if (string.IsNullOrEmpty(this.TemplateId) || this.PageQuestions.Any(x => x.IsValid == false))
            {
                this.CorrectionComplete = false;
                this.ValidationFailed = true;
            }
        }

        /// <summary>
        /// Runs template correction
        /// </summary>
        private async Task CorrectTemplateAsync()
        {
            BusyIndicatorManager.Enable();

            string templateData = TemplateSerializer.TemplateToJson(this);
            byte[] imageData = ImageProcessor.CompressImage(this.TemplateImage, this.ImageSizeInBytes);
            string additionalPars = string.Empty;

            try
            {
                TemplateViewModel correctedTemplate = await Task.Run(() => CoreApi.CorrectTemplate(
                    this.TemplateImageName,
                    imageData,
                    templateData,
                    this.WasUploaded,
                    additionalPars));

                this.ClearSelection();
                this.PageQuestions.Clear();

                this.AddQuestions(correctedTemplate.PageQuestions);
                this.TemplateId = correctedTemplate.TemplateId;

                double koeff = this.TemplateImage.PixelWidth / correctedTemplate.PageWidth;
                ZoomKoefficient *= koeff;
                this.OnPropertyChanged(nameof(this.PageScale));

                this.PageWidth = correctedTemplate.PageWidth;
                this.PageHeight = correctedTemplate.PageHeight;
                this.WasUploaded = true;

                this.CorrectionComplete = true;

                // clean commands after correction complete
                ActionTracker.ClearCommands();
                this.Warnings.Add("Template Correction complete!");
            }
            catch (Exception e)
            {
                DialogManager.ShowErrorDialog(e.Message);
                this.ValidationFailed = true;
            }
            finally
            {
                BusyIndicatorManager.Disable();
            }
        }

        /// <summary>
        /// Runs template finalization
        /// </summary>
        private async Task FinilizeTemplateAsync()
        {
            BusyIndicatorManager.Enable();

            string templateData = TemplateSerializer.TemplateToJson(this);
            string additionalPars = string.Empty;

            try
            {
                string templateName = Path.GetFileNameWithoutExtension(this.TemplateImageName) + ".omrcr";

                FinalizationData finalizationResult = await Task.Run(() => CoreApi.FinalizeTemplate(templateName,
                    Encoding.UTF8.GetBytes(templateData), this.TemplateId, additionalPars));

                this.ProcessFinalizationResponse(finalizationResult);
            }
            catch (Exception e)
            {
                DialogManager.ShowErrorDialog(e.Message);
                this.ValidationFailed = true;
            }
            finally
            {
                BusyIndicatorManager.Disable();
            }
        }

        /// <summary>
        /// Process finalization data and display warnings
        /// </summary>
        /// <param name="response">Response containing finalization data</param>
        private void ProcessFinalizationResponse(FinalizationData response)
        {
            // flag indicates if we should open recognition after finalization
            bool openRecognition = false;

            // parse recognition results for template image
            ObservableCollection<RecognitionResult> recognitionResults = this.ParseAnswers(response.Answers);

            if (recognitionResults.Any(x => !string.IsNullOrEmpty(x.AnswerKey)))
            {
                var finalizationViewModel = new FinalizationResultsViewModel(recognitionResults, this.TemplateImage);
                if (!finalizationViewModel.ProceedToRecognition)
                {
                    this.FinalizationComplete = false;
                    this.ValidationFailed = true;
                    return;
                }
                else
                {
                    openRecognition = true;
                }
            }

            this.FinalizationComplete = true;

            // check warnings
            if (response.Warnings.Length > 0)
            {
                foreach (var responseWarning in response.Warnings)
                {
                    this.Warnings.Add(responseWarning);
                }
            }
            else
            {
                this.Warnings.Add("Template Finalization complete!");
            }

            if (openRecognition)
            {
                if (this.FinalizationApproved != null)
                {
                    this.FinalizationApproved();
                }
            }
        }

        /// <summary>
        /// Adjust zoom level so that image fits page width
        /// </summary>
        /// <param name="viewportWidth">The viewport width</param>
        private void OnFitPageWidth(double viewportWidth)
        {
            // 40 pixels threshold for better visuals
            this.ZoomLevel = (viewportWidth - 40) / (this.PageWidth * ZoomKoefficient);
        }

        /// <summary>
        /// Adjust zoom level so that image fits page height
        /// </summary>
        /// <param name="viewportSize">The viewport size</param>
        private void OnFitPageHeight(Size viewportSize)
        {
            // 20 pixels threshold for better visuals
            this.ZoomLevel = (viewportSize.Height - 20) / (this.PageHeight * ZoomKoefficient);
        }

        /// <summary>
        /// Copy selected questions
        /// </summary>
        private void OnCopyQuestions()
        {
            this.copiedQuestionsBuffer.Clear();

            foreach (BaseQuestionViewModel element in this.SelectedElements)
            {
                this.copiedQuestionsBuffer.Add(element.CreateCopy());
            }
        }

        /// <summary>
        /// Paste questions in certain position
        /// </summary>
        /// <param name="position">Position to paste questions</param>
        private void OnPasteQuestions(Point position)
        {
            this.ClearSelection();

            // for the case when mouse position was out of UI change start position
            if (position.X < 0 || position.Y < 0)
            {
                position.X = position.Y = 30;
            }

            // calculate delta between topmost item and paste position
            BaseQuestionViewModel topmostItem = this.copiedQuestionsBuffer.OrderByDescending(x => x.Top).Last();
            double deltaTop = topmostItem.Top - position.Y;
            double deltaLeft = topmostItem.Left - position.X;

            // buffer for undo/redo
            BaseQuestionViewModel[] copiedElements = new BaseQuestionViewModel[this.copiedQuestionsBuffer.Count];

            for (int i = 0; i < this.copiedQuestionsBuffer.Count; i++)
            {
                // create new copy and update name and position
                BaseQuestionViewModel copy = this.copiedQuestionsBuffer[i].CreateCopy();
                copy.Name = NamingManager.GetNextAvailableElementName(this.PageQuestions);
                copy.Top -= deltaTop;
                copy.Left -= deltaLeft;

                this.OnAddQuestion(copy, true);
                copy.IsSelected = true;

                copiedElements[i] = copy;
            }

            ActionTracker.TrackAction(new AddElementsAction(copiedElements, this.PageQuestions));
        }
    }
}
