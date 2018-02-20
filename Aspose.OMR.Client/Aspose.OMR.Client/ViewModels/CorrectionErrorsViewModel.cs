/*
 * Copyright (c) 2017 Aspose Pty Ltd. All Rights Reserved.
 *
 * Licensed under the MIT (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *       https://github.com/asposecloud/Aspose.OMR-Cloud/blob/master/LICENSE
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace Aspose.OMR.Client.ViewModels
{
    using Utility;
    using Views;

    /// <summary>
    /// View model for view with correction issues
    /// </summary>
    public class CorrectionErrorsViewModel : ViewModelBase
    {
        /// <summary>
        /// Correction errors view
        /// </summary>
        private CorrectionErrorsView view;

        /// <summary>
        /// Initializes a new instance of the <see cref="CorrectionErrorsViewModel"/> class.
        /// </summary>
        public CorrectionErrorsViewModel()
        {
            this.FixErrorsCommand = new RelayCommand(x => this.OnFixErrorsCommand());
            this.IgnoreAndContinueCommand = new RelayCommand(x => this.OnIgnoreAndContinueCommand());

            this.view = new CorrectionErrorsView(this);
            this.view.ShowDialog();
        }

        /// <summary>
        /// Gets the value indicating whether we should ignore invalid bubbles and force continue validation
        /// </summary>
        public bool ForceValidation { get; private set; }

        /// <summary>
        /// Gets or sets the FixErrorsCommand command
        /// </summary>
        public RelayCommand FixErrorsCommand { get; private set; }

        /// <summary>
        /// Gets or sets the IgnoreAndContinueCommand command
        /// </summary>
        public RelayCommand IgnoreAndContinueCommand { get; private set; }

        /// <summary>
        /// Use chose to attempt to fix errors, set flag value and close view
        /// </summary>
        private void OnFixErrorsCommand()
        {
            this.ForceValidation = false;
            this.view.Close();
        }

        /// <summary>
        /// Use chose to ignore issues, set flag value and close view
        /// </summary>
        private void OnIgnoreAndContinueCommand()
        {
            this.ForceValidation = true;
            this.view.Close();
        }
    }
}
