/*
 * Copyright (c) 2020 Aspose Pty Ltd. All Rights Reserved.
 *
 * Licensed under the MIT (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://github.com/aspose-omr-cloud/aspose-omr-cloud-dotnet/blob/master/LICENSE
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Aspose.OMR.Demo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Aspose.Omr.Cloud.Sdk;
    using Aspose.Omr.Cloud.Sdk.Api;
    using Aspose.Omr.Cloud.Sdk.Model;
    using Aspose.Omr.Cloud.Sdk.Model.Requests;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using FileInfo = Aspose.Omr.Cloud.Sdk.Model.FileInfo;

    class Program
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
        /// File with dictionary for configuration in JSON format
        /// The config file should be looked like:
        /// {
        ///     "app_key"  : "xxxxx",
        ///     "app_sid"   : "xxx-xxx-xxx-xxx-xxx",
        ///     "base_path" : "https://api.aspose.cloud/v1.1",
        ///     "data_folder" : "Data"
        /// }
        /// Provide your own app_key and app_sid, which you can receive by registering at Aspose Cloud Dashboard (https://dashboard.aspose.cloud/) 
        /// </summary>
        string configFileName = "test_config_sample.json";

        /// <summary>
        /// Name of the submodule with demo data and configuration file
        /// </summary>
        string demoDataSubmoduleName = "aspose-omr-cloud-demo-data";

        /// <summary>
        /// Output path where all results are placed
        /// </summary>
        string PathToOutput = ".\\Temp";

        /// <summary>
        /// Name of the folder where all images used in template generation are located
        /// </summary>
        string LogosFolderName = "Logos";

        // Task file names
        const string TemplatePath = "Aspose_test";
        readonly string TemplateGenerationFileName = TemplatePath + ".txt";
        readonly string TemplateImageName = TemplatePath + ".png";
        readonly string[] templateUserImagesNames = new string[] { "photo.jpg", "scan.jpg" };
        readonly string[] templateLogosImagesNames = new string[] { "logo1.jpg", "logo2.png" };

        /// <summary>
        /// App Key value lodaed from config
        /// </summary>
        private string AppKey => this.Config["app_key"].ToString();

        /// <summary>
        /// App Sid value lodaed from config
        /// </summary>
        private string AppSid => this.Config["app_sid"].ToString();

        /// <summary>
        /// API base path loaded from config
        /// </summary>
        private string Basepath => this.Config["base_path"].ToString();

        /// <summary>
        /// Path to data folder with sample files, 
        /// should be on same level with config file
        /// </summary>
        private string DataFolder { get; set; }

        /// <summary>
        /// Parsed json containing config data
        /// </summary>
        private JObject Config = null;

        /// <summary>
        /// Instance of Cloud Storage API
        /// </summary>
        private StorageApi StorageApi = null;

        /// <summary>
        /// Instance of Cloud Folder API
        /// </summary>
        private FolderApi FolderApi = null;

        /// <summary>
        /// Instance of Cloud File API
        /// </summary>
        private FileApi FileApi = null;

        /// <summary>
        /// Instance of OMR API
        /// </summary>
        private OmrApi OmrApi = null;

        public OmrDemo()
        {
            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());

            string configFileRelativePath = Path.Combine(this.demoDataSubmoduleName, this.configFileName);

            // Locate submodule folder containing demo data and config
            while (current != null && !File.Exists(Path.Combine(current.FullName, configFileRelativePath)))
            {
                current = current.Parent;
            }

            // Check if config file exists
            if (current == null)
            {
                throw new Exception($"Unable to find {this.configFileName}");
            }

            string configFilePath = Path.Combine(current.FullName, configFileRelativePath);

            // parse config
            this.Config = JObject.Parse(File.ReadAllText(configFilePath));
            this.DataFolder = Path.Combine(Directory.GetParent(configFilePath).FullName, this.Config["data_folder"].ToString());

            // create storage api and provide parameters
            string baseHost = new Uri(this.Basepath).GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped).ToString();

            Configuration storageConfiguration = new Configuration();
            storageConfiguration.AppKey = this.AppKey;
            storageConfiguration.AppSid = this.AppSid;
            storageConfiguration.ApiBaseUrl = baseHost;

            this.StorageApi = new StorageApi(storageConfiguration);
            this.FolderApi = new FolderApi(storageConfiguration);
            this.FileApi = new FileApi(storageConfiguration);

            this.OmrApi = new OmrApi(storageConfiguration);
        }

        /// <summary>
        /// Runs all omr functions using sample data
        /// </summary>
        public void RunDemo()
        {
            // Step 1: Upload demo files on cloud and Generate template
            Console.WriteLine("\t\tUploading demo files...");
            this.UploadDemoFiles(this.DataFolder);

            Console.WriteLine("\t\tGenerate template...");
            OmrResponse generateReponse = this.GenerateTemplate(Path.Combine(this.DataFolder, this.TemplateGenerationFileName), this.LogosFolderName);
            if (generateReponse.ErrorCode == 0)
            {
                Utility.DeserializeFiles(generateReponse.Payload.Result.ResponseFiles, this.PathToOutput);
            }

            // Step 2: Validate template
            Console.WriteLine("\t\tValidate template...");
            string templateId = this.ValidateTemplate(Path.Combine(this.PathToOutput, this.TemplateImageName), this.PathToOutput);

            // Step 3: Recognize photos and scans
            Console.WriteLine("\t\tRecognize image...");
            foreach (var userImage in this.templateUserImagesNames)
            {
                var recognizeReponse = this.RecognizeImage(templateId, Path.Combine(this.DataFolder, userImage));
                if (recognizeReponse.ErrorCode == 0)
                {
                    var resultFile = Utility.DeserializeFiles(recognizeReponse.Payload.Result.ResponseFiles, this.PathToOutput)[0];
                    Console.WriteLine($"Result file {resultFile}");
                }
            }
        }

        /// <summary>
        /// Generate new template based on provided text description
        /// </summary>
        /// <param name="templateFilePath">Path to template text description</param>
        /// <param name="logosFolder">Name of the cloud folder with logo images</param>
        /// <returns>Generation response</returns>
        protected OmrResponse GenerateTemplate(string templateFilePath, string logosFolder)
        {
            // upload template text description
            string fileName = Path.GetFileName(templateFilePath);
            this.UploadFile(templateFilePath, fileName);

            // provide function parameters
            OmrFunctionParam callParams = new OmrFunctionParam();
            callParams.FunctionParam = JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "ExtraStoragePath", logosFolder}
            }, Formatting.Indented);

            PostRunOmrTaskRequest request = new PostRunOmrTaskRequest(fileName, "GenerateTemplate", callParams);
            return this.OmrApi.PostRunOmrTask(request);
        }

        /// <summary>
        /// Helper function that combines template correction and finalization
        /// </summary>
        /// <param name="templateImagePath">Path to template image</param>
        /// <param name="templateDataDir">The folder where Template Data will be stored</param>
        /// <returns>Template ID</returns>
        protected string ValidateTemplate(string templateImagePath, string templateDataDir)
        {
            OmrResponse correctReponse = this.CorrectTemplate(templateImagePath, templateDataDir);

            // save correction results and provide them to the template finalization 
            string correctedTemplatePath = string.Empty;
            if (correctReponse.ErrorCode == 0)
            {
                foreach (var responseFile in correctReponse.Payload.Result.ResponseFiles)
                {
                    Utility.DeserializeFile(responseFile, this.PathToOutput);
                    if (responseFile.Name.ToLower().EndsWith(".omrcr"))
                    {
                        correctedTemplatePath = Utility.DeserializeFile(responseFile, this.PathToOutput);
                    }
                }
            }
            else
            {
                throw new Exception($"Correct Template failed with error code {correctReponse.ErrorCode}, '{correctReponse.ErrorText}'");
            }

            string templateId = correctReponse.Payload.Result.TemplateId;
            OmrResponse finalizeReponse = this.FinalizeTemplate(templateId, correctedTemplatePath);
            return templateId;
        }

        /// <summary>
        /// Run template correction
        /// </summary>
        /// <param name="templateImagePath">Path to template image</param>
        /// <param name="templateDataDir">Path to template data file (.omr)</param>
        /// <returns>Correction response</returns>
        protected OmrResponse CorrectTemplate(string templateImagePath, string templateDataDir)
        {
            // upload template image
            string imageFileName = Path.GetFileName(templateImagePath);
            this.UploadFile(templateImagePath, imageFileName);

            // locate generated template file (.omr) and provide it's data as function parameter
            string templateDataPath = Path.Combine(templateDataDir, Path.GetFileNameWithoutExtension(imageFileName) + ".omr");
            OmrFunctionParam callParams = new OmrFunctionParam();
            callParams.FunctionParam = Utility.SerializeFiles(new string[] { templateDataPath });

            // call template correction
            PostRunOmrTaskRequest request = new PostRunOmrTaskRequest(imageFileName, "CorrectTemplate", callParams);
            return this.OmrApi.PostRunOmrTask(request);
        }

        /// <summary>
        /// Run template finalization
        /// </summary>
        /// <param name="templateId">Template id recieved after template correction</param>
        /// <param name="correctedTemplatePath">Path to corrected template (.omrcr)</param>
        /// <returns>Finalization response</returns>
        protected OmrResponse FinalizeTemplate(string templateId, string correctedTemplatePath)
        {
            // upload corrected template data on cloud
            string templateFileName = Path.GetFileName(correctedTemplatePath);
            this.UploadFile(correctedTemplatePath, templateFileName);

            // provide template id as function parameter
            OmrFunctionParam callParams = new OmrFunctionParam();
            callParams.FunctionParam = templateId;

            // call template finalization
            PostRunOmrTaskRequest request = new PostRunOmrTaskRequest(templateFileName, "FinalizeTemplate", callParams);
            return this.OmrApi.PostRunOmrTask(request);
        }

        /// <summary>
        /// Runs mark recognition on image
        /// </summary>
        /// <param name="templateId">Template ID</param>
        /// <param name="imagePath">Path to the image</param>
        /// <returns>Recognition response</returns>
        protected OmrResponse RecognizeImage(string templateId, string imagePath)
        {
            // upload image on cloud
            string imageFileName = Path.GetFileName(imagePath);
            this.UploadFile(imagePath, imageFileName);

            // provide template id as function parameter
            OmrFunctionParam callParams = new OmrFunctionParam();
            callParams.FunctionParam = templateId;

            // call image recognition
            PostRunOmrTaskRequest request = new PostRunOmrTaskRequest(imageFileName, "RecognizeImage", callParams);
            return this.OmrApi.PostRunOmrTask(request);
        }

        /// <summary>
        /// Upload file on cloud storage
        /// </summary>
        /// <param name="srcFile">Path to the file</param>
        /// <param name="dstPath">Name of the file on cloud</param>
        protected void UploadFile(string srcFile, string dstPath)
        {
            using (FileStream fs = new FileStream(srcFile, FileMode.Open))
            {
                FilesUploadResult response = FileApi.UploadFile(new UploadFileRequest(dstPath, fs));
                Console.WriteLine($"File {dstPath} uploaded successfully with response {response}");
            }
        }

        /// <summary>
        /// Upload logo images used during template generation in separate folder on cloud storage
        /// </summary>
        /// <param name="dataDirPath">Path to directory containing logo images</param>
        protected void UploadDemoFiles(string dataDirPath)
        {
            // check if folder already exists on storage
            ObjectExist response = this.StorageApi.ObjectExists(new ObjectExistsRequest(this.LogosFolderName));
            if (response.Exists == false)
            {
                FolderApi.CreateFolder(new CreateFolderRequest(this.LogosFolderName, "storage"));
            }

            // upload logo images
            foreach (string logo in this.templateLogosImagesNames)
            {
                string destLogoPath = $"{this.LogosFolderName}/{logo}";
                response = this.StorageApi.ObjectExists(new ObjectExistsRequest(destLogoPath));
                if (response.Exists == false)
                {
                    this.UploadFile(Path.Combine(dataDirPath, logo), destLogoPath);
                }
                else
                {
                    Console.WriteLine($"File {destLogoPath} already exists");
                }
            }
        }
    }

    /// <summary>
    /// Helper class containing serialization and deserialization methods
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Deserialize list of files to specified location
        /// </summary>
        /// <param name="files">List of response files</param>
        /// <param name="dstPath">Location to deserialize files to</param>
        /// <returns>Path to deserialized files</returns>
        public static List<string> DeserializeFiles(List<FileInfo> files, string dstPath)
        {
            List<string> result = new List<string>();
            foreach (var info in files)
            {
                result.Add(DeserializeFile(info, dstPath));
            }

            return result;
        }

        /// <summary>
        /// Deserialize single response file to specified location
        /// </summary>
        /// <param name="fileInfo">Response file to deserialize</param>
        /// <param name="dstPath">Location to deserialize files to</param>
        /// <returns>Path to deserialized file</returns>
        public static string DeserializeFile(FileInfo fileInfo, string dstPath)
        {
            if (!Directory.Exists(dstPath))
            {
                Directory.CreateDirectory(dstPath);
            }

            string dstFilePath = Path.Combine(dstPath, fileInfo.Name);
            using (FileStream fs = new FileStream(dstFilePath, FileMode.Create))
            {
                fs.Write(fileInfo.Data, 0, fileInfo.Data.Length);
                Console.WriteLine($"File saved {fileInfo.Name}");
            }

            return dstFilePath;
        }

        /// <summary>
        /// Serialize files for the request
        /// </summary>
        /// <param name="filePaths">Files to serialize</param>
        /// <returns>Json string with serialized files</returns>
        public static string SerializeFiles(string[] filePaths)
        {
            var filesInfo = new List<ParamFileInfo>();
            foreach (var filePath in filePaths)
            {
                filesInfo.Add(new ParamFileInfo()
                {
                    Name = Path.GetFileName(filePath),
                    Size = Convert.ToInt32(new System.IO.FileInfo(filePath).Length),
                    Data = File.ReadAllBytes(filePath)
                });
            }

            return JsonConvert.SerializeObject(new Dictionary<string, List<ParamFileInfo>>
            {
                {"Files", filesInfo}
            }, Formatting.Indented);
        }
    }

    /// <summary>
    /// Class representing file used in requests and responses
    /// </summary>
    class ParamFileInfo
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public byte[] Data { get; set; }
    }
}
