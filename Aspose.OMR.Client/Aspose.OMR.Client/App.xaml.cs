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
namespace Aspose.OMR.Client
{
    using System;
    using Utility;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Application.Current.DispatcherUnhandledException += OnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
        }

        private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                DialogManager.ShowErrorDialog("Unhandled error occured. Please contact support on Aspose support forums.\nDetails: " + exception.Message);
            }
            else
            {
                DialogManager.ShowErrorDialog("Unknown error occured. Please contact support on Aspose support forums.");
            }
        }

        private void OnUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            DialogManager.ShowErrorDialog("Unhandled error occured. Please contact support on Aspose support forums.\nDetails: " + e.Exception.Message);
        }
    }
}
