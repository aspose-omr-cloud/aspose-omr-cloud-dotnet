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
namespace Aspose.OMR.Client.Controls
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// Thumb used for moving controls
    /// </summary>
    public class MoveThumb : Thumb
    {
        /// <summary>
        /// Pixel threshold around border for better visuals
        /// </summary>
        private static readonly int BorderThreshold = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveThumb"/> class.
        /// </summary>
        public MoveThumb()
        {
            // subscribe to drag event
            this.DragDelta += this.MoveThumbDragDelta;
        }

        /// <summary>
        /// Handle thumb drag
        /// </summary>
        private void MoveThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            // dragging content - question or bubble
            DependencyObject content;

            // find dragging content - layout different for question and bubbles
            Adorner adorner = VisualTreeHelper.GetParent(this) as Adorner;
            if (adorner != null)
            {
                content = adorner.AdornedElement;
            }
            else
            {
                var grid = VisualTreeHelper.GetParent(this);
                content = VisualTreeHelper.GetParent(grid);
            }

            // presenter that holds omr item
            ContentPresenter presenter = (ContentPresenter) VisualTreeHelper.GetParent(content);

            // parent canvas
            CustomCanvas canvas = (CustomCanvas) VisualTreeHelper.GetParent(presenter);

            // get all selected elements
            List<ContentPresenter> presenters = ControlHelper.GetSelectedChildPresenters(canvas);

            double deltaHorizontal = e.HorizontalChange;
            double deltaVertical = e.VerticalChange;

            // check out of bounds for each item
            for (int i = 0; i < presenters.Count; i++)
            {
                if (Canvas.GetLeft(presenters[i]) + deltaHorizontal > canvas.ActualWidth - presenters[i].ActualWidth - BorderThreshold
                    ||
                    Canvas.GetLeft(presenters[i]) + deltaHorizontal < BorderThreshold)
                {
                    deltaHorizontal = 0;
                }

                if (Canvas.GetTop(presenters[i]) + deltaVertical > canvas.ActualHeight - presenters[i].ActualHeight - BorderThreshold
                    ||
                    Canvas.GetTop(presenters[i]) + deltaVertical < BorderThreshold)
                {
                    deltaVertical = 0;
                }
            }

            // apply change for each item
            for (int i = 0; i < presenters.Count; i++)
            {
                Canvas.SetLeft(presenters[i], Canvas.GetLeft(presenters[i]) + deltaHorizontal);
                Canvas.SetTop(presenters[i], Canvas.GetTop(presenters[i]) + deltaVertical);
            }

            // canvas.InvalidateMeasure();
            e.Handled = true;
        }
    }
}
