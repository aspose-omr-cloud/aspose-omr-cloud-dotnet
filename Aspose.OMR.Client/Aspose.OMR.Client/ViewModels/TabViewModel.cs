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
    /// Common class for all tab viewmodels
    /// </summary>
    public abstract class TabViewModel : ViewModelBase
    {
        /// <summary>
        /// Tab name showed to the user
        /// </summary>
        private string tabName;

        /// <summary>
        /// Indicates that view model tab is selected
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Indicates whether template has unsaved changes
        /// </summary>
        private bool isDirty;

        /// <summary>
        /// Gets or sets command to remove selected element in tab
        /// </summary>
        public RelayCommand RemoveElementCommand { get; protected set; }

        /// <summary>
        /// Gets or sets the tab name
        /// </summary>
        public string TabName
        {
            get { return this.tabName; }
            set
            {
                this.tabName = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether tab has any unsaved changes
        /// </summary>
        public bool IsDirty
        {
            get { return this.isDirty; }
            set
            {
                this.isDirty = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether tab is selected
        /// </summary>
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.OnPropertyChanged();
            }
        }

        protected ObservableCollection<RecognitionResult> ParseAnswers(string results)
        {
            var recognitionResults = new ObservableCollection<RecognitionResult>();

            string[] answersInfo = results.Split('\n');
            foreach (string answerInfo in answersInfo)
            {
                // sort out empty strings that may be recieved from core
                if (string.IsNullOrEmpty(answerInfo))
                {
                    continue;
                }

                int index = answerInfo.LastIndexOf(':');

                if (index != answerInfo.Length - 1)
                {
                    // standart string like "question1:A"
                    recognitionResults.Add(new RecognitionResult(answerInfo.Substring(0, index), answerInfo.Substring(index + 1)));
                }
                else
                {
                    // no answer string like "question1:"
                    recognitionResults.Add(new RecognitionResult(answerInfo.Substring(0, index), string.Empty));
                }
            }

            return recognitionResults;
        }
    }
}
