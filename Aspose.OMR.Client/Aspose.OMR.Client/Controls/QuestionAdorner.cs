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
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using ViewModels;

    /// <summary>
    /// Adorner with drag and resize thumbs for OmrQuestion
    /// </summary>
    public class QuestionAdorner : Adorner
    {
        /// <summary>
        /// Collection of visual children of the adorner.
        /// </summary>
        private readonly VisualCollection visualChildren;

        /// <summary>
        /// value to scale element visuals
        /// </summary>
        private readonly double scale;

        /// <summary>
        /// Text header used for dragging.
        /// </summary>
        private MoveThumb textMoveThumb;

        /// <summary>
        /// Question rectangle used for dragging
        /// </summary>
        private MoveThumb outlineMoveThumb;

        /// <summary>
        /// Resize thumbs used to resize question
        /// </summary>
        private Thumb fullDotsResizeThumb;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The adorned element</param>
        public QuestionAdorner(UIElement adornedElement) : base(adornedElement)
        {
            // set adorner data context to be the same as context of adorned element
            this.SetBinding(DataContextProperty,
                new Binding(DataContextProperty.Name)
                {
                    Mode = BindingMode.OneWay,
                    Source = adornedElement
                }
            );

            this.visualChildren = new VisualCollection(this);
            this.scale = TemplateViewModel.ZoomKoefficient;

            this.BuildAdorner();
        }

        /// <summary>
        /// Gets visual children count
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return this.visualChildren.Count; }
        }

        /// <summary>
        /// Get desired transform for visual children
        /// </summary>
        /// <returns>Desired transform</returns>
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            if (this.visualChildren.Count > 0)
            {
                foreach (var thumb in this.visualChildren.OfType<Thumb>())
                {
                    thumb.RenderTransform = new ScaleTransform(1 / this.scale, 1 / this.scale);
                    thumb.RenderTransformOrigin = new Point(0.0, 0.0);
                }
            }

            return base.GetDesiredTransform(transform);
        }

        /// <summary>
        /// Rearrange size and position of thumbs
        /// </summary>
        /// <param name="finalSize">Final area within parent should be used to arrange</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Size scaledSize = new Size(finalSize.Width * this.scale, finalSize.Height * this.scale);
            double offset = 25 / this.scale;

            double adornerDesiredWidth = Math.Max(20, scaledSize.Width * 0.5);

            this.textMoveThumb.Arrange(new Rect(0, -offset, adornerDesiredWidth, 20));
            this.outlineMoveThumb.Arrange(new Rect(scaledSize));
            this.fullDotsResizeThumb.Arrange(new Rect(scaledSize));

            return base.ArrangeOverride(finalSize);
        }

        /// <summary>
        /// Returns a child at the specified index from a collection of child elements
        /// </summary>
        /// <param name="index">The zero-based index of the requested child element in the collection.</param>
        /// <returns>The requested child element.</returns>
        protected override Visual GetVisualChild(int index)
        {
            return this.visualChildren[index];
        }

        /// <summary>
        /// Build arr needed thumbs
        /// </summary>
        private void BuildAdorner()
        {
            this.BuildResizeThumbs();
            this.BuildTextMoveThumb();
            this.BuildOutlineMoveThumb();
        }

        /// <summary>
        /// Build resize thumbs
        /// </summary>
        private void BuildResizeThumbs()
        {
            this.fullDotsResizeThumb = new Thumb();
            this.fullDotsResizeThumb.Template = Application.Current.FindResource("FullDotsResizeTemplate") as ControlTemplate;
            this.visualChildren.Add(this.fullDotsResizeThumb);
        }

        /// <summary>
        /// Build text header used for dragging
        /// </summary>
        private void BuildTextMoveThumb()
        {
            this.textMoveThumb = new MoveThumb();
            this.textMoveThumb.Cursor = Cursors.SizeAll;
            this.textMoveThumb.Template = Application.Current.FindResource("MoveTextTemplate") as ControlTemplate;
            this.visualChildren.Add(this.textMoveThumb);
        }

        /// <summary>
        /// Build outline move thumbs
        /// </summary>
        private void BuildOutlineMoveThumb()
        {
            this.outlineMoveThumb = new MoveThumb();
            this.outlineMoveThumb.Cursor = Cursors.SizeAll;
            this.outlineMoveThumb.Template = Application.Current.FindResource("OutlineMoveThumb") as ControlTemplate;
            this.visualChildren.Add(this.outlineMoveThumb);
        }
    }
}
