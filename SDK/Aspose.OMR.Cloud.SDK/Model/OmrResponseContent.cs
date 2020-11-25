// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Aspose" file="OmrResponseContent.cs">
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

namespace Aspose.Omr.Cloud.Sdk.Model 
{
  using System.Collections.Generic;
  using System.Text;

  /// <summary>
  /// Represents information about part of the text.
  /// </summary>  
  public class OmrResponseContent 
  {                       
        /// <summary>
        /// GUID string that is used to identify template on server This value is assigned after Template Correction and used later in Template Finalization and Image Recognition
        /// </summary>  
        public string TemplateId { get; set; }
		
        /// <summary>
        /// Indicates how long it took to perform task on server.
        /// </summary>  
        public double? ExecutionTime { get; set; }
		
        /// <summary>
        /// This structure holds array of files returned in response Type and content of files differes depending on action
        /// </summary>  
        public List<FileInfo> ResponseFiles { get; set; }
		
        /// <summary>
        /// Gets or sets Info
        /// </summary>  
        public OmrResponseInfo Info { get; set; }
		
        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()  
        {
          var sb = new StringBuilder();
          sb.Append("class OmrResponseContent {\n");
          sb.Append("  TemplateId: ").Append(this.TemplateId).Append("\n");
          sb.Append("  ExecutionTime: ").Append(this.ExecutionTime).Append("\n");
          sb.Append("  ResponseFiles: ").Append(this.ResponseFiles).Append("\n");
          sb.Append("  Info: ").Append(this.Info).Append("\n");
          sb.Append("}\n");
          return sb.ToString();
        }
    }
}
