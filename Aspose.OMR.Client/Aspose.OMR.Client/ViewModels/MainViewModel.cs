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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using Controls;
    using UndoRedo;
    using Utility;
    using Views;

    /// <summary>
    /// View model for main view
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Collection of tab items
        /// </summary>
        private ObservableCollection<TabViewModel> tabViewModels;

        /// <summary>
        /// Currently selected tab
        /// </summary>
        private TabViewModel selectedTab;

        /// <summary>
        /// Flag indicating if busy indicator should be enabled
        /// </summary>
        private bool isBusy;

        /// <summary>
        /// Indicates visibility of start panel with quick access commands
        /// </summary>
        private Visibility startPanelVisibility;

        /// <summary>
        /// Indicates if drop functionality is enabled
        /// </summary>
        private bool canDrop;

        /// <summary>
        /// Busy indication message
        /// </summary>
        private string busyIndicationMessage;

        /// <summary>
        /// Recently opened template files collection
        /// </summary>
        private ObservableCollection<string> recentFiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            BusyIndicatorManager.EnabledChanged += this.BusyChanged;
            this.tabViewModels = new ObservableCollection<TabViewModel>();

            this.CanDrop = true;
            this.StartPanelVisibility = Visibility.Visible;
            this.RecentFiles = new ObservableCollection<string>(UserSettingsUtility.LoadRecentFiles());

            this.InitCommands();
        }

        #region Commands

        public RelayCommand NewTemplateCommand { get; set; }

        public RelayCommand LoadTemplateCommand { get; set; }

        public RelayCommand LoadRecentTemlateCommand { get; set; }
        
        public RelayCommand GenerateTemplateCommand { get; set; }

        public RelayCommand DropTemplateCommand { get; set; }

        public RelayCommand SaveTemplateCommand { get; private set; }

        public RelayCommand SaveAsTemplateCommand { get; private set; }

        public RelayCommand StartRecognitionCommand { get; set; }

        public RelayCommand CloseTabCommand { get; set; }

        public RelayCommand UndoCommand { get; set; }

        public RelayCommand RedoCommand { get; set; }

        public RelayCommand LoadTemplateImageCommand { get; set; }

        public RelayCommand CopyCommand { get; set; }

        public RelayCommand PasteCommand { get; set; }

        public RelayCommand DeleteCommand { get; set; }

        public RelayCommand SelectAllCommand { get; set; }

        public RelayCommand ApplyFormattingCommand { get; private set; }

        public RelayCommand ShrinkElementCommand { get; private set; }

        public RelayCommand RenameGroupCommand { get; private set; }

        public RelayCommand ZoomInCommand { get; set; }

        public RelayCommand ZoomOutCommand { get; set; }

        public RelayCommand ZoomOriginalCommand { get; set; }

        public RelayCommand ExitCommand { get; set; }

        public RelayCommand ShowCredentialsSettingsCommand { get; set; }

        public RelayCommand ShowAboutCommand { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets the recent files list
        /// </summary>
        public ObservableCollection<string> RecentFiles
        {
            get { return this.recentFiles; }
            set
            {
                this.recentFiles = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the visibility of start panel
        /// </summary>
        public Visibility StartPanelVisibility
        {
            get { return this.startPanelVisibility; }
            private set
            {
                this.startPanelVisibility = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets a value indicating whether busy indicator should be enabled
        /// </summary>
        public bool IsBusy
        {
            get { return this.isBusy; }
            private set
            {
                this.isBusy = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets a value indicating whether recognition tab is available
        /// </summary>
        public bool IsRecognitionAvailable
        {
            get
            {
                return this.SelectedTab is TemplateViewModel;
            }
        }

        /// <summary>
        /// Gets or sets the selected tab
        /// </summary>
        public TabViewModel SelectedTab
        {
            get { return this.selectedTab; }
            set
            {
                // unselect old tab
                if (this.selectedTab != null)
                {
                    this.selectedTab.IsSelected = false;
                }

                // select new tab
                this.selectedTab = value;
                if (value != null)
                {
                    this.selectedTab.IsSelected = true;
                    this.StartPanelVisibility = Visibility.Collapsed;
                    this.CanDrop = false;
                }
                else
                {
                    this.StartPanelVisibility = Visibility.Visible;
                    this.CanDrop = true;
                }

                this.OnPropertyChanged();
                this.OnPropertyChanged("IsRecognitionAvailable");
            }
        }

        /// <summary>
        /// Gets a value indicating whether template finalization was complete with no warnings
        /// </summary>
        public bool FinalizationComplete
        {
            get
            {
                var templateViewModel = this.TabViewModels.FirstOrDefault(x => x is TemplateViewModel);
                if (templateViewModel != null)
                {
                    return ((TemplateViewModel) templateViewModel).FinalizationComplete;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the collection of TabViewModels
        /// </summary>
        public ObservableCollection<TabViewModel> TabViewModels
        {
            get { return this.tabViewModels; }
            private set
            {
                this.tabViewModels = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether drop can be performed on main window
        /// </summary>
        public bool CanDrop
        {
            get { return this.canDrop; }
            set
            {
                this.canDrop = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets message displayed during busy indication
        /// </summary>
        public string BusyIndicationMessage
        {
            get { return this.busyIndicationMessage; }
            set
            {
                this.busyIndicationMessage = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Busy indicator manager busy state changed
        /// </summary>
        /// <param name="isEnabled">Indicates if busy state is enabled</param>
        /// <param name="message">Busy indication message</param>
        private void BusyChanged(bool isEnabled, string message)
        {
            this.IsBusy = isEnabled;
            this.BusyIndicationMessage = message;
        }

        /// <summary>
        /// Initialize commands
        /// </summary>
        private void InitCommands()
        {
            this.NewTemplateCommand = new RelayCommand(x => this.OnNewTemplate());
            this.LoadTemplateCommand = new RelayCommand(x => this.OnLoadTemplate());
            this.LoadRecentTemlateCommand = new RelayCommand(x => this.OnLoadRecentTemplate((string) x), x => this.RecentFiles.Any());
            this.GenerateTemplateCommand = new RelayCommand(x => this.OnGenerateTemplate());

            this.DropTemplateCommand = new RelayCommand(x => this.LoadTemplateFromFile((string)x));

            this.SaveTemplateCommand = new RelayCommand(x => this.OnSaveTemplate(), x => this.CanSaveTemplate());
            this.SaveAsTemplateCommand = new RelayCommand(x => this.OnSaveAsTemplate(), x => this.CanSaveAsTemplate());

            this.StartRecognitionCommand = new RelayCommand(x => this.OnStartRecognition());

            this.CloseTabCommand = new RelayCommand(x => this.OnCloseTab());

            this.UndoCommand = new RelayCommand(x => this.OnUndoCommand(), x => this.CanUndo());
            this.RedoCommand = new RelayCommand(x => this.OnRedoCommand(), x => this.CanRedo());

            this.LoadTemplateImageCommand = new RelayCommand(x => this.LoadTemplateImage(), x => this.CanLoadTemplateImage());

            this.CopyCommand = new RelayCommand(x => this.OnCopyCommand(), x => this.CanCopy());
            this.PasteCommand = new RelayCommand(x => this.OnPasteCommand(x), x => this.CanPaste());
            this.DeleteCommand = new RelayCommand(x => this.OnDeleteCommand(), x => this.CanDelete());

            this.SelectAllCommand = new RelayCommand(x => this.OnSelectAllCommand(), x => this.CanSelectAll());
            this.ZoomInCommand = new RelayCommand(x => this.OnZoomInCommand());
            this.ZoomOutCommand = new RelayCommand(x => this.OnZoomOutCommand());
            this.ZoomOriginalCommand = new RelayCommand(x => this.OnZoomOriginalCommand());

            this.ApplyFormattingCommand = new RelayCommand(x => this.OnApplyFormattingCommand(), x => this.CanApplyFormatting());
            this.RenameGroupCommand = new RelayCommand(x => this.OnRenameGroupCommand(), x => this.CanRenameGoup());
            this.ShrinkElementCommand = new RelayCommand(x => this.OnShrinkElementCommand(), x => this.CanShrinkElement());

            this.ShowCredentialsSettingsCommand = new RelayCommand(x => new CredentialsViewModel());
            this.ShowAboutCommand = new RelayCommand(x => new AboutView().ShowDialog());

            this.ExitCommand = new RelayCommand(x => this.Exit());
        }

        #region Shortcuts commands

        private void OnSaveTemplate()
        {
            string loadedPath = (this.SelectedTab as TemplateViewModel).LoadedPath;

            if (!string.IsNullOrEmpty(loadedPath))
            {
                // if template was loaded from file, save to same location
                this.SaveTemplateByPath(loadedPath);
            }
            else
            {
                // if saving new template, ask for save location
                this.OnSaveAsTemplate();
            }
        }

        private bool CanSaveTemplate()
        {
            if (this.SelectedTab is TemplateViewModel)
            {
                TemplateViewModel template = this.SelectedTab as TemplateViewModel;
                return template.PageQuestions.Any() && template.IsDirty;
            }

            return false;
        }

        /// <summary>
        /// Saves As.. template
        /// </summary>
        private void OnSaveAsTemplate()
        {
            string savePath = DialogManager.ShowSaveTemplateDialog();
            if (savePath == null)
            {
                return;
            }

            RecentMenuManager.AddFileNameToRecentList(this.RecentFiles, savePath);
            this.SaveTemplateByPath(savePath);
        }

        private bool CanSaveAsTemplate()
        {
            if (this.SelectedTab is TemplateViewModel)
            {
                TemplateViewModel template = this.SelectedTab as TemplateViewModel;
                return template.PageQuestions.Any();
            }

            return false;
        }

        /// <summary>
        /// Performs template save routine by specified path
        /// </summary>
        /// <param name="path">Path to save template</param>
        private void SaveTemplateByPath(string path)
        {
            if (this.SelectedTab is TemplateViewModel)
            {
                TemplateViewModel template = this.SelectedTab as TemplateViewModel;
                template.TemplateName = Path.GetFileNameWithoutExtension(path);

                // save .omr template data
                string jsonRes = TemplateSerializer.TemplateToJson(template);
                File.WriteAllText(path, jsonRes);

                path = path.Replace(".omr", ".jpg");

                // save template image
                ImageProcessor.SaveTemplateImage(template.TemplateImage, path);

                template.IsDirty = false;
                template.LoadedPath = path;
            }
        }

        private void LoadTemplateImage()
        {
            ((TemplateViewModel) this.SelectedTab).LoadTemplateImageCommand.Execute(null);
        }

        private bool CanLoadTemplateImage()
        {
            var selectedTemplate = this.SelectedTab as TemplateViewModel;
            if (selectedTemplate != null && selectedTemplate.LoadTemplateImageCommand.CanExecute(null))
            {
                return true;
            }

            return false;
        }

        private void OnRedoCommand()
        {
            ((TemplateViewModel) this.SelectedTab).RedoCommand.Execute(null);
        }

        private bool CanRedo()
        {
            var selectedTemplate = this.SelectedTab as TemplateViewModel;
            if (selectedTemplate != null && selectedTemplate.RedoCommand.CanExecute(null))
            {
                return true;
            }

            return false;
        }

        private void OnUndoCommand()
        {
            ((TemplateViewModel) this.SelectedTab).UndoCommand.Execute(null);
        }

        private bool CanUndo()
        {
            var selectedTemplate = this.SelectedTab as TemplateViewModel;
            if (selectedTemplate != null && selectedTemplate.UndoCommand.CanExecute(null))
            {
                return true;
            }

            return false;
        }

        private void OnZoomOutCommand()
        {
            var selectedTemplate = this.SelectedTab as TemplateViewModel;
            if (selectedTemplate != null && selectedTemplate.ZoomOutCommand.CanExecute(null))
            {
                selectedTemplate.ZoomOutCommand.Execute(null);
            }
        }

        private void OnZoomInCommand()
        {
            var selectedTemplate = this.SelectedTab as TemplateViewModel;
            if (selectedTemplate != null && selectedTemplate.ZoomInCommand.CanExecute(null))
            {
                selectedTemplate.ZoomInCommand.Execute(null);
            }
        }

        private void OnZoomOriginalCommand()
        {
            var selectedTemplate = this.SelectedTab as TemplateViewModel;
            if (selectedTemplate != null && selectedTemplate.ZoomOriginalCommand.CanExecute(null))
            {
                selectedTemplate.ZoomOriginalCommand.Execute(null);
            }
        }

        private void OnApplyFormattingCommand()
        {
            var selectedTemplate = this.SelectedTab as TemplateViewModel;
            if (selectedTemplate != null && selectedTemplate.ApplyFormattingCommand.CanExecute(null))
            {
                selectedTemplate.ApplyFormattingCommand.Execute(null);
            }
        }

        private bool CanApplyFormatting()
        {
            var selectedTemplate = this.SelectedTab as TemplateViewModel;
            if (selectedTemplate != null && selectedTemplate.ApplyFormattingCommand.CanExecute(null))
            {
                return true;
            }

            return false;
        }

        private void OnRenameGroupCommand()
        {
            var selectedTemplate = this.SelectedTab as TemplateViewModel;
            if (selectedTemplate != null && selectedTemplate.RenameGroupCommand.CanExecute(null))
            {
                selectedTemplate.RenameGroupCommand.Execute(null);
            }
        }

        private bool CanRenameGoup()
        {
            var selectedTemplate = this.SelectedTab as TemplateViewModel;
            if (selectedTemplate != null && selectedTemplate.RenameGroupCommand.CanExecute(null))
            {
                return true;
            }

            return false;
        }

        private void OnShrinkElementCommand()
        {
            var selectedTemplate = this.SelectedTab as TemplateViewModel;
            if (selectedTemplate != null && selectedTemplate.ShrinkElementCommand.CanExecute(null))
            {
                selectedTemplate.ShrinkElementCommand.Execute(null);
            }
        }

        private bool CanShrinkElement()
        {
            var selectedTemplate = this.SelectedTab as TemplateViewModel;
            if (selectedTemplate != null && selectedTemplate.ShrinkElementCommand.CanExecute(null))
            {
                return true;
            }

            return false;
        }

        private void OnSelectAllCommand()
        {
            ((TemplateViewModel) this.SelectedTab).SelectAllElementsCommand.Execute(null);
        }

        private bool CanSelectAll()
        {
            var selectedTemplate = this.SelectedTab as TemplateViewModel;
            if (selectedTemplate != null && selectedTemplate.SelectAllElementsCommand.CanExecute(null))
            {
                return true;
            }

            return false;
        }

        private void OnCopyCommand()
        {
            ((TemplateViewModel) this.SelectedTab).CopyElementsCommand.Execute(null);
        }

        private bool CanCopy()
        {
            var selectedTemplate = this.SelectedTab as TemplateViewModel;
            if (selectedTemplate != null && selectedTemplate.CopyElementsCommand.CanExecute(null))
            {
                return true;
            }

            return false;
        }

        private void OnPasteCommand(object control)
        {
            // find parent canvas
            UIElement uiItem = (UIElement)control;
            var canvas = uiItem.FindChild<CustomCanvas>(Properties.Resources.RootCanvasName);

            // get mouse position to use for paste questions
            Point mousePos = Mouse.GetPosition(canvas);

            ((TemplateViewModel) this.SelectedTab).PasteElementsCommand.Execute(mousePos);
        }

        private bool CanPaste()
        {
            var selectedTemplate = this.SelectedTab as TemplateViewModel;
            if (selectedTemplate != null && selectedTemplate.PasteElementsCommand.CanExecute(null))
            {
                return true;
            }

            return false;
        }

        private void OnDeleteCommand()
        {
            this.SelectedTab.RemoveElementCommand.Execute(null);
        }

        private bool CanDelete()
        {
            if (this.SelectedTab != null)
            {
                return this.SelectedTab.RemoveElementCommand.CanExecute(null);
            }

            return false;
        }

        #endregion

        /// <summary>
        /// Run all needed logic on app shutdown
        /// </summary>
        /// <returns>True if all tabs closed and ready for shutdown, false is process was cancelled</returns>
        public bool CleanUpOnClosing()
        {
            RecentMenuManager.UpdateRecentFiles(this.RecentFiles.ToList());

            // Manually close all tabs on app shut down to provoce asking for save
            while (this.SelectedTab != null)
            {
                bool res = this.OnCloseTab();

                // in case closing was cancelled, break
                if (!res)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        private void Exit()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Show template generation view and process results returned by request
        /// </summary>
        private void OnGenerateTemplate()
        {
            // return if there is active tabs and action was cancelled
            if (!this.CloseActiveTemplateTab())
            {
                return;
            }

            var viewModel = new TemplateGeneratorViewModel();
            viewModel.ViewClosed += this.GenerationCompleted;
        }

        /// <summary>
        /// Template generation completed handler
        /// </summary>
        /// <param name="generationResult">Generation result</param>
        private void GenerationCompleted(TemplateGenerationContent generationResult)
        {
            if (generationResult == null)
            {
                return;
            }

            TemplateViewModel templateViewModel = generationResult.Template;
            templateViewModel.TemplateImage = TemplateSerializer.DecompressImage(Convert.ToBase64String(generationResult.ImageData));
            templateViewModel.TemplateImageName = templateViewModel.TemplateName + ".jpg";
            templateViewModel.IsGeneratedTemplate = true;
            templateViewModel.IsDirty = true;

            this.AddTab(templateViewModel);
        }

        /// <summary>
        /// Loads recent template file from disk
        /// </summary>
        private void OnLoadRecentTemplate(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            if (!File.Exists(path))
            {
                DialogManager.ShowErrorDialog("Failed to find file " + path);
                RecentMenuManager.RemoveFileFromRecentList(this.RecentFiles, path);
                return;
            }

            this.LoadTemplateFromFile(path);
        }

        /// <summary>
        /// Loads template from disk
        /// </summary>
        private void OnLoadTemplate()
        {
            string templatePath = DialogManager.ShowOpenTemplateDialog();
            if (string.IsNullOrEmpty(templatePath))
            {
                return;
            }

            this.LoadTemplateFromFile(templatePath);
        }

        /// <summary>
        /// Loads template by specified path
        /// </summary>
        /// <param name="file">Path to template file</param>
        private void LoadTemplateFromFile(string file)
        {
            // return if there is active tabs and action was cancelled
            if (!this.CloseActiveTemplateTab())
            {
                return;
            }

            if (!File.Exists(file))
            {
                DialogManager.ShowErrorDialog("Failed to find file " + file + ".");
                return;
            }

            string directory = Path.GetDirectoryName(file);
            if (string.IsNullOrEmpty(directory))
            {
                DialogManager.ShowErrorDialog("Failed to load template image.");
                return;
            }

            string templateName = Path.GetFileNameWithoutExtension(file);

            // find files with image extension and template name in the same directory
            List<string> imageFiles = DialogManager.GetImageFilesFromDirectory(directory)
                .Where(x => Path.GetFileNameWithoutExtension(x).Equals(templateName)).ToList();

            if (imageFiles.Count < 1)
            {
                DialogManager.ShowErrorDialog("Failed to find template image.");
                return;
            }

            if (imageFiles.Count > 1)
            {
                DialogManager.ShowErrorDialog("Failed to load template image. Found several files with name: " +
                                              Path.GetFileNameWithoutExtension(file) + ".");
                return;
            }

            // load and deserialize template data
            string jsonString = File.ReadAllText(file);
            TemplateViewModel templateViewModel = TemplateSerializer.JsonToTemplate(jsonString);

            // if no name in template, use name of the file
            if (string.IsNullOrEmpty(templateViewModel.TemplateName))
            {
                templateViewModel.TemplateName = Path.GetFileNameWithoutExtension(file);
            }

            // load image and check if it was loaded
            bool imageLoaded = templateViewModel.LoadTemplateImageFromFile(imageFiles[0]);
            if (!imageLoaded)
            {
                return;
            }

            templateViewModel.LoadedPath = file;
            templateViewModel.IsDirty = false;
            templateViewModel.FinalizationApproved += this.OnStartRecognition;

            this.CloseActiveTemplateTab();

            this.AddTab(templateViewModel);

            RecentMenuManager.AddFileNameToRecentList(this.RecentFiles, file);
        }

        /// <summary>
        /// Closes active template vm tabs if there is any
        /// </summary>
        /// <returns>False if tab closing was cancelled by user, true otherwise</returns>
        private bool CloseActiveTemplateTab()
        {
            if (this.TabViewModels.Any(x => x is TemplateViewModel))
            {
                var templateVm = this.TabViewModels.First(x => x is TemplateViewModel) as TemplateViewModel;
                this.SelectedTab = templateVm;
                return this.OnCloseTab();
            }

            return true;
        }

        /// <summary>
        /// Attempts to close active tab
        /// </summary>
        /// <returns>False if tab closing was cancelled by user, true otherwise</returns>
        private bool OnCloseTab()
        {
            if (this.SelectedTab.IsDirty)
            {
                if (this.SelectedTab is TemplateViewModel)
                {
                    MessageBoxResult dialogResult = DialogManager.ShowConfirmDirtyClosingDialog(
                            "This template has unsaved changes. Do you want to save them?");

                    if (dialogResult == MessageBoxResult.Cancel)
                    {
                        // cancel closing
                        return false;
                    }
                    else if (dialogResult == MessageBoxResult.Yes)
                    {
                        // save
                        this.OnSaveTemplate();
                    }
                }
                else if (this.SelectedTab is ResultsViewModel)
                {
                }
            }

            if (this.SelectedTab is TemplateViewModel)
            {
                ActionTracker.ClearCommands();
            }

            this.TabViewModels.Remove(this.SelectedTab);
            if (this.TabViewModels.Count > 0)
            {
                this.SelectedTab = this.TabViewModels.Last();
            }
            else
            {
                this.SelectedTab = null;
            }

            return true;
        }

        /// <summary>
        /// Create new template
        /// </summary>
        private void OnNewTemplate()
        {
            // return if there is active tabs and action was cancelled
            if (!this.CloseActiveTemplateTab())
            {
                return;
            }

            var templateVm = new TemplateViewModel(false, string.Empty);
            templateVm.TemplateName = "New Template";
            templateVm.FinalizationApproved += this.OnStartRecognition;

            // reinit zoom koefficient
            TemplateViewModel.ZoomKoefficient = 1;

            this.AddTab(templateVm);
        }

        /// <summary>
        /// Opens recognition view
        /// </summary>
        private void OnStartRecognition()
        {
            if (!this.FinalizationComplete)
            {
                DialogManager.ShowErrorDialog("There is no finalized template to work with!");
                return;
            }

            // find opened results tab and select it
            TabViewModel resultsTab = this.TabViewModels.FirstOrDefault(x => x is ResultsViewModel);
            if (resultsTab != null)
            {
                this.SelectedTab = resultsTab;
                return;
            }

            string tabName = "Recognition";

            // find template and get needed info
            TemplateViewModel template = (TemplateViewModel) this.TabViewModels.First(x => x is TemplateViewModel);
            string templateId = template.TemplateId;
            bool isGenerated = template.IsGeneratedTemplate;

            ResultsViewModel resultsVm = new ResultsViewModel(tabName, templateId, isGenerated);
            this.AddTab(resultsVm);
        }
        
        /// <summary>
        /// Adds new tab and selects it. Subscribe to events if any
        /// </summary>
        /// <param name="tab">Tab to add and select</param>
        private void AddTab(TabViewModel tab)
        {
            this.TabViewModels.Add(tab);
            this.SelectedTab = tab;

            if (tab is TemplateViewModel)
            {
                TemplateViewModel templateViewModel = tab as TemplateViewModel;

                // subscriptions for template view model events
                templateViewModel.FinalizationApproved += this.OnStartRecognition;
            }
        }
    }
}
