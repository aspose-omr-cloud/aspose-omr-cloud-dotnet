// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Aspose" file="DeleteFileRequest.cs">
//   Copyright (c) 2020 Aspose.Omr for Cloud
// </copyright>
// <summary>
//   Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Aspose.Omr.Cloud.Sdk.Model.Requests
{
    /// <summary>
    /// Request model for <see cref="Aspose.Omr.Cloud.Sdk.Api.OmrApi.DeleteFile" /> operation.
    /// </summary>  
    public class DeleteFileRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFileRequest"/> class.
        /// </summary>        
        public DeleteFileRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFileRequest"/> class.
        /// </summary>
        /// <param name="path">File path e.g. &#39;/folder/file.ext&#39;</param>
        /// <param name="storageName">Storage name</param>
        /// <param name="versionId">File version ID to delete</param>
        public DeleteFileRequest(string path, string storageName = null, string versionId = null)
        {
            this.path = path;
            this.storageName = storageName;
            this.versionId = versionId;
        }

        /// <summary>
        /// File path e.g. '/folder/file.ext'
        /// </summary>  
        public string path { get; set; }

        /// <summary>
        /// Storage name
        /// </summary>  
        public string storageName { get; set; }

        /// <summary>
        /// File version ID to delete
        /// </summary>  
        public string versionId { get; set; }
    }
}
