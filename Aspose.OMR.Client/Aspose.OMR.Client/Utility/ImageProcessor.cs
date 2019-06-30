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
namespace Aspose.OMR.Client.Utility
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows.Media.Imaging;
    using System.Windows;
    using System.Windows.Media;
    using Encoder = System.Drawing.Imaging.Encoder;
    using Point = System.Windows.Point;
    using TemplateModel;

    /// <summary>
    /// Contains various image processing methods
    /// </summary>
    public static class ImageProcessor
    {
        /// <summary>
        /// The size of the reference point
        /// </summary>
        private static readonly int ReferencePointSize = 6;

        /// <summary>
        /// Calculate positions of reference points rectangles
        /// </summary>
        /// <param name="image">The input image</param>
        /// <returns>Returns a tuple of (white rectangles, black rectangles)</returns>
        public static Tuple<Rect[], Rect[]> ConstructReferencePoints(BitmapImage image)
        {
            double dpix = image.DpiX;
            double dpiy = image.DpiY;
            int width = (int)(image.PixelWidth * 96 / dpix);
            int height = (int)(image.PixelHeight * 96 / dpiy);

            // 1% white margin
            int margin = width / 100;

            // % total white zone area: margin + black area + margin
            int whiteZone = width * ReferencePointSize / 100;

            Rect topLeftWhite = new Rect(new Point(0, 0), new Point(whiteZone, whiteZone));
            Rect topRightWhite = new Rect(new Point(width - whiteZone, 0), new Point(width, whiteZone));
            Rect downLeftWhite = new Rect(new Point(0, height - whiteZone), new Point(whiteZone, height));
            Rect downRightWhite = new Rect(new Point(width - whiteZone, height - whiteZone), new Point(width, height));

            Rect topLeftBlack = new Rect(new Point(margin, margin), new Point(whiteZone - margin, whiteZone - margin));
            Rect topRightBlack = new Rect(new Point(width - whiteZone + margin, margin), new Point(width - margin, whiteZone - margin));
            Rect downLeftBlack = new Rect(new Point(margin, height - whiteZone + margin), new Point(whiteZone - margin, height - margin));
            Rect downRightBlack = new Rect(new Point(width - whiteZone + margin, height - whiteZone + margin), new Point(width - margin, height - margin));

            Rect[] whiteAreas = new Rect[] { topLeftWhite, topRightWhite, downLeftWhite, downRightWhite };
            Rect[] refPoints = new Rect[] {topLeftBlack, topRightBlack, downLeftBlack, downRightBlack};
            return new Tuple<Rect[], Rect[]>(whiteAreas, refPoints);
        }

        /// <summary>
        /// Draw reference points on the image
        /// </summary>
        /// <param name="image">Image to draw</param>
        /// <param name="whiteAreas">White rectangles for white margin areas</param>
        /// <param name="refPoints">Black rectangles - actual reference points</param>
        /// <returns>Image with drawn reference points</returns>
        public static BitmapImage DrawReferencePoints(BitmapImage image, Rect[] whiteAreas, Rect[] refPoints)
        {
            // prepare resources
            SolidColorBrush whiteBrush = new SolidColorBrush(Colors.White);
            var blackBrush = new SolidColorBrush(Colors.Black);
            var whitePen = new System.Windows.Media.Pen(whiteBrush, 1);
            var blackPen = new System.Windows.Media.Pen(blackBrush, 1);
            whiteBrush.Freeze();
            blackBrush.Freeze();
            whitePen.Freeze();
            blackPen.Freeze();

            var target = new RenderTargetBitmap(image.PixelWidth, image.PixelHeight, image.DpiX, image.DpiY, PixelFormats.Pbgra32);
            var visual = new DrawingVisual();
            using (DrawingContext r = visual.RenderOpen())
            {
                r.DrawImage(image, new Rect(0, 0, image.Width, image.Height));

                for (int i = 0; i < whiteAreas.Length; i++)
                {
                    r.DrawRectangle(whiteBrush, whitePen, whiteAreas[i]);
                    r.DrawRectangle(blackBrush, blackPen, refPoints[i]);
                }
            }

            // call render to render all changes on the image
            target.Render(visual);

            return ConvertRenderTargetToBitmapImage(target);
        }

        /// <summary>
        /// Create reference points model elements for the template
        /// </summary>
        /// <param name="refPoints">The reference points rectnagles</param>
        /// <returns>Array of reference point elements</returns>
        public static ReferencePointElement[] CreateReferencePointsModels(Rect[] refPoints)
        {
            ReferencePointElement[] result = new ReferencePointElement[refPoints.Length];
            for (int i = 0; i < refPoints.Length; i++)
            {
                ReferencePointElement element = new ReferencePointElement();
                element.Top = refPoints[i].Top;
                element.Left = refPoints[i].Left;
                element.Height = refPoints[i].Height;
                element.Width = refPoints[i].Width;
                element.Name = "ReferencePoint" + i.ToString();

                result[i] = element;
            }

            return result;
        }

        /// <summary>
        /// Save template image as png by specified location
        /// </summary>
        /// <param name="templateImage">Image to save</param>
        /// <param name="path">Save location</param>
        public static void SaveTemplateImage(BitmapImage templateImage, string path)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(templateImage));

            using (var filestream = new FileStream(path, FileMode.OpenOrCreate))
            {
                encoder.Save(filestream);
            }
        }

        /// <summary>
        /// Copies user image from temp location to user provided destination
        /// </summary>
        /// <param name="pathToTempLocation">Path to temp folder where image is located</param>
        /// <param name="destination">New destination path</param>
        /// <returns>True if copy was successful, false otherwise</returns>
        public static bool CopyUserTemplateImage(string pathToTempLocation, string destination)
        {
            try
            {
                if (!File.Exists(pathToTempLocation))
                {
                    return false;
                }

                byte[] fileData = File.ReadAllBytes(pathToTempLocation);
                File.WriteAllBytes(destination, fileData);
            }
            catch (Exception e)
            {
                DialogManager.ShowErrorDialog(e.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Attempts to copy file data to temp location under Aspose.OMR.Client folder
        /// </summary>
        /// <param name="filePath">Path to current file location</param>
        /// <param name="imageName">Image name with extension</param>
        /// <returns>Path to file temp location if copy successful, empty string otherwise</returns>
        public static string CopyFileToTemp(string filePath, string imageName)
        {
            string tempPath = Path.GetTempPath();
            string appFolder = "Aspose.OMR.Client";
            string destPath = Path.Combine(tempPath, appFolder);

            try
            {
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }

                byte[] fileData = File.ReadAllBytes(filePath);

                destPath = Path.Combine(destPath, imageName);
                File.WriteAllBytes(destPath, fileData);
            }
            catch (Exception e)
            {
                DialogManager.ShowErrorDialog(e.Message);
                return "";
            }

            return destPath;
        }

        /// <summary>
        /// Load image without lock
        /// </summary>
        /// <param name="path">Path to the image</param>
        /// <returns>Loaded bitmap image</returns>
        public static BitmapImage LoadImageNoLock(string path)
        {
            var bitmap = new BitmapImage();
            var stream = File.OpenRead(path);

            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            bitmap.StreamSource = stream;
            bitmap.EndInit();

            stream.Close();
            stream.Dispose();

            return bitmap;
        }

        /// <summary>
        /// Check provided image size and compress image to byte array
        /// </summary>
        /// <param name="templateImage">Template image</param>
        /// <param name="imageSizeInBytes">Image size in bytes</param>
        /// <returns>Image data</returns>
        public static byte[] CompressImage(BitmapImage templateImage, long imageSizeInBytes)
        {
            int compressionQuality = 100;

            // if image is larger than 500KB
            if (imageSizeInBytes > 1024 * 500)
            {
                compressionQuality = 90;
            }

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = compressionQuality;

            encoder.Frames.Add(BitmapFrame.Create(templateImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Compress and resize image
        /// </summary>
        /// <param name="imagePath">Path to the image</param>
        /// <param name="quality">Specified Jpeg quality level</param>
        /// <param name="width">Width to fit image to</param>
        /// <param name="height">Height to fit image to</param>
        /// <returns>Image data</returns>
        public static byte[] CompressAndResizeImage(string imagePath, int quality, int width, int height)
        {
            using (Bitmap image = new Bitmap(imagePath))
            {
                Bitmap resizedImage = ResizeImage(image, width, height);

                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                Encoder myEncoder = Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
                myEncoderParameters.Param[0] = myEncoderParameter;

                var memoryStream = new MemoryStream();
                resizedImage.Save(memoryStream, jpgEncoder, myEncoderParameters);

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Resize image to fit specified size preserving aspect ratio
        /// </summary>
        /// <param name="image">Image to process</param>
        /// <param name="width">Width to fit image to</param>
        /// <param name="height">Height to fit image to</param>
        /// <returns>Resized image</returns>
        private static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            // find scales
            float scaleHeight = (float)height / (float)image.Height;
            float scaleWidth = (float)width / (float)image.Width;

            // find final scale
            float scale = Math.Min(scaleHeight, scaleWidth);

            // determine updated width and height
            int newWidth = (int) (image.Width * scale);
            int newHeight = (int) (image.Height * scale);

            Rectangle destRect = new Rectangle(0, 0, newWidth, newHeight);
            Bitmap destImage = new Bitmap(newWidth, newHeight);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        /// <summary>
        /// Get encoded by specified format
        /// </summary>
        /// <param name="format">Iamge format</param>
        /// <returns>Image encoder</returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        /// <summary>
        /// Convert render target bitmap to the bitmap image
        /// </summary>
        /// <param name="input">The render bitmap input</param>
        /// <returns>The resulting bitmap image</returns>
        private static BitmapImage ConvertRenderTargetToBitmapImage(RenderTargetBitmap input)
        {
            BitmapImage bitmapImage = new BitmapImage();

            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(input));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }
    }
}
