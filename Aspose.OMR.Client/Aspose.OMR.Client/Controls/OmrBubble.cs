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
    using System.Windows.Documents;
    using System.Windows.Input;

    /// <summary>
    /// Class containing logic for OmrBubble visual control
    /// </summary>
    public class OmrBubble : BaseOmrElement
    {
        /// <summary>
        /// Add or remove adorners based on selection
        /// </summary>
        /// <param name="isSelected">IsSelected property value</param>
        protected override void UpdateAdorners(bool isSelected)
        {
            if (isSelected)
            {
                this.AddAdorners();
            }
            else
            {
                this.ClearAdorners();
            }
        }

        /// <summary>
        /// Handles preview mouse down event
        /// </summary>
        /// <param name="e">The event args</param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (!this.IsSelected)
            {
                // unselect all other items of same level and selected this item
                ControlHelper.FindParentCanvas(this).ClearSelection();
                this.IsSelected = true;
            }

            this.Focus();
            e.Handled = false;
        }

        /// <summary>
        /// Adds bubble adorner to adorner layer
        /// </summary>
        private void AddAdorners()
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
            BubbleAdorner adorner = new BubbleAdorner(this);
            adornerLayer.Add(adorner);
        }

        /// <summary>
        /// Removes all adorners from adorner layer
        /// </summary>
        private void ClearAdorners()
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
            Adorner[] adorners = adornerLayer.GetAdorners(this);

            if (adorners != null)
            {
                foreach (Adorner adorner in adorners)
                {
                    adornerLayer.Remove(adorner);
                }
            }
        }
    }
}
