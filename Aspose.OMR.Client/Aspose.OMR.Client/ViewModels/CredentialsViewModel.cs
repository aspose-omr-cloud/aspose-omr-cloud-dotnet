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
    using System.Text;
    using Utility;
    using Views;

    /// <summary>
    /// View model for user credentials dialog
    /// </summary>
    public class CredentialsViewModel : ViewModelBase
    {
        /// <summary>
        /// Custom mapping window
        /// </summary>
        private readonly CredentialsView view;

        /// <summary>
        /// App sid value
        /// </summary>
        private string appSidText;

        /// <summary>
        /// App key value
        /// </summary>
        private string appKeyText;

        /// <summary>
        /// Initializes a new instance of the <see cref="CredentialsViewModel"/> class
        /// </summary>
        public CredentialsViewModel()
        {
            // init commands
            this.SaveCommand = new RelayCommand(x => this.OnSaveCommand());
            this.CancelCommand = new RelayCommand(x => this.OnCancelCommand());

            // init view
            this.view = new CredentialsView(this);

            // try loading keys
            byte[] appKey = UserSettingsUtility.LoadAppKey();
            byte[] appSid = UserSettingsUtility.LoadAppSid();

            // decrypt loaded keys if any
            if (appKey != null && appSid != null)
            {
                this.AppSidText = Encoding.UTF8.GetString(SecurityUtility.Decrpyt(appSid));
                this.AppKeyText = Encoding.UTF8.GetString(SecurityUtility.Decrpyt(appKey));
            }

            // display view
            this.view.ShowDialog();
        }

        public RelayCommand SaveCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }

        /// <summary>
        /// Gets or sets the app sid string
        /// </summary>
        public string AppSidText
        {
            get { return this.appSidText; }
            set
            {
                this.appSidText = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the app key string
        /// </summary>
        public string AppKeyText
        {
            get { return this.appKeyText; }
            set
            {
                this.appKeyText = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Saves keys
        /// </summary>
        private void OnSaveCommand()
        {
            // check app sid length
            if (this.AppSidText.Length != 36)
            {
                DialogManager.ShowErrorDialog("Incorrect APP SID length! APP SID should be 36 symbols length.");
                return;
            }

            // check app key length
            if (this.AppKeyText.Length != 32)
            {
                DialogManager.ShowErrorDialog("Incorrect APP KEY length! APP KEY should be 32 symbols length.");
                return;
            }

            // update keys to provide new values
            CoreApi.UpdateKeys(this.AppKeyText, this.AppSidText);

            // encrypt keys and save in settings
            byte[] encryptedAppKey = SecurityUtility.Encrypt(Encoding.UTF8.GetBytes(this.AppKeyText));
            byte[] encryptedAppSid = SecurityUtility.Encrypt(Encoding.UTF8.GetBytes(this.AppSidText));
            UserSettingsUtility.SaveCredentials(encryptedAppKey, encryptedAppSid);

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
