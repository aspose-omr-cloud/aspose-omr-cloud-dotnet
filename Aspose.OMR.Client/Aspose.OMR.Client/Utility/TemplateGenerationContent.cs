/*
 * Copyright (c) 2017 Aspose Pty Ltd. All Rights Reserved.
 *
 * Licensed under the MIT (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *       https://github.com/asposecloud/Aspose.OMR-Cloud/blob/master/LICENSE
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace Aspose.OMR.Client.Utility
{
    using ViewModels;

    /// <summary>
    /// Represents content of template generation response
    /// </summary>
    public class TemplateGenerationContent
    {
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the generated template
        /// </summary>
        public TemplateViewModel Template { get; set; }

        /// <summary>
        /// Gets or sets the generated image as byte array
        /// </summary>
        public byte[] ImageData { get; set; }
    }
}
