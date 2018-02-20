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
    /// <summary>
    /// Contains help messages and maps messages to workflow stages
    /// </summary>
    public static class HelpManager
    {
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

        /// <summary>
        /// Returns help message corresponding to the provided stage
        /// </summary>
        /// <param name="stage">Workflow stage</param>
        /// <returns>Corresponding message</returns>
        public static string GetMessageByStage(TemplateCreationStages stage)
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
    }
}
