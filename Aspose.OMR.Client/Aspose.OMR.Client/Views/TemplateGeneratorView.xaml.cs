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
    using ViewModels;
    using System.Diagnostics;
    using System.Windows.Navigation;

    /// <summary>
    /// Interaction logic for TemplateGeneratorView.xaml
    /// </summary>
    public partial class TemplateGeneratorView
    {
        public TemplateGeneratorView(TemplateGeneratorViewModel context)
        {
            this.InitializeComponent();
            this.DataContext = context;

            this.Owner = Application.Current.Windows.OfType<MainWindow>().First();
        }

        private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
