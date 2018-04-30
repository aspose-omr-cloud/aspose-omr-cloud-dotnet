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
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using ViewModels;

    /// <summary>
    /// Interaction logic for FinalizationResultsView.xaml
    /// </summary>
    public partial class FinalizationResultsView : Window
    {
        public FinalizationResultsView(FinalizationResultsViewModel finalizationResultsViewModel)
        {
            this.InitializeComponent();
            this.DataContext = finalizationResultsViewModel;
            this.Owner = Application.Current.Windows.OfType<MainWindow>().First();
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            bool isCtrlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);

            if (!isCtrlDown)
            {
                return;
            }

            var context = (FinalizationResultsViewModel)this.DataContext;

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
            var context = (FinalizationResultsViewModel)this.DataContext;

            ICommand command = context.FitPageWidthCommand;
            if (command.CanExecute(renderSize.Width))
            {
                command.Execute(renderSize.Width);
            }
        }

        private void OnFitPageHeight(object sender, RoutedEventArgs e)
        {
            Size renderSize = this.MainScroll.RenderSize;
            var context = (FinalizationResultsViewModel)this.DataContext;

            ICommand command = context.FitPageHeightCommand;
            if (command.CanExecute(renderSize))
            {
                command.Execute(renderSize);
            }
        }
    }
}
