
// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Aspose" file="PostRunOmrTaskRequest.cs">
//   Copyright (c) 2019 Aspose.Omr for Cloud
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
  using Aspose.Omr.Cloud.Sdk.Model; 

  /// <summary>
  /// Request model for <see cref="Aspose.Omr.Cloud.Sdk.Api.Omr.PostRunOmrTask" /> operation.
  /// </summary>  
  public class PostRunOmrTaskRequest  
  {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostRunOmrTaskRequest"/> class.
        /// </summary>        
        public PostRunOmrTaskRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostRunOmrTaskRequest"/> class.
        /// </summary>
        /// <param name="name">Name of the file to recognize.</param>
        /// <param name="actionName">Action name [&#39;CorrectTemplate&#39;, &#39;FinalizeTemplate&#39;, &#39;RecognizeImage&#39;, &#39;GenerateTemplate&#39;]</param>
        /// <param name="param">Function params, specific for each actionName</param>
        /// <param name="storage">Image&#39;s storage.</param>
        /// <param name="folder">Image&#39;s folder.</param>
        public PostRunOmrTaskRequest(string name, string actionName, OmrFunctionParam param, string storage = null, string folder = null)             
        {
            this.name = name;
            this.actionName = actionName;
            this.param = param;
            this.storage = storage;
            this.folder = folder;
        }

        /// <summary>
        /// Name of the file to recognize.
        /// </summary>  
        public string name { get; set; }

        /// <summary>
        /// Action name ['CorrectTemplate', 'FinalizeTemplate', 'RecognizeImage', 'GenerateTemplate']
        /// </summary>  
        public string actionName { get; set; }

        /// <summary>
        /// Function params, specific for each actionName
        /// </summary>  
        public OmrFunctionParam param { get; set; }

        /// <summary>
        /// Image's storage.
        /// </summary>  
        public string storage { get; set; }

        /// <summary>
        /// Image's folder.
        /// </summary>  
        public string folder { get; set; }
  }
}
