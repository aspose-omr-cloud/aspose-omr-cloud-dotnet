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
    /// <summary>
    /// Manager handling application busy state changes
    /// </summary>
    public static class BusyIndicatorManager
    {
        /// <summary>
        /// Text displayed during busy indication
        /// </summary>
        private static string busyText;

        /// <summary>
        /// Busy state indicator, true if busy
        /// </summary>
        private static bool busyState;

        /// <summary>
        /// Delegate for enabled changed event
        /// </summary>
        /// <param name="isEnabled">IsEnabled flag</param>
        /// <param name="busyText">Text displayed during busy indication</param>
        public delegate void EnabledChangedDelegate(bool isEnabled, string busyText);

        /// <summary>
        /// Event raised when enabled flag changed
        /// </summary>
        public static event EnabledChangedDelegate EnabledChanged;

        /// <summary>
        /// Enable busy indication
        /// </summary>
        public static void Enable()
        {
            if (string.IsNullOrEmpty(busyText))
            {
                busyText = "Loading...";
            }

            busyState = true;
            EnabledChanged?.Invoke(true, busyText);
        }

        /// <summary>
        /// Update busy indication text
        /// </summary>
        /// <param name="message">New message to display</param>
        public static void UpdateText(string message)
        {
            EnabledChanged?.Invoke(busyState, message);
        }

        /// <summary>
        /// Disable busy indication
        /// </summary>
        public static void Disable()
        {
            busyState = false;
            EnabledChanged?.Invoke(false, string.Empty);
        }
    }
}
