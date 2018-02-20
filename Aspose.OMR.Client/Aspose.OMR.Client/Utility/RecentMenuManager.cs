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
namespace Aspose.OMR.Client.Utility
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Manages recent menu content
    /// </summary>
    public static class RecentMenuManager
    {
        /// <summary>
        /// Max number of recent entries
        /// </summary>
        private const int MaxEntries = 10;

        /// <summary>
        /// Adds filename to recent items list
        /// </summary>
        /// <param name="recentFiles">Recent files collection</param>
        /// <param name="fileName">Recent file path</param>
        public static void AddFileNameToRecentList(ObservableCollection<string> recentFiles, string fileName)
        {
            if (recentFiles.Contains(fileName))
            {
                recentFiles.Remove(fileName);
            }

            recentFiles.Insert(0, fileName);

            if (recentFiles.Count > MaxEntries)
            {
                // remove last entry
                recentFiles.RemoveAt(recentFiles.Count - 1);
            }
        }

        /// <summary>
        /// Removes file from recent files list
        /// </summary>
        /// <param name="recentFiles">Recent files collection</param>
        /// <param name="fileName">File to remove</param>
        public static void RemoveFileFromRecentList(ObservableCollection<string> recentFiles, string fileName)
        {
            if (recentFiles.Contains(fileName))
            {
                recentFiles.Remove(fileName);
            }
        }

        /// <summary>
        /// Update saved settings with new recent files list
        /// </summary>
        /// <param name="recentFiles">Recent files list</param>
        public static void UpdateRecentFiles(List<string> recentFiles)
        {
            UserSettingsUtility.SaveRecentFiles(recentFiles.ToArray());
        }
    }
}
