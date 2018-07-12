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
namespace Aspose.OMR.Client.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Media;
    using UndoRedo;
    using ViewModels;

    /// <summary>
    /// Thumb used for moving controls
    /// </summary>
    public class MoveThumb : Thumb
    {
        /// <summary>
        /// Pixel threshold around border for better visuals
        /// </summary>
        private static readonly int BorderThreshold = 0;

        private double startTop;
        private double startLeft;
        private double bubbleStartTop;
        private double bubbleStartLeft;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveThumb"/> class.
        /// </summary>
        public MoveThumb()
        {
            // subscribe to drag event
            this.DragDelta += this.MoveThumbDragDelta;

            // handle drag start and end events to calculate delta and track action for undo/redo support
            this.DragStarted += this.MoveThumbDragStarted;
            this.DragCompleted += this.MoveThumbDragCompleted;
        }

        /// <summary>
        /// Thumb drag started event handler
        /// </summary>
        private void MoveThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            if (this.DataContext is BaseQuestionViewModel)
            {
                // move question
                BaseQuestionViewModel questionViewModel = (BaseQuestionViewModel)this.DataContext;
                this.startTop = questionViewModel.Top;
                this.startLeft = questionViewModel.Left;
            }
            else if (this.DataContext is BubbleViewModel)
            {
                // move bubble
                BubbleViewModel bubbleViewModel = (BubbleViewModel)this.DataContext;
                this.bubbleStartTop = bubbleViewModel.Top;
                this.bubbleStartLeft = bubbleViewModel.Left;
            }
        }

        /// <summary>
        /// Thumb drag completed event handler
        /// </summary>
        private void MoveThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (this.DataContext is BaseQuestionViewModel)
            {
                // resize question
                BaseQuestionViewModel questionViewModel = (BaseQuestionViewModel)this.DataContext;

                double deltaTop = this.startTop - questionViewModel.Top;
                double deltaLeft = this.startLeft - questionViewModel.Left;

                if (Math.Abs(deltaTop) > 0.01 || Math.Abs(deltaLeft) > 0.01)
                {
                    if (questionViewModel.ParentTemplate != null)
                    {
                        // moving choicebox question, check all other selected questions
                        List<BaseQuestionViewModel> selectedItems = questionViewModel.ParentTemplate.SelectedElements.ToList();
                        ActionTracker.TrackChangeQuestionsPosition(selectedItems, deltaTop, deltaLeft, 1, 1);
                    }
                    else if (questionViewModel is ChoiceBoxViewModel)
                    {
                        if (((ChoiceBoxViewModel)questionViewModel).ParentGrid != null)
                        {
                            // moving choicebox child of grid question
                            ActionTracker.TrackChangeQuestionsPosition(new List<BaseQuestionViewModel>() { questionViewModel }, deltaTop, deltaLeft, 1, 1);
                        }
                    }
                }
            }
            else if (this.DataContext is BubbleViewModel)
            {
                // resize bubble
                BubbleViewModel bubbleViewModel = (BubbleViewModel)this.DataContext;

                double deltaTop = this.bubbleStartTop - bubbleViewModel.Top;
                double deltaLeft = this.bubbleStartLeft - bubbleViewModel.Left;

                if (Math.Abs(deltaTop) > 0.01 || Math.Abs(deltaLeft) > 0.01)
                {
                    ActionTracker.TrackChangeBubble(new List<BubbleViewModel> { bubbleViewModel }, deltaTop, deltaLeft, 1, 1);
                }
            }
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

            ContentPresenter presenter;

            if (content is ContentPresenter)
            {
                presenter = (ContentPresenter) content;
            }
            else
            {
                // presenter that holds omr item
                presenter = (ContentPresenter)VisualTreeHelper.GetParent(content);
            }

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
