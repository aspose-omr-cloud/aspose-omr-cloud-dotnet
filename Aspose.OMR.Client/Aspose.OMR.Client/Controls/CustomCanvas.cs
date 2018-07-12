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
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using Utility;
    using ViewModels;
    using Point = System.Windows.Point;

    /// <summary>
    /// Custom canvas with mouse events
    /// </summary>
    public class CustomCanvas : Canvas
    {
        /// <summary>
        /// Starting point for rubberband adorner
        /// </summary>
        private Point? dragStartPoint;

        /// <summary>
        /// The selection mode of RubberbandAdorner
        /// </summary>
        private SelectionRectnagleModes mode;

        /// <summary>
        /// Clears elements selection
        /// </summary>
        public void ClearSelection()
        {
            // find all content presenters wrapping child items (questions or bubbles)
            List<ContentPresenter> presenters = ControlHelper.GetChildPresenters(this);

            foreach (ContentPresenter presenter in presenters)
            {
                // find actual omr element that wrapped inside presenter
                BaseOmrElement omrElement = VisualTreeHelper.GetChild(presenter, 0) as BaseOmrElement;

                if (omrElement != null)
                {
                    if (omrElement.IsSelected)
                    {
                        omrElement.IsSelected = false;
                    }
                }
            }
        }

        /// <summary>
        /// Handles canvas mouse click
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (Equals(e.Source, this))
            {
                // remember click point in case of dragging
                this.dragStartPoint = e.GetPosition(this);

                // clear selected items
                this.ClearSelection();

                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles mouse move on canvas
        /// </summary>
        /// <param name="e">The event args</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                this.dragStartPoint = null;
            }

            // add rubberband adorner
            if (this.dragStartPoint.HasValue)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    TemplateViewModel dataContext = (TemplateViewModel) this.DataContext;

                    this.mode = SelectionRectnagleModes.Selection;

                    if (dataContext.IsAddingChoiceBox)
                    {
                        this.mode = SelectionRectnagleModes.ChoiceBox;
                    }
                    else if (dataContext.IsAddingGrid)
                    {
                        this.mode = SelectionRectnagleModes.Grid;
                    }
                    else if (dataContext.IsAddingBarcode)
                    {
                        this.mode = SelectionRectnagleModes.Barcode;
                    }

                    RubberbandAdorner adorner = new RubberbandAdorner(this, this.dragStartPoint, this.mode);
                    adorner.MouseUp += this.AdornerMouseUp;
                    adornerLayer.Add(adorner);
                }

                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles mouse up event sent from adorner
        /// </summary>
        /// <param name="sender">The adorner</param>
        /// <param name="e">The event args</param>
        private void AdornerMouseUp(object sender, MouseButtonEventArgs e)
        {
            RubberbandAdorner adorner = (RubberbandAdorner) sender;
            
            // remove adorner from adorner layer
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
            adornerLayer?.Remove(adorner);

            if (this.mode != SelectionRectnagleModes.Selection)
            {
                TemplateViewModel context = (TemplateViewModel)this.DataContext;
                context.AddQuestion(adorner.Rectangle);
            }
        }
    }
}
