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
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Base class for question, bubbles and other controls with selectable functionality
    /// </summary>
    public abstract class BaseOmrElement : ContentControl
    {
        /// <summary>
        /// IsSelected dependency property 
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected",
                typeof(bool),
                typeof(BaseOmrElement),
                new FrameworkPropertyMetadata
                {
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = IsSelectedPropertyChanged
                }
            );

        /// <summary>
        /// Gets or sets a value indicating whether the item is selected
        /// </summary>
        public bool IsSelected
        {
            get { return (bool) this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Update adorners on selection changes
        /// </summary>
        /// <param name="isSelected">IsSelected flag</param>
        protected abstract void UpdateAdorners(bool isSelected);

        /// <summary>
        /// Fires on IsSelected property changed
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        private static void IsSelectedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            BaseOmrElement question = (BaseOmrElement) sender;
            question.UpdateAdorners((bool)e.NewValue);
        }
    }
}
