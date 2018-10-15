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
    /// Contains help messages and maps messages to workflow stages
    /// </summary>
    public static class HelpManager
    {
        #region Template Creation Messages

        private static readonly string NoImageMessage =
            "To start creating template load good quality, high resolution image you are planning to print.";

        private static readonly string NoElementsMessage =
            "Next is to add questions on page: click Add Choice Box or Add Grid button to place new elements over questions on form.";

        private static readonly string WorkWithElementMessage =
            "Bubbles area should be covered completely. Note that it is not necessary to cover scrupulously OMR element over bubbles.";

        private static readonly string NoValidationMessage =
            "To continue press Validate Template button so that template you created will be checked. Make sure to mark out all questions on form!";

        private static readonly string ValidatedWithNoErrors =
            "Template succesfully validated and has no issues! Click Recognize button to start recognizing filled OMR forms.";

        private static readonly string ValidatedWithErrors =
            "Issues occurred during Template Validation! Unfixed problems may affect the recognition quality. Try to fix them and repeat Validation.";


        #endregion

        #region Recognition Messages

        private static readonly string NoImagesMessage =
            "Start with selecting images that you want to recognize.";

        private static readonly string GotImagesToRecognize =
            "Press Recognize All Images, Recognize Selected Image or Cloud icon on the image preview to start recognition.";

        private static readonly string GotResultsToExportMessage =
            "Recognition successful! Check that results are correct and save them by using Export buttons.";

        private static readonly string RecognitionErrorMessage =
            "One of the images was not recognized correctly! Please check that you are using correct template for the image.";

        #endregion


        /// <summary>
        /// Returns hint message corresponding to the provided template creation stage
        /// </summary>
        /// <param name="stage">Workflow stage</param>
        /// <returns>Corresponding hint message</returns>
        public static string GetTemplateMessageByStage(TemplateCreationStages stage)
        {
            switch (stage)
            {
                case TemplateCreationStages.NoImage:
                    return NoImageMessage;
                case TemplateCreationStages.NoElements:
                    return NoElementsMessage;
                case TemplateCreationStages.WorkWithElements:
                    return WorkWithElementMessage;
                case TemplateCreationStages.NoValidation:
                    return NoValidationMessage;
                case TemplateCreationStages.ValidatedWithErrors:
                    return ValidatedWithErrors;
                case TemplateCreationStages.ValidatedWithNoErrors:
                    return ValidatedWithNoErrors;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Returns hint message corresponding to the provided result processing stage
        /// </summary>
        /// <param name="stage">Result processing stage</param>
        /// <returns>Corresponding hint message</returns>
        public static string GetResultsMessageByStage(ResultProcessingStages stage)
        {
            switch (stage)
            {
                case ResultProcessingStages.NoImages:
                    return NoImagesMessage;
                case ResultProcessingStages.GotImagesToRecognize:
                    return GotImagesToRecognize;
                case ResultProcessingStages.GotResultsToExport:
                    return GotResultsToExportMessage;
                case ResultProcessingStages.RecognitionError:
                    return RecognitionErrorMessage;
                default:
                    return string.Empty;
            }
        }
    }
}
