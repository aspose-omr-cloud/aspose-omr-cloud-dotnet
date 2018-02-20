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
using System.Text;

namespace Aspose.OMR.Cloud.SDK.Model
{
    public class FileInfo
    {
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets file's size
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets file's data
        /// </summary>
        public byte[] Data { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class FilesInfo {\n");
            sb.Append("  Name: ").Append(this.Name).Append("\n");
            sb.Append("  Size: ").Append(this.Size).Append("\n");

            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
