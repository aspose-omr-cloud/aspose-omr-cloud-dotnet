/*
 * Copyright (C) 2023 Aspose Pty Ltd. All Rights Reserved.
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

using System.Collections.Generic;
using System.IO;
using System.Threading;

using NUnit.Framework;

using Aspose.Omr.Cloud.Sdk.Api;
using Aspose.Omr.Cloud.Sdk.Model;

namespace Aspose.Omr.Cloud.Sdk.Test
{
    /// <summary>
    ///  Class for testing GenerateTemplateApi
    /// </summary>
    [TestFixture]
    public class GenerateTemplateApiTests
    {
        private GenerateTemplateApi instance;

        /// <summary>
        /// Setup before each unit test
        /// </summary>
        [SetUp]
        public void Init()
        {
            string url = Common.GetURL();
            instance = new GenerateTemplateApi(url);
        }
       
        /// <summary>
        /// Test GenerateTemplate
        /// </summary>
        [Test]
        public void GenerateTemplateTest()
        {
            string[] templateLogosImagesNames = new string[] { "logo1.jpg", "logo2.png" };
            string pathFile = Path.Combine(Common.GetDataFolderDir(), "Aspose_test.txt");
            byte[] markupFile = File.ReadAllBytes(pathFile);

            PageSettings settings = new PageSettings();
           
            Dictionary<string, byte[]> images = new Dictionary<string, byte[]>();
            if (templateLogosImagesNames != null)
            {
                for (int i = 0; i < templateLogosImagesNames.Length; i++)
                {
                    byte[] logo = File.ReadAllBytes(Path.Combine(Common.GetDataFolderDir(), templateLogosImagesNames[i]));
                    string name = Path.GetFileName(templateLogosImagesNames[i]);
                    images.Add(name, logo);
                }
            }

            OmrGenerateTask task = new OmrGenerateTask(markupFile, settings, images);
            string templateId = instance.PostGenerateTemplate(task);

            Assert.IsInstanceOf<string>(templateId, "response is string");

            OMRResponse generationResult = new OMRResponse();
            while (true)
            {
                generationResult = instance.GetGenerateTemplate(templateId);

                if (generationResult.ResponseStatusCode == ResponseStatusCode.Ok)
                {
                    break;
                }

                Thread.Sleep(5000);
            }

            Assert.AreEqual("Ok", generationResult.ResponseStatusCode.ToString());
            Assert.AreEqual(null, generationResult.Error);
            Assert.IsTrue(generationResult.Results.Count > 0);
        }
    }
}
