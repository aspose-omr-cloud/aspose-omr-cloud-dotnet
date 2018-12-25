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
namespace Aspose.OMR.Client.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Convers string image path to Uri and then to BitmapImage consumed by xaml
    /// </summary>
    public sealed class StringToUriConverter : MarkupExtension, IValueConverter
    {
        private static StringToUriConverter converter;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (value is string)
            {
                value = new Uri((string) value);
            }

            if (value is Uri)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.DecodePixelWidth = 100;
                image.DecodePixelHeight = 140;
                image.UriSource = (Uri) value;
                image.EndInit();
                return image;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new StringToUriConverter());
        }
    }
}
