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
    using Views;

    /// <summary>
    /// View model for Group Rename window
    /// </summary>
    public class GroupRenameViewModel : ViewModelBase
    {
        /// <summary>
        /// Group rename window
        /// </summary>
        private readonly GroupRenameView view;

        /// <summary>
        /// Prefix string for new questions
        /// </summary>
        private string namePrefix;

        /// <summary>
        /// Starting index integer for new questions
        /// </summary>
        private int startingIndex;

        /// <summary>
        /// Preview string for new names
        /// </summary>
        private string previewNewNames;

        /// <summary>
        /// Gets or sets the collection of selected questions
        /// </summary>
        private readonly ObservableCollection<BaseQuestionViewModel> selectedQuestions;

        /// <summary>
        /// selected questions count
        /// </summary>
        private int questionsCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupRenameViewModel"/> class
        /// </summary>
        /// <param name="selectedQuestions">Selected questions</param>
        public GroupRenameViewModel(ObservableCollection<BaseQuestionViewModel> selectedQuestions)
        {
            // init commands
            this.SaveCommand = new RelayCommand(x => this.OnSaveCommand());
            this.CancelCommand = new RelayCommand(x => this.OnCancelCommand());

            this.QuestionsCount = selectedQuestions.Count;
            this.selectedQuestions = selectedQuestions;

            this.NamePrefix = "Question";
            this.StartingIndex = 1;

            this.view = new GroupRenameView(this);

            this.view.ShowDialog();
        }

        /// <summary>
        /// Save changes command
        /// </summary>
        public RelayCommand SaveCommand { get; set; }

        /// <summary>
        /// Cancel command
        /// </summary>
        public RelayCommand CancelCommand { get; set; }

        /// <summary>
        /// Gets or sets the selected questions count
        /// </summary>
        public int QuestionsCount
        {
            get { return this.questionsCount; }
            set
            {
                this.questionsCount = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the name prefix
        /// </summary>
        public string NamePrefix
        {
            get { return this.namePrefix; }
            set
            {
                this.namePrefix = value; 
                this.OnPropertyChanged();
                this.UpdateNamePreview();
            }
        }

        /// <summary>
        /// Gets or sets the starting index of questions names
        /// </summary>
        public int StartingIndex
        {
            get { return this.startingIndex; }
            set
            {
                this.startingIndex = value;
                this.OnPropertyChanged();
                this.UpdateNamePreview();
            }
        }

        /// <summary>
        /// Gets or sets the preview string for new names
        /// </summary>
        public string PreviewNewNames
        {
            get { return this.previewNewNames; }
            set
            {
                this.previewNewNames = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Update preview property value
        /// </summary>
        private void UpdateNamePreview()
        {
            this.PreviewNewNames = this.NamePrefix.ToString() + this.StartingIndex
                + " - "
                + this.NamePrefix.ToString() + (this.StartingIndex + this.QuestionsCount - 1).ToString();
        }

        /// <summary>
        /// Apply changes and close
        /// </summary>
        private void OnSaveCommand()
        {
            for (int i = 0; i < this.selectedQuestions.Count; i++)
            {
                this.selectedQuestions[i].Name = this.NamePrefix + (this.StartingIndex + i);
            }

            this.view.Close();
        }

        /// <summary>
        /// Closes view
        /// </summary>
        private void OnCancelCommand()
        {
            this.view.Close();
        }
    }
}
