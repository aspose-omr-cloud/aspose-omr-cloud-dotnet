/*
 * Copyright (c) 2018 Aspose Pty Ltd. All Rights Reserved.
 *
 * Licensed under the MIT (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      https://github.com/aspose-omr/Aspose.OMR-for-Cloud/blob/master/LICENSE
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using Com.Aspose.OMR.Api;
using Com.Aspose.OMR.Model;
using Com.Aspose.Storage.Api;
using Newtonsoft.Json;
using FileInfo = Com.Aspose.OMR.Model.FileInfo;

namespace Aspose.OMR.ConsoleClient
{
    class Program
    {
        // provide your own keys recieved by registrating at Aspose Cloud Dashboard (https://dashboard.aspose.cloud/)
        private static string APIKEY = "xxxxx";
        private static string APPSID = "xxxxx";

        /// <summary>
        /// API base path
        /// </summary>
        private static string BASEPATH = "https://api.aspose.cloud/v1.1";

        /// <summary>
        /// Path to test data
        /// </summary>
        static string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"TestExamples\");

        static void Main(string[] args)
        {
            // 0. Create template (using Aspose.OMR.Client) or generate template using Generate action
            GenerateExample();

            // pack template file data in JSON string
            string packedTemplate = PackTemplate("AsposeTestExample.omr", File.ReadAllText(path + "AsposeTestExample.omr"));

            // 1. Run template correction
            OMRResponse correctionResponse = RunOmrTask("AsposeTestExample.jpg", "CorrectTemplate", packedTemplate);

            // get template id, which will be used during finalization and recognition
            string templateId = correctionResponse.Payload.Result.TemplateId;

            // save corrected template data to file
            File.WriteAllBytes(path + "AsposeTestExample.omrcr", correctionResponse.Payload.Result.ResponseFiles[0].Data);

            // 2. Run template Finalization
            OMRResponse finaliztionResponse = RunOmrTask("AsposeTestExample.omrcr", "FinalizeTemplate", templateId);

            // 3. Recognize image
            OMRResponse recognitionResponse = RunOmrTask("1.jpg", "RecognizeImage", templateId);

            // get recognition results as string
            string recognitionResults = Encoding.UTF8.GetString(recognitionResponse.Payload.Result.ResponseFiles[0].Data);

            Console.WriteLine(recognitionResults);
            Console.WriteLine("DONE");
            Console.ReadLine();
        }

        /// <summary>
        /// Example of generation function
        /// </summary>
        private static void GenerateExample()
        {
            string generationParameter = string.Empty;

            // (optional) provide path to cloud storage where images for template are located,
            // in case you want to generate template with images
            // string generationParameter = "{ \"ExtraStoragePath\":\"Logos\"}";

            // call generate template function, provide txt file with textual description and parameters
            OMRResponse generationResponse = RunOmrTask("AsposeTest.txt", "GenerateTemplate", generationParameter);

            // get response and check each response file
            OmrResponseContent responseResult = generationResponse.Payload.Result;
            for (int i = 0; i < responseResult.ResponseFiles.Length; i++)
            {
                FileInfo responseFile = responseResult.ResponseFiles[i];
                if (responseFile.Name.Contains(".omr"))
                {
                    // save .omr file, which is generated template
                    string templateContent = Encoding.UTF8.GetString(responseFile.Data);
                    File.WriteAllText("AsposeTest.omr", templateContent);
                }
                else if (responseFile.Name.Contains(".png"))
                {
                    // save .png file, which is generated image
                    using (Image image = Image.FromStream(new MemoryStream(responseFile.Data)))
                    {
                        image.Save("AsposeTest.png", ImageFormat.Png);
                    }
                }
            }
        }

        private static Com.Aspose.OMR.Model.OMRResponse RunOmrTask(string inputFileName, string actionName, string functionParam)
        {
            // Instantiate Aspose Storage Cloud API SDK
            OmrApi target = new OmrApi(APIKEY, APPSID, BASEPATH);

            // Instantiate Aspose OMR Cloud API SDK
            StorageApi storageApi = new StorageApi(APIKEY, APPSID, BASEPATH);

            // Init function parameters
            OMRFunctionParam param = new OMRFunctionParam();
            param.FunctionParam = functionParam;

            // Set 3rd party cloud storage server (if any)
            string storage = null;
            string folder = null;

            // Upload source file to aspose cloud storage
            storageApi.PutCreate(inputFileName, "", "", System.IO.File.ReadAllBytes(path + inputFileName));

            // Invoke Aspose.OMR Cloud SDK API
            Com.Aspose.OMR.Model.OMRResponse response = target.PostRunOmrTask(inputFileName, actionName, param, storage, folder);
            return response;
        }

        /// <summary>
        /// Serialize file parameters to JSON string
        /// </summary>
        /// <param name="templateName">Name of the template (should correspond with image name)</param>
        /// <param name="templateData">Template data as a string</param>
        /// <returns>Serialized template file</returns>
        private static string PackTemplate(string templateName, string templateData)
        {
            CorrectionParameter parameter = new CorrectionParameter();
            parameter.Files = new OmrFile[1];

            byte[] bytes = Encoding.UTF8.GetBytes(templateData);

            OmrFile templateFile = new OmrFile();
            templateFile.Name = templateName;
            templateFile.Data = Convert.ToBase64String(bytes);
            templateFile.Size = templateFile.Data.Length;

            parameter.Files[0] = templateFile;

            string output = JsonConvert.SerializeObject(parameter);
            return output;
        }
    }

    /// <summary>
    /// Data model for template correction parameter
    /// </summary>
    public class CorrectionParameter
    {
        public OmrFile[] Files { get; set; }
    }

    /// <summary>
    /// Data model for file transferred as parameter
    /// </summary>
    public class OmrFile
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public string Data { get; set; }
    }
}