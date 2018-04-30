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
    using System.Linq;
    using Utility;
    using Views;

    /// <summary>
    /// View model for custom mapping view
    /// </summary>
    public class CustomMappingViewModel : ViewModelBase
    {
        /// <summary>
        /// Custom mapping window
        /// </summary>
        private readonly CustomMappingView view;

        /// <summary>
        /// Custom mapping name, i.e. key
        /// </summary>
        private string customMappingName;

        /// <summary>
        /// Amount of mapping values
        /// </summary>
        private int customMappingItemsCount;

        /// <summary>
        /// Actual mapping values
        /// </summary>
        private ObservableCollection<StringWrapper> mappingValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMappingViewModel"/> class.
        /// </summary>
        public CustomMappingViewModel()
        {
            this.OkCommand = new RelayCommand(x => this.OnOkCommand());
            this.CancelCommand = new RelayCommand(x => this.OnCancelCommand());

            // add default values
            this.customMappingItemsCount = 4;
            this.CustomMappingAdded = false;

            this.MappingValues = new ObservableCollection<StringWrapper>();
            this.MappingValues.Add(new StringWrapper("A"));
            this.MappingValues.Add(new StringWrapper("B"));
            this.MappingValues.Add(new StringWrapper("C"));
            this.MappingValues.Add(new StringWrapper("D"));

            this.view = new CustomMappingView(this);
            this.view.ShowDialog();
        }

        /// <summary>
        /// Gets or sets custom mapping key value, i.e. name of the mapping
        /// </summary>
        public string CustomMappingName
        {
            get { return this.customMappingName; }
            set
            {
                this.customMappingName = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether custom mapping was added
        /// </summary>
        public bool CustomMappingAdded { get; set; }

        /// <summary>
        /// Gets or sets mapping values count
        /// </summary>
        public int CustomMappingItemsCount
        {
            get { return this.customMappingItemsCount; }
            set
            {
                int diff = value - this.customMappingItemsCount;

                if (diff > 0)
                {
                    for (int i = 0; i < diff; i++)
                    {
                        this.MappingValues.Add(new StringWrapper(""));
                    }
                }
                else
                {
                    for (int i = 0; i < Math.Abs(diff); i++)
                    {
                        this.MappingValues.RemoveAt(this.MappingValues.Count - 1);
                    }
                }

                this.customMappingItemsCount = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets mapping values
        /// </summary>
        public ObservableCollection<StringWrapper> MappingValues
        {
            get { return this.mappingValues; }
            set
            {
                this.mappingValues = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the Ok command
        /// </summary>
        public RelayCommand OkCommand { get; private set; }

        /// <summary>
        /// Gets the Cancel command
        /// </summary>
        public RelayCommand CancelCommand { get; private set; }

        /// <summary>
        /// Closes window
        /// </summary>
        private void OnCancelCommand()
        {
            this.CustomMappingAdded = false;
            this.view.Close();
        }

        /// <summary>
        /// Applies changes and closes window
        /// </summary>
        private void OnOkCommand()
        {
            if (string.IsNullOrEmpty(this.CustomMappingName))
            {
                // show error
                DialogManager.ShowErrorDialog("Mapping name cannot be empty!");
                return;
            }

            if (AnswersMappingHelper.CheckMappingExists(this.CustomMappingName))
            {
                // already exists, should be unique
                DialogManager.ShowErrorDialog("Mapping name already exists! Make sure mapping name is unique.");
                return;
            }

            string[] resArray = this.MappingValues.Select(x => x.StringValue).ToArray();
            AnswersMappingHelper.AddCustomMapping(this.CustomMappingName, resArray);

            this.CustomMappingAdded = true;
            this.view.Close();
        }
    }
}
