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
    using Encoder = System.Drawing.Imaging.Encoder;

    /// <summary>
    /// Contains various image processing methods
    /// </summary>
    public static class ImageProcessor
    {
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
    }
}
