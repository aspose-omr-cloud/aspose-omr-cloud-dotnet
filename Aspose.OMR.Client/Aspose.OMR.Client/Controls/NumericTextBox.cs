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
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Custom text box which accepts only numeric values
    /// </summary>
    public class NumericTextBox : TextBox
    {
        /// <summary>
        /// Identifies <see cref="AcceptsFloat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AcceptsFloatProperty = DependencyProperty.Register(
            "AcceptsFloat",
            typeof(bool),
            typeof(NumericTextBox),
            new PropertyMetadata(false));

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericTextBox"/> class.
        /// </summary>
        public NumericTextBox()
        {
            this.PreviewTextInput += this.OnPreviewTextInput;
            this.PreviewKeyDown += this.OnPreviewKeyDown;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text box accepts float numbers.
        /// </summary>
        public bool AcceptsFloat
        {
            get { return (bool) this.GetValue(AcceptsFloatProperty); }
            set { this.SetValue(AcceptsFloatProperty, value); }
        }

        /// <summary>
        /// Occurs when a key is pressed while focus is on this element
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="keyEventArgs">Event arguments</param>
        private void OnPreviewKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            // workaround for space
            if (keyEventArgs.Key == Key.Space)
            {
                keyEventArgs.Handled = true;
            }
        }

        /// <summary>
        /// Called when text of a <see cref="NumericTextBox"/> is about to be changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">
        /// The <see cref="TextCompositionEventArgs"/> instance containing the event data.
        /// </param>
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs eventArgs)
        {
            string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            bool ok = true;
            foreach (char c in eventArgs.Text)
            {
                bool isDecimalSeparator = this.AcceptsFloat && decimalSeparator.IndexOf(c) >= 0
                                          && !this.Text.Contains(decimalSeparator);
                if (!char.IsDigit(c) && !isDecimalSeparator)
                {
                    ok = false;
                    break;
                }
            }

            eventArgs.Handled = !ok;
        }
    }
}
