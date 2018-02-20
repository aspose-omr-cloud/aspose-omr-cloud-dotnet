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
namespace Aspose.OMR.TemplateModel
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents omr template
    /// </summary>
    [DataContract]
    public class OmrTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OmrTemplate"/> class
        /// </summary>
        public OmrTemplate()
        {
            this.Pages = new List<OmrPage>();
        }

        /// <summary>
        /// Gets or sets list of pages in template
        /// </summary>
        [DataMember(Order = 3, Name = "Pages")]
        public List<OmrPage> Pages { get; set; }

        /// <summary>
        /// Gets or sets template markup version
        /// </summary>
        [DataMember(Order = 2, Name = "Version")]
        public readonly string Version = "1.0";

        /// <summary>
        /// Gets or sets template name
        /// </summary>
        [DataMember(Order = 1, Name = "Name")]
        public string Name { get; set; }

        [DataMember(Order = 4, Name = "TemplateId")]
        public string TemplateId { get; set; }

        [DataMember(Order = 5, Name = "FinalizationComplete")]
        public bool FinalizationComplete { get; set; }

        [DataMember(Order = 6, Name = "IsGenerated")]
        public bool IsGenerated { get; set; }

        /// <summary>
        /// Adds page to template
        /// </summary>
        /// <returns>Created page</returns>
        public OmrPage AddPage()
        {
            OmrPage newPage =new OmrPage();
            this.Pages.Add(newPage);

            return newPage;
        }
    }
}
