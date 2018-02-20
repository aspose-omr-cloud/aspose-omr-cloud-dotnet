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
    using System.Collections.ObjectModel;
    using ViewModels;

    /// <summary>
    /// Performs alignment operations
    /// </summary>
    public static class AlignmentHelper
    {
        /// <summary>
        /// Right alignment
        /// </summary>
        /// <param name="items">Items to align</param>
        public static void AlignRight(ObservableCollection<BaseQuestionViewModel> items)
        {
            double maxX = items[0].Left + items[0].Width;

            foreach (BaseQuestionViewModel element in items)
            {
                if (element.Left + element.Width > maxX)
                {
                    maxX = element.Left + element.Width;
                }
            }

            foreach (BaseQuestionViewModel element in items)
            {
                element.Left = maxX - element.Width;
            }
        }

        /// <summary>
        /// Left alignment
        /// </summary>
        /// <param name="items">Items to align</param>
        public static void AlignLeft(ObservableCollection<BaseQuestionViewModel> items)
        {
            double minX = items[0].Left;

            foreach (BaseQuestionViewModel element in items)
            {
                if (minX > element.Left)
                {
                    minX = element.Left;
                }
            }

            foreach (BaseQuestionViewModel element in items)
            {
                element.Left = minX;
            }
        }

        /// <summary>
        /// Top alignment
        /// </summary>
        /// <param name="items">Items to align</param>
        public static void AlignTop(ObservableCollection<BaseQuestionViewModel> items)
        {
            double minY = items[0].Top;

            foreach (BaseQuestionViewModel element in items)
            {
                if (minY > element.Top)
                {
                    minY = element.Top;
                }
            }

            foreach (BaseQuestionViewModel element in items)
            {
                element.Top = minY;
            }
        }

        /// <summary>
        /// Bottom alignment
        /// </summary>
        /// <param name="items">Items to align</param>
        public static void AlignBottom(ObservableCollection<BaseQuestionViewModel> items)
        {
            double maxY = items[0].Top + items[0].Height;

            foreach (BaseQuestionViewModel element in items)
            {
                if (element.Top + element.Height > maxY)
                {
                    maxY = element.Top + element.Height;
                }
            }

            foreach (BaseQuestionViewModel element in items)
            {
                element.Top = maxY - element.Height;
            }
        }
    }
}
