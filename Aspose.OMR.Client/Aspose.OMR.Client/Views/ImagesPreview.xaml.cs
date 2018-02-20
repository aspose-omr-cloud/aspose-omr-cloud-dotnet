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
namespace Aspose.OMR.Client.Views
{
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using Utility;
    using ViewModels;

    /// <summary>
    /// Interaction logic for ImagesPreview.xaml
    /// </summary>
    public partial class ImagesPreview : UserControl
    {
        public ImagesPreview()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles image file drop
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        private void ImagesPreviewDrop(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            var context = this.DataContext as ResultsViewModel;

            if (context == null || files == null || files.Length < 1)
            {
                return;
            }

            foreach (string file in files)
            {
                FileAttributes attributes = File.GetAttributes(file);

                if (attributes.HasFlag(FileAttributes.Directory))
                {
                    // handle directory drop
                    context.DropFolderCommand.Execute(file);
                }
                else
                {
                    string format = Path.GetExtension(file);

                    if (GlobalConstants.SupportedImageFormats.Contains(format.ToLower()))
                    {
                        context.DropImagesCommand.Execute(new string[] {file});
                    }
                }
            }

            // bring client window on top
            Window.GetWindow(this).Activate();

            // handle event so it is not fired on main window
            e.Handled = true;
        }
    }
}
