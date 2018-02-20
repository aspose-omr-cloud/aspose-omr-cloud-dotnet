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
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Media;
    using ViewModels;

    /// <summary>
    /// Adorner with resize thumbs for OmrBubble
    /// </summary>
    public class BubbleAdorner : Adorner
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
        /// Resize thumb for bubble
        /// </summary>
        private ResizeThumb cornerDotsResizeThumb;

        /// <summary>
        /// Initializes a new instance of the <see cref="BubbleAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The adorned element</param>
        public BubbleAdorner(UIElement adornedElement) : base(adornedElement)
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

            this.BuildCornerResizeThumb();
        }

        /// <summary>
        /// Gets visual children count
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return this.visualChildren.Count; }
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            if (this.visualChildren.Count > 0)
            {
                foreach (Thumb thumb in this.visualChildren.OfType<Thumb>())
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

            this.cornerDotsResizeThumb.Arrange(new Rect(scaledSize));

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
        /// Add resize thumb to the bubble
        /// </summary>
        private void BuildCornerResizeThumb()
        {
            this.cornerDotsResizeThumb = new ResizeThumb();
            this.cornerDotsResizeThumb.Template = Application.Current.FindResource("CornerDotsResizeTemplate") as ControlTemplate;
            this.visualChildren.Add(this.cornerDotsResizeThumb);
        }
    }
}
