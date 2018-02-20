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
    using System.IO;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Contains utility methods to determine user monitor resolution
    /// </summary>
    public static class ResolutionUtility
    {
        /// <summary>
        /// Computes monitor resolution
        /// </summary>
        /// <param name="monitorWidth">Monitor width</param>
        /// <param name="monitorHeight">Monitor height</param>
        public static void GetMonitorResolution(out double monitorWidth, out double monitorHeight)
        {
            PresentationSource sourse = PresentationSource.FromVisual(Application.Current.MainWindow);
            Matrix matrix = sourse.CompositionTarget.TransformToDevice;

            double thisDpiWidthFactor = matrix.M11;
            double thisDpiHeightFactor = matrix.M22;

            monitorWidth = SystemParameters.PrimaryScreenWidth * thisDpiWidthFactor;
            monitorHeight = SystemParameters.PrimaryScreenHeight * thisDpiHeightFactor;
        }

        /// <summary>
        /// Check image size
        /// </summary>
        /// <param name="file">File info</param>
        /// <returns>True if image size is valid</returns>
        public static bool CheckImageSize(FileInfo file)
        {
            if (file.Length >= 1024 * 1024 * 50)
            {
                DialogManager.ShowErrorDialog("File is too large. Please use images less than 50MB in size.");
                return false;
            }

            if (file.Length < 1024 * 100)
            {
                DialogManager.ShowWarningDialog("File is too small. We recommend using images larger than 100KB.");
                return true;
            }

            return true;
        }
    }
}
