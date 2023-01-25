/*
 * Copyright (C) 2023 Aspose Pty Ltd. All Rights Reserved.
 *
 * Licensed under the MIT License (hereinafter the "License");
 * you may not use this file except in accordance with the License.
 * You can obtain a copy of the License at
 *
 *      https://github.com/aspose-omr-cloud/aspose-omr-cloud-dotnet/blob/master/LICENSE
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace OMRDemo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    using Newtonsoft.Json.Linq;

    using Aspose.Omr.Cloud.Sdk.Api;
    using Aspose.Omr.Cloud.Sdk.Model;
    using Aspose.Omr.Cloud.Sdk.Client;
    using System.Text.RegularExpressions;

    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                OmrDemo demo = new OmrDemo();
                demo.RunDemo();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                Console.WriteLine("StackTrace:");
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("Press a key to Exit.");
            Console.ReadKey();
        }
    }

    class OmrDemo
    {
        /// <summary>
        /// The configuration file in JSON format:
        /// {
        ///     "client_secret"  : "xxxxx",
        ///     "client_id"   : "xxx-xxx-xxx-xxx-xxx",
        ///     "base_path"  : "Aspose.OMR Cloud URL",
        ///     "data_folder"   : "Data",
        ///     "result_folder" : "Temp",
        /// }
        /// </summary>
        string configFileName = "test_config.json";

        /// <summary>
        /// Declare an object to hold the parsed configuration data
        /// </summary>
        private JObject Config = null;

        /// <summary>
        /// Name of the sub-module with demo data and the configuration file
        /// </summary>
        string demoDataSubmoduleName = "aspose-omr-cloud-demo-data";

        /// <summary>
        /// File names for template sources, printable form, recognition pattern and results
        /// </summary>
        const string BaseFileName = "Aspose_test";
        readonly string TemplateGenerationFileName = BaseFileName + ".txt";
        readonly string TemplateImageName = BaseFileName + ".jpg";
        readonly string OmrFileName = BaseFileName + ".omr";
        readonly string ResultFileName = BaseFileName + ".csv";
        readonly string[] templateLogosImagesNames = new string[] { "logo1.jpg", "logo2.png" };

        /// <summary>
        /// App Key value lodaed from config
        /// </summary>
        private string ClientID => this.Config["client_id"].ToString();

        /// <summary>
        /// App Sid value lodaed from config
        /// </summary>
        private string ClientSecret => this.Config["client_secret"].ToString();

        /// <summary>
        /// API base path loaded from config
        /// </summary>
        private string Basepath => this.Config["base_path"].ToString();

        /// <summary>
        /// Path to the folder with sample files (data_folder)
        /// Must be relative to the the configuration file folder
        /// </summary>
        private string DataFolder { get; set; }

        /// <summary>
        /// Path to the folder with printable form and recognition results (result_folder)
        /// Must be relative to the the configuration file folder
        /// </summary>
        private string ResultFolder { get; set; }

        /// <summary>
        /// Authentication url
        /// </summary>
        private string AuthUrl => this.Config["auth_url"].ToString();

        /// <summary>
        /// Declare an object to hold an instance of GenerateTemplateApi class
        /// </summary>
        private GenerateTemplateApi GenerateTemplateApi = null;

        /// <summary>
        /// Declare an object to hold an instance of RecognizeTemplateApi class
        /// </summary>
        private RecognizeTemplateApi RecognizeTemplateApi = null;

       
        public OmrDemo()
        {
            /// <summary>
            /// Get the working directory of the application
            /// </summary>
            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());

            /// <summary>
            /// Get the relative path to the configuration file
            /// </summary>
            string configFileRelativePath = Path.Combine(this.demoDataSubmoduleName, this.configFileName);

            /// <summary>
            /// Locate a sub-module folder with demo data and the configuration file
            /// </summary>
            while (current != null && !File.Exists(Path.Combine(current.FullName, configFileRelativePath)))
            {
                current = current.Parent;
            }

            /// <summary>
            /// Check if the configuration file exists
            /// </summary>
            if (current == null)
            {
                throw new Exception($"Unable to find {this.configFileName}");
            }

            /// <summary>
            /// Get the absolute path to the configuration file
            /// </summary>
            string configFilePath = Path.Combine(current.FullName, configFileRelativePath);

            /// <summary>
            /// Parse the configuration file
            /// </summary>
            this.Config = JObject.Parse(File.ReadAllText(configFilePath));
            this.DataFolder = Path.Combine(Directory.GetParent(configFilePath).FullName, this.Config["data_folder"].ToString());
            this.ResultFolder = Path.Combine(Directory.GetParent(configFilePath).FullName, this.Config["result_folder"].ToString());
            

            Configuration apiConfiguration = new Configuration();
            apiConfiguration.ClientID = this.ClientID;
            apiConfiguration.ClientSecret = this.ClientSecret;
            apiConfiguration.AuthUrl= this.AuthUrl;
            apiConfiguration.BasePath = this.Basepath;

            /// <summary>
            /// Create an instance of GenerateTemplateApi class
            /// </summary>
            this.GenerateTemplateApi = new GenerateTemplateApi(apiConfiguration);

            /// <summary>
            /// Create an instance of RecognizeTemplateApi class
            /// </summary>
            this.RecognizeTemplateApi = new RecognizeTemplateApi(apiConfiguration);
        }

        /// <summary>
        /// Process sample data
        /// </summary>
        public void RunDemo()
        {
            /// <summary>
            /// STEP 1: Queue the template source file for generation
            /// </summary>
            Console.WriteLine("\t\tGenerate template...");
            string templateId = GenerateTemplate(Path.Combine(DataFolder, TemplateGenerationFileName),null /*templateLogosImagesNames*/);

            /// <summary>
            /// STEP 2: Fetch generated printable form and recognition pattern
            /// </summary>
            Console.WriteLine("\t\tGet generation result by ID...");
            OMRResponse generationResult = this.GetGenerationResultById(templateId);

            /// <summary>
            /// STEP 3: Save the printable form and recognition pattern into result_folder
            /// </summary>
            Console.WriteLine("\t\tSave generation result...");
            SaveGenerationResult(generationResult, ResultFolder);

            /// <summary>
            /// STEP 4: Queue the scan / photo of the filled form for recognition
            /// </summary>
            Console.WriteLine("\t\tRecognize image...");
            string recognizeTemplateId = RecognizeImage(Path.Combine(DataFolder, TemplateImageName),
                Path.Combine(ResultFolder, OmrFileName));

            /// <summary>
            /// STEP 5: Fetch recognition results
            /// </summary>
            Console.WriteLine("\t\tGet recognition result by ID...");
            OMRRecognitionResponse recognitionResponse = this.GetRecognitionResultById(recognizeTemplateId);

            /// <summary>
            /// STEP 6: Save the recognition results into result_folder
            /// </summary>
            Console.WriteLine("\t\tSave recognition result...");
            SaveRecognitionResult(recognitionResponse, Path.Combine(ResultFolder, ResultFileName));
        }

        /// <summary>
        /// Generate the template from the provided sources
        /// </summary>
        /// <param name="templateFilePath">Path to the template source code</param>
        /// <param name="templateLogosImagesNames">Names of all image files mentioned in template source code</param>
        /// <returns>Response from generation queue</returns>
        protected string GenerateTemplate(string templateFilePath, string[] templateLogosImagesNames)
        {
            /// <summary>
            /// Read the template source code
            /// </summary>
            byte[] image = File.ReadAllBytes(templateFilePath);

            /// <summary>
            /// Configure the page layout
            /// </summary>
            PageSettings settings = new PageSettings
            {
                FontSize = 13,
            };

            /// <summary>
            /// Load images used in the template
            /// </summary>
            Dictionary<string, byte[]> images = new Dictionary<string, byte[]>();
            if (templateLogosImagesNames != null)
            {
                for (int i = 0; i < templateLogosImagesNames.Length; i++)
                {
                    byte[] logo = File.ReadAllBytes(Path.Combine(DataFolder, templateLogosImagesNames[i]));
                    string name = Path.GetFileName(templateLogosImagesNames[i]);
                    images.TryAdd(name, logo);
                }
            }

            /// <summary>
            /// Build request
            /// </summary>
            OmrGenerateTask task = new OmrGenerateTask(image, settings, images);

            /// <summary>
            /// Put the request into queue
            /// </summary>
            return GenerateTemplateApi.PostGenerateTemplate(task);
        }

        /// <summary>
        /// Fetch generated printable form and recognition pattern by ID
        /// If the request is still being processed, wait for 5 seconds and try again
        /// </summary>
        /// <param name="templateId">Generated template ID</param>
        /// <returns>OMRGenerationResponse</returns>
        protected OMRResponse GetGenerationResultById(string templateId)
        {
            OMRResponse generationResult = new OMRResponse();
            while (true)
            {
                generationResult = GenerateTemplateApi.GetGenerateTemplate(templateId);
                
                if (generationResult.ResponseStatusCode == ResponseStatusCode.Ok)
                {
                    break;
                }                

                else if (generationResult.ResponseStatusCode == ResponseStatusCode.Error)
                {
                    throw new Exception($"Something went wrong ... + {generationResult.Error}");
                }

                else
                {
                    Console.WriteLine("Please wait while we are processing your request...");
                    Thread.Sleep(5000);
                }
            }

            return generationResult;
        }

        /// <summary>
        /// Recognize the image of the filled form
        /// </summary>
        /// <param name="imagePath">Path to the scanned or photographed image of the filled form</param>
        /// <param name="omrFilePath">Path to the recognition pattern file (.OMR)</param>
        /// <returns>Response from recognition queue</returns>
        protected string RecognizeImage(string imagePath, string omrFilePath)
        {
            /// <summary>
            /// Read the recognition pattern file
            /// </summary>
            byte[] omrFile = File.ReadAllBytes(omrFilePath);

            /// <summary>
            /// Set the recognition accuracy
            /// Lower value allow even the lightest marks to be recognized
            /// Higher value require a more solid fill and may cause pencil marks to be ignored
            /// </summary>
            int recognitionThreshold = 30;

            /// <summary>
            /// Read the filled form
            /// </summary>
            byte[] image = File.ReadAllBytes(imagePath);
            List<byte[]> images = new List<byte[]>();
            images.Add(image);

            /// <summary>
            /// Build request
            /// </summary>
            OmrRecognizeTask task = new OmrRecognizeTask(images, omrFile, recognitionThreshold);

            /// <summary>
            /// Put the request into queue
            /// </summary>
            return RecognizeTemplateApi.PostRecognizeTemplate(task);
        }

        /// <summary>
        /// Fetch recognition result by ID
        /// If the request is still being processed, wait for 5 seconds and try again
        /// </summary>
        /// <param name="templateId">Template ID</param>
        /// <returns>OMRRecognitionResponse</returns>
        protected OMRRecognitionResponse GetRecognitionResultById(string templateId)
        {
            OMRRecognitionResponse recognitionResult = new OMRRecognitionResponse();
            while (true)
            {
                recognitionResult = RecognizeTemplateApi.GetRecognizeTemplate(templateId);

                if (recognitionResult.ResponseStatusCode == ResponseStatusCode.Ok)
                {
                    break;
                }

                else if (recognitionResult.ResponseStatusCode == ResponseStatusCode.Error)
                {
                    throw new Exception($"Something went wrong ... + {recognitionResult.Error}");
                }

                else
                {
                    Console.WriteLine("Please wait while we are processing your request...");
                    Thread.Sleep(5000);
                }
            }

            return recognitionResult;
        }

        /// <summary>
        /// Save the printable form and recognition pattern
        /// </summary>
        /// <param name="generationResult">Response from GetGenerationResultById method</param>
        /// <param name="path">Path for saving generated OMR form</param>
        public void SaveGenerationResult(OMRResponse generationResult, string path)
        {
            if (generationResult.Error == null)
            {
                for (int i = 0; i < generationResult.Results.Count; i++)
                {
                    string name = BaseFileName + "." + generationResult.Results[i].Type.ToLower();
                    string pathFile = Path.Combine(path, name);
                    File.WriteAllBytes(pathFile, generationResult.Results[i].Data);
                }
            }
            else
            {
                Console.WriteLine("Error :", generationResult.Error.ToString());
            }
        }

        /// <summary>
        /// Save the recognition results
        /// </summary>
        /// <param name="recognitionResult">Response from GetRecognitionResultById method</param>
        /// <param name="path">Path for saving recognition results</param>
        protected void SaveRecognitionResult(OMRRecognitionResponse recognitionResult, string path)
        {
            if (recognitionResult.Error == null)
            {
                File.WriteAllBytes(path, recognitionResult.Results[0].Data);
            }
            else
            {
                Console.WriteLine("Error :", recognitionResult.Error.ToString());
            }
        }
    }
}
