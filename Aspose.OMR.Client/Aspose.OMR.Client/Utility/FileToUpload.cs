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
    /// <summary>
    /// Represents file for upload on cloud
    /// </summary>
    public class FileToUpload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileToUpload"/> class
        /// </summary>
        /// <param name="name">Name of the file with extension</param>
        /// <param name="fullFilePath">Full path to the file</param>
        /// <param name="size">Size of the image in KB</param>
        public FileToUpload(string name, string fullFilePath, long size)
        {
            this.Name = name;
            this.FullPath = fullFilePath;
            this.Size = size;
        }
        /// <summary>
        /// Gets or sets the file name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the file's location
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Gets or sets the image size in KB
        /// </summary>
        public long Size { get; set; }
    }
}
