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
    /// Defined stages of template creation process
    /// </summary>
    public enum TemplateCreationStages
    {
        /// <summary>
        /// No template image loaded, initial stage
        /// </summary>
        NoImage,

        /// <summary>
        /// No elements added on template page
        /// </summary>
        NoElements,

        /// <summary>
        /// User started placing elements (less then 3)
        /// </summary>
        WorkWithElements,

        /// <summary>
        /// User placed 3 or more elements, not validated template
        /// </summary>
        NoValidation,

        /// <summary>
        /// Validated template, but faced errors
        /// </summary>
        ValidatedWithErrors,

        /// <summary>
        /// Validated template with no errors
        /// </summary>
        ValidatedWithNoErrors,
    }
}
