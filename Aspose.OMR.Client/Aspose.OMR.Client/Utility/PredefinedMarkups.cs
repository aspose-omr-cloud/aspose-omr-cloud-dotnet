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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Predefined markups for template generation
    /// </summary>
    public enum PredefinedMarkups
    {
        [Description("Blank Page")]
        [Display(Name = "Blank Page")]
        BlankPage,

        [Description("Used in multiple choice question examinations with check boxes to fill in")]
        [Display(Name = "Bubble Sheet")]
        BubbleSheet,

        [Description("A set of questions with a choice of answers, devised for the purposes of a survey or statistical study")]
        [Display(Name = "Questionnaire")]
        Questionnaire
    }
}
