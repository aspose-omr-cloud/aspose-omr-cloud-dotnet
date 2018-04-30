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
namespace Aspose.OMR.Client
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Windows.Media.Imaging;
    using TemplateModel;
    using ViewModels;

    /// <summary>
    /// Provides template serialization and deserialization functionality
    /// </summary>
    public static class TemplateSerializer
    {
        /// <summary>
        /// Serializes template into JSON string
        /// </summary>
        /// <param name="template">Template view model</param>
        /// <returns>JSON string</returns>
        public static string TemplateToJson(TemplateViewModel template)
        {
            OmrTemplate templateModel = TemplateConverter.ConvertViewModelToModel(template);

            string res;
            using (MemoryStream jsonStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(OmrTemplate));

                serializer.WriteObject(jsonStream, templateModel);
                byte[] json = jsonStream.ToArray();
                res = Encoding.UTF8.GetString(json, 0, json.Length);

                StringBuilder sb = new StringBuilder(res);
                PostprocessJson(sb);
                res = FormatJson(sb.ToString());
            }

            return res;
        }

        /// <summary>
        /// Deserializes JSON string to template view model
        /// </summary>
        /// <param name="jsonString">JSON string to process</param>
        /// <returns>Template view model</returns>
        public static TemplateViewModel JsonToTemplate(string jsonString)
        {
            StringBuilder sb = new StringBuilder(jsonString);
            PreprocessJson(sb);
            jsonString = sb.ToString();

            OmrTemplate template;
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(OmrTemplate));
                template = ser.ReadObject(ms) as OmrTemplate;
            }

            TemplateViewModel templateViewModel = TemplateConverter.ConvertModelToViewModel(template);

            return templateViewModel;
        }

        /// <summary>
        /// Deserialize finalization data
        /// </summary>
        /// <param name="jsonData">JSON string containing finalization data</param>
        /// <returns>Finalization data</returns>
        public static FinalizationData DeserializeFinalization(byte[] jsonData)
        {
            FinalizationData response;
            using (MemoryStream ms = new MemoryStream(jsonData))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(FinalizationData));
                response = ser.ReadObject(ms) as FinalizationData;
            }

            return response;
        }

        /// <summary>
        /// Check provided image size and compress image to base64 string
        /// </summary>
        /// <param name="templateImage">Template image</param>
        /// <param name="qualityLevel">Jpeg compression quality</param>
        /// <returns>Image data packed in base64 string</returns>
        public static string CompressImageBase64(BitmapImage templateImage, int qualityLevel)
        {
            string imageData;

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(templateImage));

            encoder.QualityLevel = qualityLevel;

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                imageData = Convert.ToBase64String(ms.ToArray());
            }

            return imageData;
        }

        public static string PngToBase64(BitmapImage templateImage)
        {
            string imageData;
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(templateImage));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                imageData = Convert.ToBase64String(ms.ToArray());
            }
            
            return imageData;
        }

        public static string TiffToBase64(BitmapImage templateImage)
        {
            string imageData;
            TiffBitmapEncoder encoder = new TiffBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(templateImage));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                imageData = Convert.ToBase64String(ms.ToArray());
            }

            return imageData;
        }

        public static string GifToBase64(BitmapImage templateImage)
        {
            string imageData;
            GifBitmapEncoder encoder = new GifBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(templateImage));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                imageData = Convert.ToBase64String(ms.ToArray());
            }

            return imageData;
        }

        /// <summary>
        /// Decompressed image data and loads it as BitmapImage
        /// </summary>
        /// <param name="imageData">Base64 string image</param>
        /// <returns>Loaded image</returns>
        public static BitmapImage DecompressImage(string imageData)
        {
            byte[] byteBuffer = Convert.FromBase64String(imageData);
            using (MemoryStream memoryStream = new MemoryStream(byteBuffer))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = memoryStream;
                image.EndInit();

                return image;
            }
        }

        /// <summary>
        /// Formats JSON to be more readable
        /// </summary>
        /// <returns>Formatted JSON</returns>
        private static string FormatJson(string str, string indentString = "\t")
        {
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                var ch = str[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            foreach (var e in Enumerable.Range(0, ++indent))
                                sb.Append(indentString);
                        }
                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            foreach (var e in Enumerable.Range(0, --indent))
                                sb.Append(indentString);
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && str[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            foreach (var e in Enumerable.Range(0, indent))
                                sb.Append(indentString);
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Preprocess received JSON string
        /// </summary>
        /// <param name="json">JSON to process</param>
        private static void PreprocessJson(StringBuilder json)
        {
            json.Replace("Type", "__type");
        }

        /// <summary>
        /// Postprocess resulting JSON string
        /// </summary>
        /// <param name="json">JSON to process</param>
        private static void PostprocessJson(StringBuilder json)
        {
            json.Replace("__type", "Type");
        }
    }
}
