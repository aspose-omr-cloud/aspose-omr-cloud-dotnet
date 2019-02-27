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
namespace Aspose.OMR.Client.Views
{
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Utility;
    using ViewModels;

    /// <summary>
    /// Interaction logic for TemplateView.xaml
    /// </summary>
    public partial class TemplateView : UserControl
    {
        public TemplateView()
        {
            this.InitializeComponent();
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            bool isCtrlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);

            if (!isCtrlDown)
            {
                return;
            }

            var context = (TemplateViewModel) this.DataContext;

            if (e.Delta > 0)
            {
                context.ZoomInCommand.Execute(null);
            }
            else
            {
                context.ZoomOutCommand.Execute(null);
            }
        }

        private void OnFitPageWidth(object sender, RoutedEventArgs e)
        {
            Size renderSize = this.MainScroll.RenderSize;
            var context = (TemplateViewModel) this.DataContext;

            ICommand command = context.FitPageWidthCommand;
            if (command.CanExecute(renderSize.Width))
            {
                command.Execute(renderSize.Width);
            }
        }

        private void OnFitPageHeight(object sender, RoutedEventArgs e)
        {
            Size renderSize = this.MainScroll.RenderSize;
            var context = (TemplateViewModel) this.DataContext;

            ICommand command = context.FitPageHeightCommand;
            if (command.CanExecute(renderSize))
            {
                command.Execute(renderSize);
            }
        }

        /// <summary>
        /// Handles image file drop
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        private void OnImageDrop(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            var context = this.DataContext as TemplateViewModel;

            if (context == null || files == null || files.Length < 1)
            {
                return;
            }

            // take first string (if several files were dropped, take first)
            string file = files[0];

            // get format
            string format = Path.GetExtension(file);

            if (format != null && GlobalConstants.SupportedImageFormats.Contains(format.ToLower()))
            {
                context.DropPageImageCommand.Execute(file);
            }

            // bring client window on top
            Window.GetWindow(this).Activate();

            // handle event so it is not fired on main window
            e.Handled = true;
        }

        /// <summary>
        /// Fires when scrollviewer viewport is changed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScrollChange(object sender, ScrollChangedEventArgs e)
        {
            // choose appropriate point inside viewport for precise paste 
            var scroll = sender as ScrollViewer;
            var context = this.DataContext as TemplateViewModel;
            
            if (context != null && scroll != null)
            {
                // specify viewport paste position
                double x = scroll.HorizontalOffset / TemplateViewModel.ZoomKoefficient + scroll.ViewportWidth / 2;
                double y = scroll.VerticalOffset / TemplateViewModel.ZoomKoefficient + scroll.ViewportHeight / 2;
                context.PasteViewPortPosition = new Point(x, y);
            }
        }

        private void ScrollViewLoaded(object sender, RoutedEventArgs e)
        {
            var scroll = sender as ScrollViewer;
            var context = this.DataContext as TemplateViewModel;

            if (context != null && scroll != null)
            {
                context.ViewportWidth = scroll.ViewportWidth;
                context.FitPageWidthCommand.Execute(scroll.ViewportWidth);
            }
        }
    }
}
