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
    /// Thumb used for resizing controls
    /// </summary>
    public class ResizeThumb : Thumb
    {
        /// <summary>
        /// Pixel threshold around border for better visuals
        /// </summary>
        private static readonly int BorderThreshold = 0;

        private double startTop;
        private double startLeft;
        private double startWidth;
        private double startHeight;

        private double bubbleStartWidth;
        private double bubbleStartHeight;
        private double bubbleStartTop;
        private double bubbleStartLeft;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizeThumb"/> class.
        /// </summary>
        public ResizeThumb()
        {
            // subscribe to drag event
            this.DragDelta += this.ResizeDragDelta;

            // handle drag start and end events to calculate delta and track action for undo/redo support
            this.DragStarted += this.ResizeThumbDragStarted;
            this.DragCompleted += this.ResizeThumbDragCompleted;
        }

        /// <summary>
        /// Thumb drag started event handler
        /// </summary>
        private void ResizeThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            if (this.DataContext is BaseQuestionViewModel)
            {
                // resize question
                BaseQuestionViewModel questionViewModel = (BaseQuestionViewModel)this.DataContext;

                // remember starting top and left position for measuring delta when drag is completed
                this.startWidth = questionViewModel.Width;
                this.startHeight = questionViewModel.Height;
                this.startTop = questionViewModel.Top;
                this.startLeft = questionViewModel.Left;
            }
            else if (this.DataContext is BubbleViewModel)
            {
                // resize bubble
                BubbleViewModel bubbleViewModel = (BubbleViewModel)this.DataContext;
                BubbleViewModel bubble = bubbleViewModel.ParentQuestion.Bubbles[0];

                this.bubbleStartWidth = bubble.Width;
                this.bubbleStartHeight = bubble.Height;
                this.bubbleStartTop = bubble.Top;
                this.bubbleStartLeft = bubble.Left;
            }
        }

        /// <summary>
        /// Thumb drag completed event handler
        /// </summary>
        private void ResizeThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (this.DataContext is BaseQuestionViewModel)
            {
                // resize question
                BaseQuestionViewModel questionViewModel = (BaseQuestionViewModel)this.DataContext;

                double deltaTop = this.startTop - questionViewModel.Top;
                double deltaLeft = this.startLeft - questionViewModel.Left;
                double widthKoef = this.startWidth / questionViewModel.Width;
                double heightKoef = this.startHeight / questionViewModel.Height;

                if (questionViewModel.ParentTemplate != null)
                {
                    List<BaseQuestionViewModel> selectedItems = questionViewModel.ParentTemplate.SelectedElements.ToList();
                    ActionTracker.TrackChangeQuestionsPosition(selectedItems, deltaTop, deltaLeft, widthKoef, heightKoef);
                }
                else if (questionViewModel is ChoiceBoxViewModel)
                {
                    var choiceBoxViewModel = (ChoiceBoxViewModel) questionViewModel;
                    List<BaseQuestionViewModel> selectedItems = choiceBoxViewModel.ParentGrid.ChoiceBoxes
                        .Cast<BaseQuestionViewModel>()
                        .ToList();
                    ActionTracker.TrackChangeQuestionsPosition(selectedItems, deltaTop, deltaLeft, widthKoef, heightKoef);
                }
            }
            else if (this.DataContext is BubbleViewModel)
            {
                // resize bubble
                BubbleViewModel bubbleViewModel = (BubbleViewModel)this.DataContext;
                List<BubbleViewModel> bubbles = bubbleViewModel.ParentQuestion.Bubbles.ToList();

                double deltaTop = this.bubbleStartTop - bubbles[0].Top;
                double deltaLeft = this.bubbleStartLeft - bubbles[0].Left;

                double widthKoef = this.bubbleStartWidth / bubbles[0].Width;
                double heightKoef = this.bubbleStartHeight / bubbles[0].Height;

                ActionTracker.TrackChangeBubble(bubbles, deltaTop, deltaLeft, widthKoef, heightKoef);
            }
        }

        /// <summary>
        /// Occurs as the mouse changes position when a Thumb control has logical focus and mouse capture
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        private void ResizeDragDelta(object sender, DragDeltaEventArgs e)
        {
            // get to resized element
            Grid thumbsGrid = (Grid)VisualTreeHelper.GetParent(this);
            Thumb parentThumb = (Thumb)VisualTreeHelper.GetParent(thumbsGrid);
            Adorner adorner = (Adorner)VisualTreeHelper.GetParent(parentThumb);

            // presenter that holds dragged item
            ContentPresenter presenter = (ContentPresenter) VisualTreeHelper.GetParent(adorner.AdornedElement);

            // parent canvas
            CustomCanvas canvas = (CustomCanvas) VisualTreeHelper.GetParent(presenter);

            // prepare items for resize depending on item level (question or bubble)
            List<ContentPresenter> presenters;
            if (canvas.Name.Equals(Properties.Resources.RootCanvasName))
            {
                // here we resize questions (e.g. choicebox), so take selected items
                presenters = ControlHelper.GetSelectedChildPresenters(canvas);
            }
            else
            {
                // here we resize bubbles, so get neighbor bubbles inside question (always resize bubbles equally)
                presenters = ControlHelper.GetChildPresenters(canvas);
            }

            double minLeft, minTop, minDeltaHorizontal, minDeltaVertical;
            double maxDeltaVertical, maxDeltaHorizontal;

            double maxHorizontalDeltaToChild = double.MaxValue;
            double maxVerticalDeltaToChild = double.MaxValue;

            this.CalculateLimits(canvas, presenters, out minLeft, out minTop, out minDeltaHorizontal, out minDeltaVertical,
                out maxDeltaHorizontal, out maxDeltaVertical);

            minDeltaVertical = Math.Min(minDeltaVertical, maxVerticalDeltaToChild);
            minDeltaHorizontal = Math.Min(minDeltaHorizontal, maxHorizontalDeltaToChild);

            foreach (ContentPresenter element in presenters)
            {
                double newHeight = element.ActualHeight;
                double newWidth = element.ActualWidth;

                double newTop = Canvas.GetTop(element);
                double newLeft = Canvas.GetLeft(element);

                double scale, dragDeltaVertical;
                switch (this.VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        dragDeltaVertical = Math.Max(Math.Min(-e.VerticalChange, minDeltaVertical), -maxDeltaVertical);
                        newHeight = element.ActualHeight - dragDeltaVertical;
                        break;

                    case VerticalAlignment.Top:
                        dragDeltaVertical = Math.Min(Math.Max(-minTop, e.VerticalChange), minDeltaVertical);
                        scale = (element.ActualHeight - dragDeltaVertical) / element.ActualHeight;

                        newTop = newTop - element.Height * (scale - 1);
                        newHeight = element.ActualHeight - dragDeltaVertical;
                        break;
                }

                double dragDeltaHorizontal;
                switch (this.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        dragDeltaHorizontal = Math.Min(Math.Max(-minLeft, e.HorizontalChange), minDeltaHorizontal);
                        scale = (element.ActualWidth - dragDeltaHorizontal) / element.ActualWidth;

                        newLeft = Canvas.GetLeft(element) - element.Width * (scale - 1);
                        newWidth = element.ActualWidth * scale;
                        break;

                    case HorizontalAlignment.Right:
                        dragDeltaHorizontal = Math.Max(Math.Min(-e.HorizontalChange, minDeltaHorizontal), -maxDeltaHorizontal);
                        newWidth = element.ActualWidth - dragDeltaHorizontal;
                        break;
                }

                var childs = this.GetChildElements(element);
                if (childs == null)
                {
                    element.Width = newWidth;
                    element.Height = newHeight;
                    Canvas.SetTop(element, newTop);
                    Canvas.SetLeft(element, newLeft);
                }
                else
                {
                    double scaleH = newHeight / element.ActualHeight;
                    double scaleW = newWidth / element.ActualWidth;

                    if (this.CanTransformChildsRecursive(childs, scaleH, scaleW))
                    {
                        this.TransformChildsRecursive(childs, scaleH, scaleW);

                        element.Width = newWidth;
                        element.Height = newHeight;
                        Canvas.SetTop(element, newTop);
                        Canvas.SetLeft(element, newLeft);
                    }
                }
            }

            e.Handled = true;
        }

        private void TransformChildsRecursive(List<ContentPresenter> childs, double scaleH, double scaleW)
        {
            // second pass, apply transform
            foreach (ContentPresenter child in childs)
            {
                var deeperChilds = this.GetChildElements(child);
                if (deeperChilds != null)
                {
                    this.TransformChildsRecursive(deeperChilds, scaleH, scaleW);
                }

                child.Height = child.ActualHeight * scaleH;
                child.Width = child.ActualWidth * scaleW;

                var top = Canvas.GetTop(child);
                var left = Canvas.GetLeft(child);

                Canvas.SetTop(child, top * scaleH);
                Canvas.SetLeft(child, left * scaleW);
            }
        }

        private bool CanTransformChildsRecursive(List<ContentPresenter> childs, double scaleH, double scaleW)
        {
            bool canTransform = true;

            // first pass, check if we can apply transform
            foreach (ContentPresenter child in childs)
            {
                var deeperChilds = this.GetChildElements(child);
                if (deeperChilds != null)
                {
                    canTransform = this.CanTransformChildsRecursive(deeperChilds, scaleH, scaleW);
                }

                double newTop = Canvas.GetTop(child) * scaleH;
                double newLeft = Canvas.GetLeft(child) * scaleW;

                // check out of bounds
                if (newTop < BorderThreshold || newLeft < BorderThreshold)
                {
                    canTransform = false;
                }

                double height = child.ActualHeight * scaleH;
                double width = child.ActualWidth * scaleW;

                // check out of bounds
                if (height < child.MinHeight || width < child.MinWidth)
                {
                    canTransform = false;
                }
            }

            return canTransform;
        }

        private List<ContentPresenter> GetChildElements(ContentPresenter element)
        {
            CustomCanvas elementCanvas = element.FindChild<CustomCanvas>(string.Empty);
            var childs = elementCanvas?.Children.Cast<ContentPresenter>().ToList();
            return childs;
        }

        private void CalculateLimits(CustomCanvas canvas, IEnumerable<ContentPresenter> items, out double minLeft,
            out double minTop,
            out double minDeltaHorizontal, out double minDeltaVertical,
            out double maxDeltaHorizontal, out double maxDeltaVertical)
        {
            minLeft = double.MaxValue;
            minTop = double.MaxValue;
            minDeltaHorizontal = double.MaxValue;
            minDeltaVertical = double.MaxValue;
            maxDeltaHorizontal = double.MaxValue;
            maxDeltaVertical = double.MaxValue;

            foreach (ContentPresenter item in items)
            {
                // resize delta should not go out of canvas bounds (top and left)
                // min distance to top and left among all elements
                double left = Canvas.GetLeft(item) - BorderThreshold;
                double top = Canvas.GetTop(item) - BorderThreshold;
                minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);

                // resize delta should not go over item minimum height or width
                // min distance to element minimum heigth and width
                minDeltaVertical = Math.Min(minDeltaVertical, item.ActualHeight - item.MinHeight);
                minDeltaHorizontal = Math.Min(minDeltaHorizontal, item.ActualWidth - item.MinWidth);

                // resize delta should not go out of canvas bounds (bottom and right)
                maxDeltaVertical = Math.Min(maxDeltaVertical, canvas.ActualHeight - Canvas.GetTop(item) - item.ActualHeight - BorderThreshold);
                maxDeltaHorizontal = Math.Min(maxDeltaHorizontal, canvas.ActualWidth - Canvas.GetLeft(item) - item.ActualWidth - BorderThreshold);
            }
        }
    }
}
