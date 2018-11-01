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
    /// Defined stages of results processing process
    /// </summary>
    public enum ResultProcessingStages
    {
        /// <summary>
        /// Initial state, no images has been selected for the recognition
        /// </summary>
        NoImages,

        /// <summary>
        /// Images selected, but no recognition function called
        /// </summary>
        GotImagesToRecognize,

        /// <summary>
        /// Recognition successful, results ready for export
        /// </summary>
        GotResultsToExport,

        /// <summary>
        /// Recognition resulted with an error
        /// </summary>
        RecognitionError,
    }
}
