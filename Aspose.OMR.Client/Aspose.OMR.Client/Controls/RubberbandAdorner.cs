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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using Utility;

    /// <summary>
    /// Adorner used for rubberband selection within canvas (i.e. drag rectangle)
    /// </summary>
    public class RubberbandAdorner : Adorner
    {
        /// <summary>
        /// Rectangle points
        /// </summary>
        private Point? startPoint, endPoint;

        /// <summary>
        /// Pen used for displaying selection rectangle
        /// </summary>
        private readonly Pen rubberbandPen;

        /// <summary>
        /// Brush used to color selection rectangle
        /// </summary>
        private readonly Brush rubberandBrush;

        /// <summary>
        /// Parent canvas
        /// </summary>
        private readonly CustomCanvas canvas;

        /// <summary>
        /// Indicates whether rectangle is used for selection
        /// </summary>
        private readonly bool toSelect;

        /// <summary>
        /// Initializes a new instance of the <see cref="RubberbandAdorner"/> class
        /// </summary>
        /// <param name="canvas">Parent canvas</param>
        /// <param name="dragStartPoint">Dragging start point</param>
        /// <param name="mode">Mode of selection rectangle</param>
        public RubberbandAdorner(CustomCanvas canvas, Point? dragStartPoint, SelectionRectnagleModes mode) : base(canvas)
        {
            this.canvas = canvas;
            this.startPoint = dragStartPoint;

            // choose color and selection behaviour based on mode
            switch (mode)
            {
                case SelectionRectnagleModes.Selection:
                {
                    this.rubberandBrush = (Brush) Application.Current.FindResource("SelectionBrush");
                    this.toSelect = true;
                    break;
                }

                case SelectionRectnagleModes.ChoiceBox:
                {
                    this.rubberandBrush = (Brush) Application.Current.FindResource("MainItemsBrush");
                    this.toSelect = false;
                    break;
                }

                case SelectionRectnagleModes.Barcode:
                {
                    this.rubberandBrush = (Brush)Application.Current.FindResource("BarcodeBrush");
                    this.toSelect = false;
                    break;
                }

                case SelectionRectnagleModes.Grid:
                {
                    this.rubberandBrush = (Brush)Application.Current.FindResource("MainItemsBrush");
                    this.toSelect = false;
                    break;
                }
            }

            this.rubberbandPen = new Pen(this.rubberandBrush, 1);
            this.rubberbandPen.DashStyle = new DashStyle(new double[] { 2 }, 1);
        }

        /// <summary>
        /// Gets the adorner rectangle area
        /// </summary>
        public Rect Rectangle { get; private set; }

        /// <summary>
        /// Fires on mouse move
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!this.IsMouseCaptured)
                {
                    this.CaptureMouse();
                }

                this.endPoint = e.GetPosition(this);
                this.InvalidateVisual();
            }
            else
            {
                if (this.IsMouseCaptured)
                {
                    this.ReleaseMouseCapture();
                }
            }

            e.Handled = true;
        }

        /// <summary>
        /// Fires when mouse is released
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            // release mouse capture
            if (this.IsMouseCaptured)
            {
                this.UpdateSelection();
                this.ReleaseMouseCapture();
            }
        }

        /// <summary>
        /// Fires when control is rendered
        /// </summary>
        /// <param name="drawingContext">Drawing context</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            // without a background the OnMouseMove event would not be fired
            drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(this.RenderSize));

            if (this.startPoint.HasValue && this.endPoint.HasValue)
            {
                drawingContext.DrawRectangle(Brushes.Transparent, this.rubberbandPen, new Rect(this.startPoint.Value, this.endPoint.Value));
            }
        }

        /// <summary>
        /// Updates selected elements according to rubberband adorner position
        /// </summary>
        private void UpdateSelection()
        {
            this.Rectangle = new Rect(this.startPoint.Value, this.endPoint.Value);

            // update selection only when in appropriate mode
            if (!this.toSelect)
            {
                return;
            }

            this.canvas.ClearSelection();

            foreach (ContentPresenter presenter in this.canvas.Children)
            {
                // get item bounds
                Rect itemRect = VisualTreeHelper.GetDescendantBounds(presenter);
                Rect itemBounds = presenter.TransformToAncestor(this.canvas).TransformBounds(itemRect);

                // check if item contains in rubberband rectangle
                if (this.Rectangle.IntersectsWith(itemBounds))
                {
                    // select item
                    BaseOmrElement question = (BaseOmrElement) VisualTreeHelper.GetChild(presenter, 0);
                    question.IsSelected = true;
                }
            }
        }
    }
}
