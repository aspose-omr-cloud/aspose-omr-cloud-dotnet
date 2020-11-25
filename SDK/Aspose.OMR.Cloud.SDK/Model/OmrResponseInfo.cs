// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Aspose" file="OmrResponseInfo.cs">
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
  using System.Text;

  /// <summary>
  /// Represents information about OMR result.
  /// </summary>  
  public class OmrResponseInfo 
  {                       
        /// <summary>
        /// String value representing version of the response.
        /// </summary>  
        public string ResponseVersion { get; set; }
		
        /// <summary>
        /// Total amount of processed tasks
        /// </summary>  
        public int? ProcessedTasksCount { get; set; }
		
        /// <summary>
        /// Total amount of successful tasks, i.e. tasks that completed without errors
        /// </summary>  
        public int? SuccessfulTasksCount { get; set; }
		
        /// <summary>
        /// Additional information regarding performed task.
        /// </summary>  
        public OmrResponseDetails Details { get; set; }
		
        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()  
        {
          var sb = new StringBuilder();
          sb.Append("class OmrResponseInfo {\n");
          sb.Append("  ResponseVersion: ").Append(this.ResponseVersion).Append("\n");
          sb.Append("  ProcessedTasksCount: ").Append(this.ProcessedTasksCount).Append("\n");
          sb.Append("  SuccessfulTasksCount: ").Append(this.SuccessfulTasksCount).Append("\n");
          sb.Append("  Details: ").Append(this.Details).Append("\n");
          sb.Append("}\n");
          return sb.ToString();
        }
    }
}
