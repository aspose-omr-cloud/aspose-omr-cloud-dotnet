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
    ///  Class for testing RecognizeTemplateApi
    /// </summary>
    /// <remarks>
    [TestFixture]
    public class RecognizeTemplateApiTests
    {
        private RecognizeTemplateApi instance;

        /// <summary>
        /// Setup before each unit test
        /// </summary>
        [SetUp]
        public void Init()
        {
            string url = Common.GetURL();
            instance = new RecognizeTemplateApi(url);
        }

        /// <summary>
        /// Test RecognizeTemplate
        /// </summary>
        [Test]
        public void RecognizeTemplateTest()
        {
            string omrFilePath = Path.Combine(Common.GetResultFolderDir(), "Aspose_test.omr");
            byte[] omrFile = File.ReadAllBytes(omrFilePath);
            int recognitionThreshold = 30;

            string templatePath = Path.Combine(Common.GetDataFolderDir(), "Aspose_test.jpg");

            byte[] image = File.ReadAllBytes(templatePath);
            List<byte[]> images = new List<byte[]>();
            images.Add(image);

            OmrRecognizeTask task = new OmrRecognizeTask(images, omrFile, recognitionThreshold);

            string recognitionId = instance.PostRecognizeTemplate(task);
            Assert.IsInstanceOf<string>(recognitionId, "response is string");

            OMRRecognitionResponse recognitionResult = new OMRRecognitionResponse();
            while (true)
            {
                recognitionResult = instance.GetRecognizeTemplate(recognitionId);

                if (recognitionResult.ResponseStatusCode == ResponseStatusCode.Ok)
                {
                    break;
                }

                Thread.Sleep(5000);
            }

            Assert.AreEqual("Ok", recognitionResult.ResponseStatusCode.ToString());
            Assert.AreEqual(null, recognitionResult.Error);
            Assert.IsTrue(recognitionResult.Results.Count > 0);
        }
    }
}
