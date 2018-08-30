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
namespace Aspose.OMR.Client.Utility
{
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Manages cloud storage files tracking and cleaning during work session
    /// </summary>
    public static class CloudStorageManager
    {
        /// <summary>
        /// List containing names of the tracked uploaded file
        /// </summary>
        private static List<string> uploadedFilesList = new List<string>();

        /// <summary>
        /// Gets the amount of tracked uploaded files
        /// </summary>
        public static int UploadedFilesCount
        {
            get { return uploadedFilesList.Count; }
        }

        /// <summary>
        /// Track name of the file that was uploaded
        /// </summary>
        /// <param name="fileName">Full file name with extension</param>
        public static void TrackFileUpload(string fileName)
        {
            if (!uploadedFilesList.Contains(fileName))
            {
                uploadedFilesList.Add(fileName);
            }
        }

        /// <summary>
        /// Remove from storage all files that were uploaded during session
        /// </summary>
        public static void CleanUpStorage()
        {
            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += (sender, args) =>
            {
                CoreApi.StorageCleanUp(uploadedFilesList);
            };

            worker.RunWorkerCompleted += (sender, args) =>
            {
                // clean up list
                uploadedFilesList.Clear();
                BusyIndicatorManager.Disable();

                // close app
                System.Windows.Application.Current.Shutdown();
            };

            BusyIndicatorManager.Enable();
            worker.RunWorkerAsync();
        }
    }
}
