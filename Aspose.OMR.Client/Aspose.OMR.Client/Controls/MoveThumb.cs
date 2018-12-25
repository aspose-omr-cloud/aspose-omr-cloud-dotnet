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
    using System.Windows.Shapes;
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

        /// <summary>
        /// Distance of elements snapping
        /// </summary>
        private static readonly int snapDeltaPixels = 10;

        /// <summary>
        /// Additional snap line 'tail' length for better visuals
        /// </summary>
        private static readonly int snapLineTail = 20;

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
                // move question
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

                        if (questionViewModel.ParentTemplate.GotSnapLines)
                        {
                            questionViewModel.ParentTemplate.CleanSnapLines();
                        }
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

                if (questionViewModel.ParentTemplate != null)
                {
                    if (questionViewModel.ParentTemplate.GotSnapLines)
                    {
                        questionViewModel.ParentTemplate.CleanSnapLines();
                    }
                }
            }
            else if (this.DataContext is BubbleViewModel)
            {
                // move bubble
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

            if (canvas.DataContext is TemplateViewModel)
            {
                if (presenters.Count > 1)
                {
                    SnapGroup(presenters, canvas);
                }
                else if (presenters.Count == 1)
                {
                    SnapElement(presenters[0], canvas);
                }
            }

            // canvas.InvalidateMeasure();
            e.Handled = true;
        }

        /// <summary>
        /// Snap group of elements 
        /// </summary>
        /// <param name="movedGroup">Group of elements to snap</param>
        /// <param name="canvas">Main canvas holding all items</param>
        private void SnapGroup(List<ContentPresenter> movedGroup, CustomCanvas canvas)
        {
            // get bounding box Rect of snap group
            Rect boundingBox = GetBoundingBox(movedGroup);

            TemplateViewModel context = (TemplateViewModel)canvas.DataContext;
            List<ContentPresenter> canvasItems = ControlHelper.GetUnselectedChildPresenters(canvas).ToList();

            // get list of close items according to snapDeltaPixels value
            List<Line> resultingSnapLines = new List<Line>();
            List<ContentPresenter> closeByLeft = canvasItems.Where(x => Math.Abs(Canvas.GetLeft(x) - boundingBox.Left) <= snapDeltaPixels).ToList();
            List<ContentPresenter> closeByTop = canvasItems.Where(x => Math.Abs(Canvas.GetTop(x) - boundingBox.Top) <= snapDeltaPixels).ToList();

            if (closeByLeft.Count > 0)
            {
                // closes by left position and then by top
                ContentPresenter closest = closeByLeft.OrderBy(x => Math.Abs(Canvas.GetLeft(x) - boundingBox.Left))
                    .ThenBy(x => Math.Abs(Canvas.GetTop(x) - boundingBox.Top)).First();

                double targetLeft = Canvas.GetLeft(closest);
                double targetTop = Canvas.GetTop(closest);

                // snap each item of the group according to calculated distance to the target
                double moveDelta = boundingBox.Left - targetLeft;
                movedGroup.ForEach(x => Canvas.SetLeft(x, Canvas.GetLeft(x) - moveDelta));

                // add lowest item height to the line length
                double itemHeight = targetTop > boundingBox.Top ? closest.Height : boundingBox.Height;

                // draw snap line and add to the list
                Line line = new Line();
                line.X1 = line.X2 = targetLeft;
                line.Y1 = Math.Max(Math.Min(boundingBox.Top, targetTop) - snapLineTail, 0);
                line.Y2 = Math.Max(boundingBox.Top, targetTop) + itemHeight + snapLineTail;

                resultingSnapLines.Add(line);
            }

            if (closeByTop.Count > 0)
            {
                // closes by top position and then by left
                ContentPresenter closest = closeByTop.OrderBy(x => Math.Abs(Canvas.GetTop(x) - boundingBox.Top))
                    .ThenBy(x => Math.Abs(Canvas.GetLeft(x) - boundingBox.Left)).First();

                double targetLeft = Canvas.GetLeft(closest);
                double targetTop = Canvas.GetTop(closest);

                // snap each item of the group according to calculated distance to the target
                double moveDelta = boundingBox.Top - targetTop;
                movedGroup.ForEach(x => Canvas.SetTop(x, Canvas.GetTop(x) - moveDelta));

                double itemWidth = targetLeft > boundingBox.Left ? closest.Width : boundingBox.Width;

                // draw snap line and add to the list
                Line line = new Line();
                line.Y1 = line.Y2 = targetTop;
                line.X1 = Math.Max(Math.Min(boundingBox.Left, targetLeft) - snapLineTail, 0);
                line.X2 = Math.Max(boundingBox.Left, targetLeft) + itemWidth + snapLineTail;

                resultingSnapLines.Add(line);
            }

            if (context.GotSnapLines)
            {
                context.CleanSnapLines();
            }

            // draw snap lines
            foreach (Line snapLine in resultingSnapLines)
            {
                context.AddLine(snapLine);
            }
        }

        /// <summary>
        /// Snap single element to close elements on canvas
        /// </summary>
        /// <param name="movedItem">Item to snap</param>
        /// <param name="canvas">Main canvas holding all items</param>
        private void SnapElement(ContentPresenter movedItem, CustomCanvas canvas)
        {
            double itemLeft = Canvas.GetLeft(movedItem);
            double itemTop = Canvas.GetTop(movedItem);
            TemplateViewModel context = (TemplateViewModel)canvas.DataContext;

            // get list of close items according to snapDeltaPixels value
            List<ContentPresenter> presenters = ControlHelper.GetUnselectedChildPresenters(canvas).ToList();
            List<ContentPresenter> closeByLeft = presenters.Where(x => Math.Abs(Canvas.GetLeft(x) - itemLeft) <= snapDeltaPixels).ToList();
            List<ContentPresenter> closeByTop = presenters.Where(x => Math.Abs(Canvas.GetTop(x) - itemTop) <= snapDeltaPixels).ToList();

            List<Line> resultingSnapLines = new List<Line>();
            if (closeByLeft.Count > 0)
            {
                // closest by left position and then by top
                ContentPresenter closest = closeByLeft.OrderBy(x => Math.Abs(Canvas.GetLeft(x) - itemLeft))
                    .ThenBy(x => Math.Abs(Canvas.GetTop(x) - itemTop)).First();

                // move snapped item to desired position
                double targetLeft = Canvas.GetLeft(closest);
                double targetTop = Canvas.GetTop(closest);
                Canvas.SetLeft(movedItem, targetLeft);

                // add lowest item height to the line length
                double itemHeight = targetTop > itemTop ? closest.Height : movedItem.Height;

                // draw snap line and add to the list
                Line line = new Line();
                line.X1 = line.X2 = targetLeft;
                line.Y1 = Math.Max(Math.Min(itemTop, targetTop) - snapLineTail, 0);
                line.Y2 = Math.Max(itemTop, targetTop) + itemHeight + snapLineTail;

                resultingSnapLines.Add(line);
            }

            if (closeByTop.Count > 0)
            {
                // closes by top position and then by left
                ContentPresenter closest = closeByTop.OrderBy(x => Math.Abs(Canvas.GetTop(x) - itemTop))
                    .ThenBy(x => Math.Abs(Canvas.GetLeft(x) - itemLeft)).First();

                // move snapped item to desired position
                double targetLeft = Canvas.GetLeft(closest);
                double targetTop = Canvas.GetTop(closest);
                Canvas.SetTop(movedItem, targetTop);

                double itemWidth = targetLeft > itemLeft ? closest.Width : movedItem.Width;

                // draw snap line and add to the list
                Line line = new Line();
                line.Y1 = line.Y2 = targetTop;
                line.X1 = Math.Max(Math.Min(itemLeft, targetLeft) - snapLineTail, 0);
                line.X2 = Math.Max(itemLeft, targetLeft) + itemWidth + snapLineTail;

                resultingSnapLines.Add(line);
            }

            if (context.GotSnapLines)
            {
                context.CleanSnapLines();
            }
            
            // draw snap lines
            foreach (Line snapLine in resultingSnapLines)
            {
                context.AddLine(snapLine);
            }
        }

        /// <summary>
        /// Get bounding box Rect of group of presenters
        /// </summary>
        /// <param name="presenters">Group of presenters</param>
        /// <returns>Resulting bounding box Rect</returns>
        private Rect GetBoundingBox(List<ContentPresenter> presenters)
        {
            double top = presenters.Min(x => Canvas.GetTop(x));
            double left = presenters.Min(x => Canvas.GetLeft(x));
            double bottom = presenters.Max(x => Canvas.GetTop(x) + x.Height);
            double right = presenters.Max(x => Canvas.GetLeft(x) + x.Width);

            Rect boundinBox = new Rect(new Point(left, top), new Point(right, bottom));
            return boundinBox;
        }
    }
}
